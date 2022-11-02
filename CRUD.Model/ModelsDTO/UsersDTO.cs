namespace CRUD.Model.ModelsDTO
{
    public partial class UserDTO
    {
        public string FName { get; set; } = null!;
        public string? LName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool? Gender { get; set; }
    }
}
