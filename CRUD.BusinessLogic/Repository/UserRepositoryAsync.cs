using CRUD.BusinessLogic.IRepository;
using CRUD.DataAccess;
using CRUD.Model.Models;
using Dapper;
using System.Data;

namespace CRUD.BusinessLogic.Repository
{
    public class UserRepositoryAsync : IUserRepositoryAsync
    {
        private readonly UserContext _userContext;

        public UserRepositoryAsync(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<User1> CreateUserAsync(User1 user)
        {
            var query = "INSERT INTO [dbo].[Users] VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @Gender)" +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("FirstName", user.FirstName, DbType.String);
            parameters.Add("LastName", user.LastName, DbType.String);
            parameters.Add("PhoneNumber", user.PhoneNumber, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("Gender", user.Gender, DbType.String);

            using (var connection = _userContext.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdSchool = new User1
                {
                    Id = id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Gender = user.Gender
                };

                return createdSchool;
            }
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            var query = "DELETE FROM [dbo].[Users] WHERE ID = @Id";

            using (var conn = _userContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, new { id });
            }

        }

        public async Task<IEnumerable<User1>> GetAllUsersAsync()
        {
            var query = "SELECT * FROM [dbo].[Users]";
            using (var conn = _userContext.CreateConnection())
            {
                var users = await conn.QueryAsync<User1>(query);
                return users.ToList();
            }
        }

        public async Task<User1> GetUserByIdAsync(int id)
        {
            var query = "SELECT * FROM [dbo].[Users] WHERE ID = @id";
            var param = new DynamicParameters();
            param.Add("id", id, DbType.Int32);
            using (var conn = _userContext.CreateConnection())
            {
                var user = await conn.QueryFirstOrDefaultAsync<User1>(query);
                return user;
            }
        }

        public async Task UpdateUserAsync(int id, User1 user)
        {
            var query = "UPDATE [dbo].[Users] SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber, Email = @Email, Gender = @Gender WHERE ID = @id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("FirstName", user.FirstName, DbType.String);
            parameters.Add("LastName", user.LastName, DbType.String);
            parameters.Add("PhoneNumber", user.PhoneNumber, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("Gender", user.Gender, DbType.String);

            using (var conn = _userContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, parameters);
            }
        }

        public async Task CreateUserLoginAsync(UserLogin user)
        {
            var procedure = "RegisterUser";
            var parameter = new DynamicParameters();
            parameter.Add("Email", user.Email, DbType.String, ParameterDirection.Input);
            parameter.Add("Pass", user.Password, DbType.String, ParameterDirection.Input);

            using (var conn = _userContext.CreateConnection())
            {
                await conn.ExecuteAsync(procedure, parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<Register> GetUserByEmailAsync(string email)
        {
            var query = "SELECT * FROM [dbo].[AspNetUsers] WHERE Email = @Email";
            var param = new DynamicParameters();
            param.Add("Email", email);
            using (var conn = _userContext.CreateConnection())
            {
                var user = await conn.QueryFirstOrDefaultAsync<Register>(query, param);
                return user;
            }
        }

        public async Task UpdateAsync(string id, AppUser user)
        {
            var query = "";
            if (user.ImageData.Length > 0)
            {
                query = "UPDATE [dbo].[AspNetUsers] SET [UserName] = @UserName, [Email] = @Email, [TwoFactorEnabled] = @TwoFactorEnabled, [ProfilePicture] = @ProfilePicture, [ImageData] = @ImageData WHERE [Id] = @Id ";
            }
            else
            {
                query = "UPDATE [dbo].[AspNetUsers] SET [UserName] = @UserName, [Email] = @Email, [TwoFactorEnabled] = @TwoFactorEnabled, [ProfilePicture] = @ProfilePicture WHERE [Id] = @Id ";
            }
            var parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.String);
            parameters.Add("UserName", user.Email, DbType.String);
            parameters.Add("Email", user.Email, DbType.String);
            parameters.Add("TwoFactorEnabled", user.TwoFactorEnabled, DbType.Boolean);
            parameters.Add("ProfilePicture", user.ProfilePicture, DbType.String);
            parameters.Add("ImageData", user.ImageData);

            using (var conn = _userContext.CreateConnection())
            {
                await conn.ExecuteAsync(query, parameters);
            }
        }

        public async Task<IEnumerable<Register>> GetAllUsersProfileAsync()
        {
            var query = "SELECT * FROM [dbo].[AspNetUsers]";
            using (var conn = _userContext.CreateConnection())
            {
                var users = await conn.QueryAsync<Register>(query);
                return users.ToList();
            }
        }
    }
}
