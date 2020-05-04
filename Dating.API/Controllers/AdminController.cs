using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dating.API.Data;
using System.Threading.Tasks;

namespace Dating.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;
        public AdminController(DataContext context)
        {
            _context = context;
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _context.Users.Orderby(x => x.UserName)
            return Ok("Only admins can see this");
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photosForModeration")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admin and Moderator can see this");
        }
    }
}