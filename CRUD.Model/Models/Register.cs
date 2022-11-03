using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUD.Model.Models
{
    public class Register
    {
        [DisplayName("User Name")]
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public bool TwoFactorEnabled { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
