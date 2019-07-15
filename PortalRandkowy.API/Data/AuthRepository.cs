using System;
using System.Text;
using System.Threading.Tasks;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        
        #region method public
        public AuthRepository(DataContext context)
        {
            this.context = context;
        }
        public Task<User> Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHashSalt(password, out passwordHash, out passwordSalt);    
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;        
            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();
            return user;
        }
        public Task<bool> UserExist(string username)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region method private
        private void CreatePasswordHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }           
        }        
        #endregion
    }
}