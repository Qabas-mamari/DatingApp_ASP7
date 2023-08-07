using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper){
            _mapper = mapper;
            _userRepository = userRepository;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers(){
           var users = await _userRepository.GetUsersAsync();

           var userToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

           return Ok(userToReturn);
        }


        [HttpGet("{username}")]
        /**
        The await keyword is used to asynchronously wait for the completion of the FindAsync operation.
         By marking the method as async and returning a Task, it allows the method to be awaited by the calling code.
        */
        public async Task<ActionResult<MemberDto>> GetUser(string username){
            var user =  await _userRepository.GetUserByUsernameAsync(username);
            return _mapper.Map<MemberDto>(user);
        }
    }
}