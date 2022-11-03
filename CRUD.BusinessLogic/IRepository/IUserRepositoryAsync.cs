using CRUD.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.BusinessLogic.IRepository
{
    public interface IUserRepositoryAsync
    {
        Task<IEnumerable<User1>> GetAllUsersAsync();
        Task<User1> GetUserByIdAsync(int id);
        Task<User1> CreateUserAsync(User1 user);
        Task UpdateUserAsync(int id, User1 user);
        Task DeleteUserByIdAsync(int id);
        Task CreateUserLoginAsync(UserLogin user);
        Task<Register> GetUserByEmailAsync(string email);
        Task UpdateAsync(string id, AppUser user);
    }
}
