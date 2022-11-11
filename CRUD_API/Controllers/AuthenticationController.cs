using CRUD.BusinessLogic.IRepository;
using CRUD.BusinessLogic.Repository;
using CRUD.Model.Models;
using CRUD_API.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly IImageUpload _imageUpload;

        public AuthenticationController(UserManager<AppUser> userManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IConfiguration configuration,
                                        IUserRepositoryAsync userRepositoryAsync,
                                        IImageUpload imageUpload)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _userRepositoryAsync = userRepositoryAsync;
            _imageUpload = imageUpload;
        }

        [HttpPost(Name = "Register")]
        public async Task<IActionResult> Register([FromBody] Register register)
        {
            var userExist = await _userManager.FindByNameAsync(register.UserName);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new Register { Status = Constants.StatusError, Message = Constants.UserAlreadyExists });
            AppUser user = new AppUser()
            {
                UserName = register.UserName,
                Email = register.Email,
                TwoFactorEnabled = true,
                ProfilePicture = register.ProfilePicture,
                ImageData = register.ImageData
            };
            user.ProfilePicture = user.Id;
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new Register { Status = Constants.StatusError, Message = Constants.UserCreationFaield });
            //var image = await _imageUpload.SaveImage(register.ProfileImage, user.Email);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var token = Base64UrlEncoder.Encode(code);

            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmail(user.Email, token);

            if (emailResponse)
                new Register { Status = Constants.StatusSuccess, Message = Constants.UserCreateSuccess };
            else
            {
                // log email failed 
            }
            return Ok(new Register { Status = Constants.StatusSuccess, Message = Constants.UserCreateSuccess });
        }

        [HttpPost(Name = "Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password) && user.TwoFactorEnabled != false)
            {
                bool emailStatus = await _userManager.IsEmailConfirmedAsync(user);
                if (emailStatus == false)
                {
                    return Ok(new Login
                    {
                        Status = Constants.StatusError,
                        Message = Constants.Message.EmailNotConfirm
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
                    Status = Constants.StatusSuccess,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Message = token.ValidTo.ToString()
                });
            }
            return Unauthorized(new Response { Status = Constants.StatusUnauthorized, Message = Constants.UserNotAuthorized, Token = "" });
        }

        [HttpPost]
        [Route("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register model)
        {
            var userExists = await _userManager.FindByNameAsync(model.UserName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Constants.StatusError, Message = Constants.UserAlreadyExists });

            AppUser user = new AppUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Constants.StatusError, Message = Constants.UserCreationFaield });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new Response { Status = Constants.StatusSuccess, Message = Constants.UserCreateSuccess });
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
                    response.ErrorMessage = Constants.StatusSuccess;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Result = new();
                }
                else
                {
                    response.ErrorMessage = Constants.StatusError;
                    response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Result = new();
                }
            }
            else
            {
                response.ErrorMessage = Constants.UserNotFound;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Result = new();
            }
            return Ok(response);
        }

        [HttpGet(Name = "GetUsersProfile")]
        public async Task<IActionResult> GetUsersProfile()
        {
            ApiResponse<IEnumerable<Register>> response = new();
            try
            {
                response.Result = await _userRepositoryAsync.GetAllUsersProfileAsync();
                response.StatusCode = StatusCodes.Status200OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.Message;
                return Ok(response);
            }
        }

        [HttpGet(Name = "GetUserByEmail")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            ApiResponse<Register> response = new();
            try
            {
                response.Result = await _userRepositoryAsync.GetUserByEmailAsync(email);
                response.StatusCode = StatusCodes.Status200OK;
                response.ErrorMessage = Constants.StatusSuccess;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.Message;
                return Ok(response);
            }
        }
    }
}
