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
    [Route("api/[controller]")]
    [Authorize(Roles = "teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly IClassRoomRepository _classRoomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserClassRepository _userClass;
        private readonly ILabRepository _labRepository;
        private readonly ISolutionLabRepository _solutionLab;
        private readonly UserManager<User> _userManager;
        
        public TeacherController( 
            IClassRoomRepository classRoomRepository,
            IUserRepository userRepository,
            IUserClassRepository userClass,
            ILabRepository labRepository,
            ISolutionLabRepository solutionLab,
            UserManager<User> userManager)
        {
            _labRepository = labRepository;
            _solutionLab = solutionLab;
            _classRoomRepository = classRoomRepository;
            _userRepository = userRepository;
            _userClass = userClass;
            _userManager = userManager;
        }
        
        /// <summary>
        /// Получение списка классов,доступных учителю
        /// </summary>
        /// <returns></returns>
        [HttpGet("class")]
        public async Task<ActionResult<List<ClassRoom>>> GetClasses()
        {
            return await _classRoomRepository.GetByUserId(this.GetUserIdFromToken());
        }
        
        /// <summary>
        /// Получение детальной информации о классе
        /// </summary>
        /// <returns></returns>
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<ClassInfoViewModel>> GetClass(int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);
                
            var cl =  await _classRoomRepository.GetById(classId);
            var users = new List<UserViewModel>();
            foreach (var uc in cl.UserClasses)
            {
                var u = await _userRepository.GetById(uc.UserId);
                var roles = await _userManager.GetRolesAsync(u);
                users.Add(new UserViewModel()
                {
                    Name = u.Name,
                    Id = u.Id,
                    Email = u.Email,
                    IsTeacher = roles.Any(r => r == "teacher")
                });
            }

            return new ClassInfoViewModel()
            {
                Id = cl.Id,
                Title = cl.Title,
                Code = cl.Code,
                Users = users,
                Labs = cl.Labs
            };
        }
        
        /// <summary>
        /// Создание класса 
        /// </summary>
        /// <returns></returns>
        [HttpPost("class")]
        public async Task<ActionResult<ClassRoom>> PostClass([FromBody] ClassForm form)
        {
            var cl = await _classRoomRepository.Create(new ClassRoom()
            {
                Code = Guid.NewGuid(),
                Title = form.Title
            });

            await _userClass.Create(cl.Id, this.GetUserIdFromToken());
            
            return await _classRoomRepository.GetById(cl.Id);
        }
        
        /// <summary>
        /// Изменение метаданных класса 
        /// </summary>
        /// <returns></returns>
        [HttpPut("class/{classId}")]
        public async Task<ActionResult<ClassRoom>> PutClass([FromBody] ClassForm form, [FromRoute] int classId)
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
        [HttpDelete("class/{classId}")]
        public async Task<ActionResult<bool>> DeleteClass(int classId)
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
        [HttpPost("class/{classId}/user/{addedUserEmail}")]
        public async Task<ActionResult<bool>> AddStudent(int classId, string addedUserEmail)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);

            var cl = await  _classRoomRepository.GetById(classId);
            
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
            
            foreach (var lab in cl.Labs)
            {
                await _solutionLab.Create(new SolutionLab()
                {
                    DateOfDownload = null,
                    Grade = null,
                    LabId = lab.Id,
                    Solution = null,
                    Status = LabStatusEnums.Assigned,
                    UserId = addedUser.Id
                });
            }

            return true;
        }
        
        /// <summary>
        /// Удаление пользователя из класса 
        /// </summary>
        /// <returns></returns>
        [HttpDelete("class/{classId}/user/{deletedUserId}")]
        public async Task<ActionResult<bool>> DeleteStudentFromClass(int classId, int deletedUserId)
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
        
        
        /// <summary>
        /// Назначение задания (лабораторной)
        /// </summary>
        /// <returns></returns>
        [HttpPost("lab")]
        public async Task<ActionResult<Lab>> PostLab([FromBody] LabForm form)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, form.ClassRoomId);
            
            var lab = await _labRepository.Create(new Lab()
            {
                TaskLabId = form.TaskLabId,
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
                
                await _solutionLab.Create(new SolutionLab()
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
        [HttpPut("lab/{labId}")]
        public async Task<ActionResult<Lab>> PutLab([FromBody] LabForm form, [FromRoute] int labId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, form.ClassRoomId);
            
            var lab = await _labRepository.GetById(labId);
            Ensure.IsNotNull(lab, nameof(_labRepository.GetById));

            lab.TaskLabId = form.TaskLabId;
            lab.Deadline = form.Deadline;
            lab.MaxGrade = form.MaxGrade;
            lab.Title = form.Title;

            return  await _labRepository.Update(lab);
        }

        /// <summary>
        /// Удаление лабораторной
        /// </summary>
        /// <returns></returns>
        [HttpDelete("lab/{labId}")]
        public async Task<ActionResult<bool>> DeleteLab(int labId)
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
        [HttpGet("lab/{labId}/user")]
        public async Task<ActionResult<List<SolutionLab>>> GetUserLabs(int labId)
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
        [HttpPut("lab/{labId}/user/{checkedUserId}")]
        public async Task<ActionResult<SolutionLab>> UpdateUserLab(int labId, int checkedUserId, [FromBody] UserLabForm form )
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
    }
}