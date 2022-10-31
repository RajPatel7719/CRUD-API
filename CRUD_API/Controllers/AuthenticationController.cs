using CRUD.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost(Name = "Register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            var userExist = await _userManager.FindByNameAsync(register.UserName);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Register { Status = "Error", Message = "User Already Exist." });
            AppUser user = new AppUser()
            {
                UserName = register.UserName,
                Email = register.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Register { Status = "Error", Message = "User Creation Faield, Please Try Again." });
            
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var token = Base64UrlEncoder.Encode(code);

            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmail(user.Email, token);

            if (emailResponse)
                new Register { Status = "Success", Message = "User Created Successfully...!" };
            else
            {
                // log email failed 
            }
            return Ok(new Register { Status = "Success", Message = "User Created Successfully...!" });
        }

        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                bool emailStatus = await _userManager.IsEmailConfirmedAsync(user);
                if (emailStatus == false)
                {
                    return Ok(new Login
                    {
                        Status = "Error",
                        Message = "Email is unconfirmed, please confirm it first"
                    });
                }
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new Login
                {
                    Status = "Success",
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = token.ValidTo.ToString()
                });
            }
            return Unauthorized(new Response { Status = "Unauthorized", Message = "You are Not Authorized", Token = "" });
        }

        [HttpPost]
        [Route("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            AppUser user = new AppUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            ApiResponse<object> response = new();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    response.ErrorMessage = "SUCCESS";
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Result = new();
                }
                else
                {
                    response.ErrorMessage = "Error";
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Result = new();
                }
            }
            else
            {
                response.ErrorMessage = "User Not Found";
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Result = new();
            }
            return Ok(response);
        }
    }
}
