using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        
        private readonly UserRepository _userRepository;
        public UsersController(UserRepository userRepository){
            _userRepository = userRepository;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers(){
            return Ok(await _userRepository.GetUsersAsync());
        }


        [HttpGet("{username}")]
        public async Task<ActionResult<AppUser>> GetUser(string username){
            return await _userRepository.GetUserByUsernameAsync(username);
        }
    }
}