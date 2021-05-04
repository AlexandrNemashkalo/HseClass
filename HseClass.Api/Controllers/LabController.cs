using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
using HseClass.Core.Guard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{/*
    [Route("api/[controller]-Teacher")]
    [Authorize(Roles = "teacher")]
    public class LabController : ControllerBase
    {
        private readonly ILabRepository _labRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISolutionLabRepository _solutionLab;
        private readonly UserManager<UserEntity> _userManager;
        
        public LabController( 
            ILabRepository labRepository,
            IUserRepository userRepository,
            ISolutionLabRepository solutionLab,
            UserManager<UserEntity> userManager)
        {
            _labRepository = labRepository;
            _userRepository = userRepository;
            _solutionLab = solutionLab;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Создание задания (лабораторной)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<LabEntity>> PostLab([FromBody] LabForm form)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, form.ClassRoomId);
            
            var lab = await _labRepository.Create(new LabEntity()
            {
                Task = form.Task,
                Deadline = form.Deadline,
                ClassRoomId = form.ClassRoomId
            });

            var usersInClass = await _userRepository.GetByClassId(form.ClassRoomId);
            foreach (var us in usersInClass)
            {
                if (await _userManager.IsInRoleAsync(user, "teacher"))
                {
                    continue;
                }
                
                await _solutionLab.Create(new SolutionLabEntity()
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
        public async Task<ActionResult<LabEntity>> PutLab([FromBody] LabForm form, [FromRoute] int labId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, form.ClassRoomId);
            
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
            await this.CheckUserInClass(user, lab.ClassRoomId);

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
        public async Task<ActionResult<List<SolutionLabEntity>>> GetUserLabs(int labId)
        {
            var lab = await _labRepository.GetById(labId);
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, lab.ClassRoomId);
            
            return await _solutionLab.GetByLabId(labId);
        }
        
        /// <summary>
        /// Изменение выполненой лаб. работы студента
        /// </summary>
        /// <returns></returns>
        [HttpPut("{labId}/user/{checkedUserId}")]
        public async Task<ActionResult<SolutionLabEntity>> UpdateUserLab(int labId, int checkedUserId, [FromBody] UserLabForm form )
        {
            var lab = await _labRepository.GetById(labId);
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            var checkedUser = await _userRepository.GetById(checkedUserId);
            await this.CheckUserInClass(user, lab.ClassRoomId);
            await this.CheckUserInClass(checkedUser ,lab.ClassRoomId);

            var userLab = await _solutionLab.GetById(checkedUserId, labId);
            Ensure.IsNotNull(userLab, nameof(_solutionLab.GetById));

            userLab.Grade = form.Grade;
            userLab.Status = form.Status;

            return await _solutionLab.Update(userLab);
        }
    }*/
}