﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
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
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserClassRepository _userClass;
        
        public ClassController( 
            IClassRepository classRepository,
            IUserRepository userRepository,
            IUserClassRepository userClass)
        {
            _classRepository = classRepository;
            _userRepository = userRepository;
            _userClass = userClass;
        }
        
        /// <summary>
        /// Получение списка классов,доступных учителю
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Class>>> Get()
        {
            return await _classRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Получение детальной информации о классе
        /// </summary>
        /// <returns></returns>
        [HttpGet("{classId}")]
        public async Task<ActionResult<List<Class>>> Get(int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);
                
            return await _classRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Создание класса 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Class>> Post([FromBody] ClassForm form)
        {
            var cl = await _classRepository.Create(new Class()
            {
                Title = form.Title
            });

            await _userClass.Create(cl.Id, this.GetUserIdFromToken());
            
            return await _classRepository.GetById(cl.Id);
        }
        
        /// <summary>
        /// Изменение метаданных класса 
        /// </summary>
        /// <returns></returns>
        [HttpPut("{classId}")]
        public async Task<ActionResult<Class>> Put([FromBody] ClassForm form, [FromRoute] int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);
            
            var cl = await _classRepository.GetById(classId);

            cl.Title = form.Title;
            
            return  await _classRepository.Update(cl);
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
                await _classRepository.Delete(classId);
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