using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("bad-request")] // https://url/bad-request  => 400 Bad Request Error
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("This was bad request");
        }

        [Authorize] //endpoint requires authentication
        [HttpGet("auth")] // https://url/auth => 401 Unauthorize Error
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }


        [HttpGet("not-found")] // https://url/not-found => 404 Not Found Error
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = _context.Users.Find(-1); // find a user with an ID of -1
            //  if no user with ID -1 was found in the database, returns a 404 Not Found response to the client.
            if (thing == null) return NotFound();
            // If the thing variable is not null, it means a user with ID -1 was found, and it is returned as the response body.
            return thing;
        }

        [HttpGet("server-error")] // https://url/server-error  => 500 Server Error
        public ActionResult<string> GetServerError()
        {
            var thing = _context.Users.Find(-1);
            var thingToReturn = thing.ToString();
            return thingToReturn;
        }

    }
}