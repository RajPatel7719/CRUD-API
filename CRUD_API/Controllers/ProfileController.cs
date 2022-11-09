using CRUD.BusinessLogic.IRepository;
using CRUD.Model.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly IImageUpload _imageUpload;
        public ProfileController(UserManager<AppUser> userManager, IUserRepositoryAsync userRepositoryAsync, IImageUpload imageUpload)
        {
            _userManager = userManager;
            _userRepositoryAsync = userRepositoryAsync;
            _imageUpload = imageUpload;
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture([FromForm] Register user)
        {
            var image = await _imageUpload.SaveImage(user.ProfileImage, user.UserName);
            user.ProfilePicture = image;
            await EditProfile(user);
            return Ok(new Register { Message = "Upload Successfully", Status = "Success"});
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile([FromBody] Register register)
        {
            var userExist = await _userManager.FindByNameAsync(register.UserName);
            if (userExist == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Register { Status = "Error", Message = "User Doesn't Exist." });
            AppUser user = new AppUser()
            {
                UserName = register.UserName,
                Email = register.Email,
                TwoFactorEnabled = register.TwoFactorEnabled,
                ProfilePicture = register.ProfilePicture == "" ? userExist.ProfilePicture : register.ProfilePicture,
                ImageData = register.ImageData
            };
            await _userRepositoryAsync.UpdateAsync(userExist.Id, user);
            
            return Ok(new Register { Status = "Success", Message = "User Profile Update Successfully!" });
        }
    }
}
