using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductsAPI.Data;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{

    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private readonly IUserSession _userSession;

        private readonly IConfiguration _configuration;
        private readonly MainDbContext _mainDbContext;

        public RolesController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, MainDbContext mainDbContext, IUserSession userSession)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _mainDbContext = mainDbContext;
            _userSession = userSession;

        }



        [HttpPost("admin-assign")]
        public async Task<IActionResult> AssignAdminRole(Guid userId)
        {
            var user = await _mainDbContext.Users
            .Where(x => x.Id == userId)
            .SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (this._userSession.IsAdmin != 1) {
                return Forbid("Only admin can assign roles.");
            }

            user.IsAdmin = 1;

            _ = await _mainDbContext.SaveChangesAsync();
            return Ok("Admin role successfully assigned");
        }

    }
}