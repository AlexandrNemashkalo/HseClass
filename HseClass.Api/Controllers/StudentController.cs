using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
using HseClass.Core.Guard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{
    /*[Route("api/[controller]")]
    [Authorize(Roles = "student")]
    public class StudentController : ControllerBase
    {
        private readonly IClassRoomRepository _classRoomRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILabRepository _labRepository;
        private readonly ISolutionLabRepository _solutionLab;
        
        public StudentController( 
            IClassRoomRepository classRoomRepository,
            IUserRepository userRepository,
            ILabRepository labRepository,
            ISolutionLabRepository solutionLab)
        {
            _classRoomRepository = classRoomRepository;
            _userRepository = userRepository;
            _labRepository = labRepository;
            _solutionLab = solutionLab;
        }
        
        /// <summary>
        /// Получение списка классов,доступных студенту
        /// </summary>
        /// <returns></returns>
        [HttpGet("class")]
        public async Task<ActionResult<List<ClassRoomEntity>>> Get()
        {
            return await _classRoomRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Получение детальной информации класса
        /// </summary>
        /// <returns></returns>
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<List<ClassRoomEntity>>> Get(int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);
                
            return await _classRoomRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Изменение выполненой лаб. работы студента
        /// </summary>
        /// <returns></returns>
        [HttpPut("lab/{labId}")]
        public async Task<ActionResult<SolutionLabEntity>> UpdateUserLab(int labId, [FromBody] UserLabSolutionForm form)
        {
            var lab = await _labRepository.GetById(labId);
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, lab.ClassRoomId);

            var userLab = await _solutionLab.GetById(user.Id, labId);
            Ensure.IsNotNull(userLab, nameof(_solutionLab.GetById));

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

            return await _solutionLab.Update(userLab);
        }
        
        
        
        
    }*/
}