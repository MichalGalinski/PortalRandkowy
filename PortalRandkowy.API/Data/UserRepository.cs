using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Data
{
    public class UserRepository : GenericRepository, IUserRepository 
    {
        private readonly DataContext context;
        public UserRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        
        
        public async Task<User> GetUser(int id)
        {
            var user = await this.context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await this.context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await this.context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }
    }
}