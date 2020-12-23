using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecretSanta.Models.Services
{
    public class SantasHelper
    {

        public string InfoMsg { private set; get; }

        public MySecretSanta GetSantasFromUserCode(string userCode)
        {
            var db = new SantaContext();

            if (!GetSantas(db, userCode, out SecretSanta santa, out SecretSanta myAssignment))
            {
                if (santa != null && myAssignment == null)
                {
                    var noSanta = new MySecretSanta()
                    {
                        MySantaFullName = "Not yet assigned",
                        SantaFullName = santa.FullName,
                        IsViewedBySecretSanta = false,
                        InfoMsg = "You can review your address, gift hints, or gift topics."
                    };
                    return noSanta;
                }
                return null;
            }

            var returnSanta = new MySecretSanta()
            {
                Address = myAssignment.AddressAndNotes,
                MySantaFullName = myAssignment.FullName,
                SantaFullName = santa.FullName,
                IsViewedBySecretSanta = myAssignment.Selected,
                IsGiftSent = myAssignment.SentGift,
                InfoMsg = santa.Selected ? StringConstants.SantaHasViewedMyInfo : StringConstants.SantaHasNotViewedMyInfo
            };
            if (santa.SentGift)
            {
                returnSanta.InfoMsg = StringConstants.SantaSentMyGift;
            }

            // Save that this user has been viewed
            if (!myAssignment.Selected)
            {
                myAssignment.Selected = true;
                db.SaveChanges();
            }

            return returnSanta;
        }

        public MySecretSanta GeMyProfileFromUserCode(string userCode)
        {
            var db = new SantaContext();

            // Get current Secret Santa
            var santa = db.SecretSantas.Where(s => s.UserCode == userCode).FirstOrDefault();
            if (santa == null)
            {
                InfoMsg = StringConstants.InvalidSecretCode;
                return null;
            }

            var returnSanta = new MySecretSanta()
            {
                Address = santa.AddressAndNotes,
                SantaFullName = santa.FullName,
                InfoMsg = santa.Selected ? StringConstants.SantaHasViewedMyInfo : StringConstants.SantaHasNotViewedMyInfo
            };
            if (santa.SentGift)
            {
                returnSanta.InfoMsg = StringConstants.SantaSentMyGift;
            }
            return returnSanta;
        }

        public MySecretSanta SaveMyInfo(MySecretSanta user)
        {
            var db = new SantaContext();

            // Get current Secret Santa
            var myInfo = db.SecretSantas.Where(s => s.UserCode == user.UserCode).FirstOrDefault();
            if (myInfo == null)
            {
                InfoMsg = StringConstants.InvalidSecretCode;
                return null;
            }
            user.SantaFullName = myInfo.FullName;

            try
            {
                // Update Address plus gift hints
                if (!string.IsNullOrWhiteSpace(user.Address) && user.Address.Trim() != myInfo.AddressAndNotes)
                {
                    myInfo.AddressAndNotes = user.Address.Trim();
                    db.SaveChanges();
                }
                else
                {
                    // No changes
                    if (myInfo.SentGift)
                    {
                        user.InfoMsg = StringConstants.SantaSentMyGift;
                    }
                    else
                    {
                        user.InfoMsg = myInfo.Selected ? StringConstants.SantaHasViewedMyInfo : StringConstants.SantaHasNotViewedMyInfo;
                    }

                    return user;
                }
            }
            catch (Exception ex)
            {
                user.InfoMsg = ex.ToString();
                return user;
            }

            user.InfoMsg = "Your information has been saved.";
            return user;
        }

        /// <summary>
        /// Randomize any Secret Santas that have not been selected
        /// </summary>
        public SecretSantaUser RandomizeSecretSantas()
        {
            var results = new SecretSantaUser();

            var db = new SantaContext();

            var santas = db.SecretSantas.Where(s => !db.SelectedSantas.Select(a => a.SantaId).Contains(s.SantaId)).Randomize().ToList();

            var users = db.SecretSantas.Where(s => !db.SelectedSantas.Select(a => a.SelectedSantaId).Contains(s.SantaId)).Randomize().ToList();

            // Currently secret santa selections in the database
            var selected = db.SelectedSantas.ToList();

            int count = santas.Count;

            // Interate through each available santa in reverse
            // Remove santas and users as they are matched
            for (int i = santas.Count - 1; i >= 0 && users.Count > 0; i--)
            {
                var santa = santas[i];
                int matchingSantaIndex = -1;
                int previousMatchingIndex = -1;
                for (int ii = 0; ii < users.Count; ii++)
                {
                    var user = users[ii];
                    // Must be on the same team or not the same person
                    if (user.TeamId == santa.TeamId && user.SantaId != santa.SantaId)
                    {
                        // We have a match
                        matchingSantaIndex = ii;
                        // Start by matching someone that has not a secret santa
                        if (!selected.Any(s => s.SantaId == user.SantaId))
                        {
                            // Good match
                            break;
                        }
                        // If this user is our secret santa then go to the previous match
                        if (previousMatchingIndex >= 0 && !selected.Any(s => s.SantaId == user.SantaId && s.SelectedSantaId == santa.SantaId))
                        {
                            // Better to use our previous match that was not our secret santa
                            matchingSantaIndex = previousMatchingIndex;
                        }
                        else
                        {
                            // Capture this index in-case we need o revert back to it
                            previousMatchingIndex = matchingSantaIndex;
                        }
                    }
                }
                // Check for a match
                if (matchingSantaIndex >= 0)
                {
                    var match = new SelectedSanta() { SantaId = santa.SantaId, SelectedSantaId = users[matchingSantaIndex].SantaId };
                    selected.Add(match);
                    db.SelectedSantas.Add(match);
                    santas.RemoveAt(i);
                    users.RemoveAt(matchingSantaIndex);
                }
            }            
            db.SaveChanges();

            if (santas.Count > 0 || users.Count > 0)
            {
                if (santas.Count == 1 && users.Count == 1 && santas[0].SantaId == users[0].SantaId)
                {
                    results.InfoMsg = $"Unable to match all Santas.  Total Santa matches = {selected.Count}.  One Santa remains unmatched.";
                }
                else
                {
                    results.InfoMsg = $"Unable to match all Santas.  Total Santa matches = {selected.Count}  Santa's remaining = {santas.Count}  Users remaining = {users.Count}";
                }
            }
            else
            {
                if (selected.Count == 0)
                {
                    results.InfoMsg = "There are not any Secret Santas in the database";
                }
                else if (selected.Count == 1)
                {
                    results.InfoMsg = "One Santa has been matched as a Secret Santa";
                }
                else
                {
                    results.InfoMsg = $"All {selected.Count} Santas have been matched as a Secret Santa";
                }
            }

            return results;
        }

        public bool SetSantaSentGift(string userCode, bool isSent)
        {
            var db = new SantaContext();

            if (!GetSantas(db, userCode, out SecretSanta santa, out SecretSanta myAssignment))
            {
                return false;
            }

            myAssignment.SentGift = isSent;
            db.SaveChanges();

            return true;
        }

        private bool GetSantas(SantaContext db, string userCode, out SecretSanta santa, out SecretSanta myAssignment)
        {
            myAssignment = null;
            // Get current Secret Santa
            santa = db.SecretSantas.Where(s => s.UserCode == userCode).FirstOrDefault();
            if (santa == null)
            {
                InfoMsg = StringConstants.InvalidSecretCode;
                return false;
            }

            // Get who they are Secret Santa for
            int santaId = santa.SantaId;
            var selected = db.SelectedSantas.Where(s => s.SantaId == santaId).FirstOrDefault();
            if (selected == null)
            {
                InfoMsg = "You have not been assigned as a Secret Santa yet.";
                return false;
            }

            // Select the person they are assigned to
            myAssignment = db.SecretSantas.Where(s => s.SantaId == selected.SelectedSantaId).FirstOrDefault();
            if (myAssignment == null)
            {
                InfoMsg = $"Your assigned persion with an ID of {selected.SantaId} is missing.  Please contact your administrator.";
                return false;
            }
            return true;
        }
    }
}
