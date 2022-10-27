using CRUD.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [AllowAnonymous]
    public class EmailController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        public EmailController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string token,[FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Ok(new Response { Message = "User Not Found", Status = "Error" });

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Ok(new Response { Message = "ConfirmEmail", Status = "Success" });
            }
            else
            {
                return Ok(new Response { Message = "Email Is Not Confirm", Status = "Error" });
            }
        }
    }
}
