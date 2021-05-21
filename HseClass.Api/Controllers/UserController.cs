using System.Linq;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
using HseClass.Core.Guard;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private IUserRepository _userRepository;
        private UserManager<User> _userManager;

        public UserController(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }


        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<UserViewModel>> Get()
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            Ensure.IsNotNull(user, nameof(_userRepository.GetById));
            
            var roles = await _userManager.GetRolesAsync(user);

            return new UserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                IsTeacher = roles.Any(r => r == "teacher")
            };
        }
    }
    
    
}