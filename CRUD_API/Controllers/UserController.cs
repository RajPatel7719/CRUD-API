using AutoMapper;
using CRUD.BusinessLogic.IRepository;
using CRUD.DataAccess;
using CRUD.Model.Models;
using CRUD.Model.ModelsDTO;
using CRUD_API.Const;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRUD_API.Controllers
{
    [ApiController, Authorize]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IUserRepositoryAsync userRepositoryAsync, IMapper mapper)
        {
            _userRepository = userRepository;
            _userRepositoryAsync = userRepositoryAsync;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            ApiResponse<IEnumerable<User1>> response = new();
            try
            {
                response.Result = await _userRepositoryAsync.GetAllUsersAsync();
                response.StatusCode = StatusCodes.Status200OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.ErrorMessage = ex.Message;
                return Ok(response);
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
        public IActionResult GetUserByID(int id)
        {
            ApiResponse<User1> response = new();
            try
            {
                response.Result = _userRepository.GetUserByID(id);
                response.StatusCode = StatusCodes.Status200OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                return Ok(response);
            }
        }

        [HttpPost(Name = "AddOrUpdateUser")]
        public IActionResult AddOrUpdateUser([FromBody] User1 user1)
        {
            ApiResponse<object> response = new();
            if (ModelState.IsValid)
            {
                try
                {
                    int result = _userRepository.AddOrUpdateUser(user1);
                    if (result != 1)
                    {
                        response.ErrorMessage = Constants.StatusError;
                        response.StatusCode = StatusCodes.Status404NotFound;
                        response.Result = new();
                    }
                    else
                    {
                        response.ErrorMessage = Constants.StatusSuccess;
                        response.StatusCode = StatusCodes.Status200OK;
                        response.Result = new();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Ok(response);
        }

        [HttpGet(Name = "DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            ApiResponse<object> response = new();
            if (id.ToString() != null)
            {
                int result = _userRepository.DeleteUser(id);
                if (result != 1)
                {
                    response.ErrorMessage = Constants.StatusError;
                    response.StatusCode = StatusCodes.Status404NotFound;
                    response.Result = new();
                }
                else
                {
                    response.ErrorMessage = Constants.StatusSuccess;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Result = new();
                }
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserLogin(UserLogin userLogin)
        {
            await _userRepositoryAsync.CreateUserLoginAsync(userLogin);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersByMapper()
        {
            var users = await _userRepositoryAsync.GetAllUsersAsync();
            var mappedUser = _mapper.Map<IEnumerable<UserDTO>>(users);
            return Ok(mappedUser);
        }

        [HttpPost]
        public IActionResult AddOrUpdateUserByMapper([FromBody] UserDTO user)
        {
            ApiResponse<object> response = new();
            if (ModelState.IsValid)
            {
                try
                {
                    var mappedUser = _mapper.Map<User1>(user);
                    int result = _userRepository.AddOrUpdateUser(mappedUser);
                    if (result != 1)
                    {
                        response.ErrorMessage = Constants.StatusError;
                        response.StatusCode = StatusCodes.Status404NotFound;
                        response.Result = new();
                    }
                    else
                    {
                        response.ErrorMessage = Constants.StatusSuccess;
                        response.StatusCode = StatusCodes.Status200OK;
                        response.Result = new();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Ok(response);
        }
    }
}
