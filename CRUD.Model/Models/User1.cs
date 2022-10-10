using System;
using System.Collections.Generic;

namespace CRUD.Model.Models
{
    public partial class User1
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool? Gender { get; set; }
    }
}
