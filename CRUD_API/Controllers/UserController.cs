using CRUD.BusinessLogic.IRepository;
using CRUD.DataAccess;
using CRUD.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User1>? GetUsers()
        {
            try
            {
                return _userRepository.GetUsers();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet(Name = "SortUserData")]
        public IEnumerable<User1> SortUserData(string sortField, string? currentSortField, string currentSortOrder)
        {
            try
            {
                return _userRepository.SortUserData(sortField, currentSortField, currentSortOrder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet(Name = "GetUserByID")]
        public User1 GetUserByID(int? id)
        {
            try
            {
                return _userRepository.GetUserByID(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost(Name = "AddOrUpdateUser")]
        public void AddOrUpdateUser([FromBody] User1 user1)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _userRepository.AddOrUpdateUser(user1);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        [HttpGet(Name = "DeleteUser")] 
        public Task DeleteUser(int id)
        {
            if (id.ToString() != null)
            {
                _userRepository.DeleteUser(id);
            }
            return Task.CompletedTask;
        }
    }
}
