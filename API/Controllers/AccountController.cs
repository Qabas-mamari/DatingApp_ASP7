
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ActionResult<AppUser>> Register(RegisterDtos registerDtos)
        {
            if(await UserExists(registerDtos.Username)) return BadRequest("Username taken");

            // This creates an instance of the HMACSHA512 class, which is a cryptographic hash function.
            // It will be used to generate the hash and salt for the user's password.
            using var hmac = new HMACSHA512();

            // Creates a new instance of the AppUser class
            var user = new AppUser
            {
                UserName = registerDtos.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDtos.Password)),
                PasswordSalt = hmac.Key
            };

            // Adds the newly created user object to the database context's Users table
            _context.Users.Add(user);

            // Saves the changes made to the database
            await _context.SaveChangesAsync();

            // Returns the newly created user object as the response of the API request.
            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDtos loginDtos){
            var user = await _context.Users.SingleOrDefaultAsync(x=> x.UserName == loginDtos.Username);

            if(user == null) return Unauthorized("Invalid Username");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDtos.Password));

            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if(user.PasswordHash[i] != ComputedHash[i]) return Unauthorized("Invalid Password");
            }

            return user; 

        }

        private async Task<bool> UserExists(string username){
            // Asynchronously determines whether any element of a sequence satisfies a condition.
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}