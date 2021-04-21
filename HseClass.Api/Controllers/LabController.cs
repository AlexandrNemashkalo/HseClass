using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
using HseClass.Core.Guard;
using HseClass.Data.Entities;
using HseClass.Data.Enums;
using HseClass.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{
    [Route("api/[controller]-Teacher")]
    [Authorize(Roles = "teacher")]
    public class LabController : ControllerBase
    {
        private readonly ILabRepository _labRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserLabRepository _userLab;
        private readonly UserManager<User> _userManager;
        
        public LabController( 
            ILabRepository labRepository,
            IUserRepository userRepository,
            IUserLabRepository userLab,
            UserManager<User> userManager)
        {
            _labRepository = labRepository;
            _userRepository = userRepository;
            _userLab = userLab;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Создание задания (лабораторной)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Lab>> PostLab([FromBody] LabForm form)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, form.ClassId);
            
            var lab = await _labRepository.Create(new Lab()
            {
                Task = form.Task,
                Deadline = form.Deadline,
                TeamId = form.ClassId
            });

            var usersInClass = await _userRepository.GetByClassId(form.ClassId);
            foreach (var us in usersInClass)
            {
                if (await _userManager.IsInRoleAsync(user, "teacher"))
                {
                    continue;
                }
                
                await _userLab.Create(new UserLab()
                {
                    UserId = us.Id,
                    LabId = lab.Id,
                    Status = LabStatusEnums.Assigned
                });
            }
            
            return lab;
        }
        
        /// <summary>
        /// Изменение метаданных задания (лабы) 
        /// </summary>
        /// <returns></returns>
        [HttpPut("{labId}")]
        public async Task<ActionResult<Lab>> PutLab([FromBody] LabForm form, [FromRoute] int labId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, form.ClassId);
            
            var lab = await _labRepository.GetById(labId);
            Ensure.IsNotNull(lab, nameof(_labRepository.GetById));

            lab.Task = form.Task;
            lab.Deadline = form.Deadline;

            return  await _labRepository.Update(lab);
        }

        /// <summary>
        /// Удаление лабораторной
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{labId}")]
        public async Task<ActionResult<bool>> Delete(int labId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            var lab = await _labRepository.GetById(labId);
            await this.CheckUserInClass(user, lab.TeamId);

            try
            {
                await _labRepository.Delete(labId);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Получить список выполненных работ студентами по конкретной лабе
        /// </summary>
        /// <returns></returns>
        [HttpGet("{labId}/user")]
        public async Task<ActionResult<List<UserLab>>> GetUserLabs(int labId)
        {
            var lab = await _labRepository.GetById(labId);
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, lab.TeamId);
            
            return await _userLab.GetByLabId(labId);
        }
        
        /// <summary>
        /// Изменение выполненой лаб. работы студента
        /// </summary>
        /// <returns></returns>
        [HttpPut("{labId}/user/{checkedUserId}")]
        public async Task<ActionResult<UserLab>> UpdateUserLab(int labId, int checkedUserId, [FromBody] UserLabForm form )
        {
            var lab = await _labRepository.GetById(labId);
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            var checkedUser = await _userRepository.GetById(checkedUserId);
            await this.CheckUserInClass(user, lab.TeamId);
            await this.CheckUserInClass(checkedUser ,lab.TeamId);

            var userLab = await _userLab.GetById(checkedUserId, labId);
            Ensure.IsNotNull(userLab, nameof(_userLab.GetById));

            userLab.Grade = form.Grade;
            userLab.Status = form.Status;

            return await _userLab.Update(userLab);
        }
    }
}