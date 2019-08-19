using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Data
{
    public class Seed
    {
        private readonly DataContext context;
        public Seed(DataContext context)
        {
            this.context = context;
        }

        public void SeedUsers()
        {
            if(!this.context.Users.Any())
            {
                var userData = File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach(var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHashSalt("password", out passwordHash, out passwordSalt);   
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();
                    this.context.Users.Add(user);
                }
                this.context.SaveChanges();
            }            
        }

        private void CreatePasswordHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }           
        } 
    }
}