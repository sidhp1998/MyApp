using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    
    public class UserController : BaseApiController
    {

        public readonly DataContext _dataContext;
        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUsers(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);
            if(user==null)
            {
                return NotFound();
            }
            return Ok(user);

        }

        [HttpGet("AllUsers")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUsers()
        {
            var users = await _dataContext.Users.ToListAsync();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);

        }
    }
}
