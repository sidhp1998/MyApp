using Microsoft.AspNetCore.Mvc;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        public readonly DataContext _dataContext;
        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

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
