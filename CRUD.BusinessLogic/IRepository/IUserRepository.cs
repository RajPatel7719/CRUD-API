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
        public int AddOrUpdateUser(User1 user);
        public int DeleteUser(int id);

        public IEnumerable<User1> SortUserData(string sortField, string? currentSortField, string currentSortOrder);
    }
}
