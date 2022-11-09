using Microsoft.AspNetCore.Identity;

namespace CRUD.Model.Models
{
    public class AppUser : IdentityUser
    {
        public string ProfilePicture { get; set; } = string.Empty;
        public byte[]? ImageData { get; set; }
    }
}
