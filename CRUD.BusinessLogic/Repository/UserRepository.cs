using CRUD.BusinessLogic.IRepository;
using CRUD.DataAccess;
using CRUD.Model.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Security.Claims;

namespace CRUD.BusinessLogic.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDBContext _userDBContext;

        private string SortField { get; set; } = string.Empty;
        private string OrderBy { get; set; } = string.Empty;

        public UserRepository(UserDBContext userDBContext) => _userDBContext = userDBContext;
        public IEnumerable<User1>? GetUsers()
        {
            try
            {
                var obj = _userDBContext.Users1.ToList();
                if (obj != null) return obj;
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<User1> SortUserData(string sortField, string? currentSortField, string currentSortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                SortField = "ID";
                OrderBy = "Asc";
            }
            else
            {
                if (currentSortField == sortField)
                {
                    OrderBy = currentSortOrder == "Asc" ? "Desc" : "Asc";
                }
                else
                {
                    OrderBy = "Asc";
                }
                SortField = sortField;
            }

            Expression<Func<User1, object>> sortExpression;
            switch (SortField)
            {
                case "Id":
                    sortExpression = (x => x.Id);
                    break;
                case "firstName":
                    sortExpression = (x => x.FirstName);
                    break;
                case "lastName":
                    sortExpression = (x => x.LastName);
                    break;
                case "phoneNumber":
                    sortExpression = (x => x.PhoneNumber);
                    break;
                case "email":
                    sortExpression = (x => x.Email);
                    break;
                case "gender":
                    sortExpression = (x => x.Gender);
                    break;
                default:
                    sortExpression = (x => x.Id);
                    break;
            }

            if (OrderBy == "Asc")
            {
                return _userDBContext.Users1.OrderBy(sortExpression).ToList();
            }
            else
            {
                return _userDBContext.Users1.OrderByDescending(sortExpression).ToList();
            }
        }

        public User1 GetUserByID(int? id)
        {
            try
            {
                var obj = _userDBContext.Users1.Where(u => u.Id == id).AsNoTracking().FirstOrDefault();
                if (obj != null) return obj;
                else return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddOrUpdateUser(User1 user1)
        {
            if (user1.Id > 0)
            {
                _userDBContext.Users1.Update(user1);
            }
            else
            {
                _userDBContext.Users1.Add(user1);
            }
            _userDBContext.SaveChanges();
        }

        public Task DeleteUser(int id)
        {
            var user = GetUserByID(id);
            _userDBContext.Users1.Remove(user);
            _userDBContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
