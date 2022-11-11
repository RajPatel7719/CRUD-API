using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRUD_API.Const
{
    public static class Constants
    {
        public const string StatusError = "Error";
        public const string StatusSuccess = "Success";
        public const string StatusUnauthorized = "Unauthorized";
        public const string UserNotFound = "User Not Found...!";
        public const string UserNotAuthorized = "You are Not Authorized";
        public const string UserAlreadyExists = "User already exists...!";
        public const string UserCreateSuccess = "User Created Successfully...!";
        public const string UserCreationFaield = "User creation failed! Please check user details and try again...!";
        public const string UserProfileUpdateSuccess = "User Profile Update Successfully...!";
        public const string ImageUploadSuccess = "Upload Successfully...!";

        public static class Message
        {
            public const string EmailNotConfirm = "Email is unconfirmed, please confirm it first...!";
            public const string EmailConfirmed = "Email is confirmed...!";
        }
    }
}
