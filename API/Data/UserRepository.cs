using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
            
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(p=> p.Photos).SingleOrDefaultAsync(x=> x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            //If the result is greater than 0, it means that changes were successfully saved, and the method returns true. 
            //Otherwise, if no changes were made (result is 0), the method returns false.
            return await _context.SaveChangesAsync()>0;
        }

        public void Update(AppUser user)
        {
            //This indicates to the context that the entity has been modified and should be updated in the database 
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}