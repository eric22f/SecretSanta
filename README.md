# SecretSanta
There are other free Secret Santa websites out there, but this will allow you to create your own Secret Santa website so that you do not thave to worry about your information being shared.

## Technologies
 - Visual Studio .net core
 - SQL Server

### Basics
 - Once you have the database setup you will need to add users manually to the SecretSantas table
   - Create a random UserCode so that users can get access and then share this code with only that Secret Santa
 - If you have multiple teams or groups then you can add those teams to the SantasTeams table and as you add a SecretSanta then designate which team they are on
   - If you do not have multiple teams then you can set the TeamId to zero
   - Or if someone is a Secret Santa for multiple teams then you will need to create multiple user accounts for that Secret Santa
 - After all Secret Santas have been added then on the footer of the website there is a link to "Randomize"
   - Once you click this link any Secret Santas that have not been designated will be randomly made or selected as a Secret Santa
     - The site will first randomly designate those who are not currently a Secret Santa and then with those that are
   - If the site is unable to designate all Secret Santas then it will let you know
     - You can clear the table SelectedSantas and run the randomizer again if there is a problem
       - However, do not do this if Secret Santas have already started shoping or using the site
   - Running the Randomizer will not change anyone who has already be designated as a Secret Santa so you can always add new users and run the Randomizer again
     - But it would be preferred to clear the SelectedSantas table as the Randomizer will only use the new Secret Santas that have been added 

### License
 - MIT License
   - You are free to do with this code what you would like



