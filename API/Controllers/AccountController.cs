
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;

        }

        [HttpPost("register")] //Post: api/account/register

        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            // This creates an instance of the HMACSHA512 class, which is a cryptographic hash function.
            // It will be used to generate the hash and salt for the user's password.
            using var hmac = new HMACSHA512();

            // Creates a new instance of the AppUser class
            var user = new AppUser
            {
                UserName = username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            // Adds the newly created user object to the database context's Users table
            _context.Users.Add(user);

            // Saves the changes made to the database
            await _context.SaveChangesAsync();

            // Returns the newly created user object as the response of the API request.
            return user;
        }
    }
}