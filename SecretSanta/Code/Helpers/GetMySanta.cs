using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Models.Services
{
    public class GetMySanta
    {

        public string InfoMsg { private set; get; }

        public MySecretSanta GetSantasFromUserCode(string userCode)
        {
            var db = new SantaContext();

            // Get current Secret Santa
            var santa = db.SecretSantas.Where(s => s.UserCode == userCode).FirstOrDefault();
            if (santa == null)
            {
                InfoMsg = StringConstants.InvalidSecretCode;
                return null;
            }

            // Get who they are Secret Santa for
            var selected = db.SelectedSantas.Where(s => s.SantaId == santa.SantaId).FirstOrDefault();
            if (selected == null)
            {
                InfoMsg = StringConstants.YouHaveNotBeenAssigned;
                return null;
            }

            // Select that Santa
            var myAssignment = db.SecretSantas.Where(s => s.SantaId == selected.SelectedSantaId).FirstOrDefault();
            if (myAssignment == null)
            {
                InfoMsg = $"Your assigned persion with an ID of {selected.SantaId} is missing.  Please contact your administrator.";
                return null;
            }

            var returnSanta = new MySecretSanta()
            {
                Address = myAssignment.AddressAndNotes,
                MySantaFullName = myAssignment.FullName,
                SantaFullName = santa.FullName,
                IsViewedBySecretSanta = myAssignment.Selected,
                IsGiftSent = santa.SentGift,
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


    }
}
