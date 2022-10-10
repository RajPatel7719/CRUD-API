using CRUD.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.BusinessLogic.IRepository
{
    public interface IUserRepository
    {
        public IEnumerable<User1>? GetUsers();
        public User1 GetUserByID(int? id);
        public void AddOrUpdateUser(User1 user1);
        public Task DeleteUser(int id);

        public IEnumerable<User1> SortUserData(string sortField, string? currentSortField, string currentSortOrder);
    }
}
