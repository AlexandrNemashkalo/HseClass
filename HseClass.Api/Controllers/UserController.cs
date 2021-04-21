using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
using HseClass.Core.EF;
using HseClass.Core.Guard;
using HseClass.Data.Entities;
using HseClass.Data.Enums;
using HseClass.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "student")]
    public class UserController : ControllerBase
    {
        private readonly IClassRepository _classRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILabRepository _labRepository;
        private readonly IUserLabRepository _userLab;
        
        public UserController( 
            IClassRepository classRepository,
            IUserRepository userRepository,
            ILabRepository labRepository,
            IUserLabRepository userLab)
        {
            _classRepository = classRepository;
            _userRepository = userRepository;
            _labRepository = labRepository;
            _userLab = userLab;
        }
        
        /// <summary>
        /// Получение списка классов,доступных студенту
        /// </summary>
        /// <returns></returns>
        [HttpGet("class")]
        public async Task<ActionResult<List<Team>>> Get()
        {
            return await _classRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Получение детальной информации класса
        /// </summary>
        /// <returns></returns>
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<List<Team>>> Get(int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);
                
            return await _classRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Изменение выполненой лаб. работы студента
        /// </summary>
        /// <returns></returns>
        [HttpPut("lab/{labId}")]
        public async Task<ActionResult<UserLab>> UpdateUserLab(int labId, [FromBody] UserLabSolutionForm form)
        {
            var lab = await _labRepository.GetById(labId);
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, lab.TeamId);

            var userLab = await _userLab.GetById(user.Id, labId);
            Ensure.IsNotNull(userLab, nameof(_userLab.GetById));

            userLab.Solution = form.Solution;
            userLab.DateOfDownload = DateTime.Now;
            
            if (lab.Deadline > DateTime.Now)
            {
                userLab.Status = LabStatusEnums.Completed;
            }
            else
            {
                userLab.Status = LabStatusEnums.Overdue;
            }

            return await _userLab.Update(userLab);
        }
        
        
        
        
    }
}