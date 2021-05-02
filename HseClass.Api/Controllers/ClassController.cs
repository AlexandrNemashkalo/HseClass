using System;
using System.Collections.Generic;
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
    [Route("api/[controller]-Teacher")]
    [Authorize(Roles = "teacher")]
    public class ClassController : ControllerBase
    {
        private readonly IClassRoomRepository _classRoomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserClassRepository _userClass;
        private readonly UserManager<User> _userManager;
        
        public ClassController( 
            IClassRoomRepository classRoomRepository,
            IUserRepository userRepository,
            IUserClassRepository userClass,
            UserManager<User> userManager)
        {
            _classRoomRepository = classRoomRepository;
            _userRepository = userRepository;
            _userClass = userClass;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Получение списка классов,доступных учителю
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<ClassRoom>>> Get()
        {
            return await _classRoomRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Получение детальной информации о классе
        /// </summary>
        /// <returns></returns>
        [HttpGet("{classId}")]
        public async Task<ActionResult<List<ClassRoom>>> Get(int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);
                
            return await _classRoomRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Создание класса 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ClassRoom>> Post([FromBody] ClassForm form)
        {
            var cl = await _classRoomRepository.Create(new ClassRoom()
            {
                Title = form.Title
            });

            await _userClass.Create(cl.Id, this.GetUserIdFromToken());
            
            return await _classRoomRepository.GetById(cl.Id);
        }
        
        /// <summary>
        /// Изменение метаданных класса 
        /// </summary>
        /// <returns></returns>
        [HttpPut("{classId}")]
        public async Task<ActionResult<ClassRoom>> Put([FromBody] ClassForm form, [FromRoute] int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);
            
            var cl = await _classRoomRepository.GetById(classId);

            cl.Title = form.Title;
            
            return  await _classRoomRepository.Update(cl);
        }

        /// <summary>
        /// Удаление класса 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{classId}")]
        public async Task<ActionResult<bool>> Delete(int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);

            try
            {
                await _classRoomRepository.Delete(classId);
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
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);

            var addedUser = await _userManager.FindByEmailAsync(addedUserEmail);
            Ensure.IsNotNull(addedUser, nameof(_userManager.FindByEmailAsync));

            try
            {
                await _userClass.Create(classId, addedUser.Id);
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
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            var deletedUser = await _userRepository.GetById(deletedUserId);
            await this.CheckUserInClass(user, classId);
            await this.CheckUserInClass(deletedUser, classId);

            try
            {
                await _userClass.Delete(classId, deletedUserId);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}