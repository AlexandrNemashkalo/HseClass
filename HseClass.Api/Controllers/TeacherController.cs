using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
using HseClass.Core;
using HseClass.Core.Entities;
using HseClass.Core.Guard;
using HseClass.Core.IRepositories;
using HseClass.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly IUnitOfWork _data;
        private readonly IClassRoomRepository _classRoomRepository;
        private readonly IUserRepository _userRepository;
        //private readonly IUserClassRepository _userClass;
        private readonly UserManager<UserEntity> _userManager;
        
        public TeacherController( 
            IUnitOfWork data,
            IClassRoomRepository classRoomRepository,
            IUserRepository userRepository,
            //IUserClassRepository userClass,
            UserManager<UserEntity> userManager)
        {
            _data = data;
            _classRoomRepository = classRoomRepository;
            _userRepository = userRepository;
            //_userClass = userClass;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Получение списка классов,доступных учителю
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ClassRoom>>> Get()
        {
            return await _data.ClassRoom.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Получение детальной информации о классе
        /// </summary>
        /// <returns></returns>
        [HttpGet("{classId}")]
        public async Task<ActionResult<List<ClassRoom>>> Get(int classId)
        {
            var result = await _data.User.CheckUserInClass(this.GetUserIdFromToken(), classId);
            Ensure.That(result, "ошибка доступа");
            return await _data.ClassRoom.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Создание класса 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ClassRoom>> Post([FromBody] ClassForm form)
        {
            var cl = await _data.ClassRoom.Create(new ClassRoom()
            {
                Title = form.Title
            });

            await _data.User.AddToClass(this.GetUserIdFromToken(),cl.Id);
            
            return await _classRoomRepository.GetById(cl.Id);
        }
        
        /// <summary>
        /// Изменение метаданных класса 
        /// </summary>
        /// <returns></returns>
        [HttpPut("{classId}")]
        public async Task<ActionResult<ClassRoom>> Put([FromBody] ClassForm form, [FromRoute] int classId)
        {
            var result = await _data.User.CheckUserInClass(this.GetUserIdFromToken(), classId);
            Ensure.That(result, "ошибка доступа");
            
            var cl = await _classRoomRepository.GetById(classId);

            cl.Title = form.Title;
            
            return  await _data.ClassRoom.Update(cl);
        }

        /// <summary>
        /// Удаление класса 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{classId}")]
        public async Task<ActionResult<bool>> Delete(int classId)
        {
            var result = await _data.User.CheckUserInClass(this.GetUserIdFromToken(), classId);
            Ensure.That(result, "ошибка доступа");
            try
            {
                await _data.ClassRoom.Delete(classId);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Добавление пользователя в класс
        /// </summary>
        /// <returns></returns>
        [HttpPost("{classId}/user/{addedUserEmail}")]
        public async Task<ActionResult<bool>> AddStudent(int classId, string addedUserEmail)
        {
            var result = await _data.User.CheckUserInClass(this.GetUserIdFromToken(), classId);
            Ensure.That(result, "ошибка доступа");

            var addedUser = await _data.User.FindByEmail(addedUserEmail);
            Ensure.IsNotNull(addedUser, nameof(_userManager.FindByEmailAsync));

            try
            {
                await _data.User.AddToClass(addedUser.Id, classId);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Удаление пользователя из класса 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{classId}/user/{deletedUserId}")]
        public async Task<ActionResult<bool>> Delete(int classId, int deletedUserId)
        {
            var result1 = await _data.User.CheckUserInClass(this.GetUserIdFromToken(), classId);
            Ensure.That(result1, "ошибка доступа");
            
            var result2 = await _data.User.CheckUserInClass(deletedUserId, classId);
            Ensure.That(result2, "ошибка доступа");
            
            try
            {
                await _data.User.DeleteFromClass(deletedUserId, classId);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
    }
}