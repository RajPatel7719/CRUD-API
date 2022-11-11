using CRUD.Model.Models;
using CRUD_API.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
                return Ok(new Response { Message = Constants.UserNotFound, Status = Constants.StatusError });
            var code = Base64UrlEncoder.Decode(token);
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Ok(new Response { Message = Constants.Message.EmailConfirmed, Status = Constants.StatusSuccess });
            }
            else
            {
                return Ok(new Response { Message = Constants.Message.EmailNotConfirm, Status = Constants.StatusError });
            }
        }
    }
}
