using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUD.Model.Models
{
    public class Register
    {
        [Required(ErrorMessage = "User Name Is Required")]
        [DisplayName("User Name")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; } = string.Empty;
    }
}
