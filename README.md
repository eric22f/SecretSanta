# SecretSanta
There are other free Secret Santa websites out there, but this will allow you to create your own Secret Santa website so that you do no thave to worry about your information being shared.

## Technologies
 - Visual Studio .net core
 - SQL Server

### Basics
 - Once you have the database setup you will need to add users manually to the SecretSantas table
   - Create a random UserCode so that users can get access and do not share that code with others
 - If you have multiple teams then you can add those teams to the SantasTeams database and as you add a SecretSanta then designate which team they are on
 - After you have added all Secret Santas added then on the website there is a link to "Randomize" on the footer
   - Once you click this link any Secret Santas that have not been designated will be randomly selected
     - The site will first randomly designate those who are not currently a Secret Santa and then with those that are
   - If the site is unable to designate all Secret Santas then it will let you know
     - It is recommended to clear the table SelectedSantas and run the randomizer again if there is a problem
   - Running the Randomizer will not change anyone who has already be designated as a Secret Santa so you can always add new users and run the Randomizer again
     - But it would be preferred to clear the SelectedSantas table as the Randomizer will only use the new Secret Santas that have been added 

### License
 - MIT License
   - You are free to do with this code what you would like



