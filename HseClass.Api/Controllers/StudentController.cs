using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
using HseClass.Api.ViewModels.StudentViewModels;
using HseClass.Core.EF;
using HseClass.Core.Guard;
using HseClass.Data.Entities;
using HseClass.Data.Enums;
using HseClass.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "student")]
    public class StudentController : ControllerBase
    {
        private readonly IClassRoomRepository _classRoomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserClassRepository _userClass;
        private readonly ILabRepository _labRepository;
        private readonly ISolutionLabRepository _solutionLab;
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ITaskLabRepository _taskLab;
        
        public StudentController( 
            IClassRoomRepository classRoomRepository,
            IUserRepository userRepository,
            ILabRepository labRepository,
            ISolutionLabRepository solutionLab,
            UserManager<User> userManager,
            IUserClassRepository userClass,
            IWebHostEnvironment environment,
            ITaskLabRepository taskLab)
        {
            _classRoomRepository = classRoomRepository;
            _userRepository = userRepository;
            _labRepository = labRepository;
            _solutionLab = solutionLab;
            _userManager = userManager;
            _userClass = userClass;
            _environment = environment;
            _taskLab = taskLab;
        }
        
        /// <summary>
        /// Получение списка классов,доступных студенту
        /// </summary>
        /// <returns></returns>
        [HttpGet("class")]
        public async Task<ActionResult<List<StudentClassViewModel>>> Get()
        {
            var classes= await _classRoomRepository.GetByUserId(this.GetUserIdFromToken());

            var result = new List<StudentClassViewModel>();
            foreach (var cl in classes)
            {

                string teacherName= "";
                string teacherEmail ="";
                foreach (var uc in cl.UserClasses)
                {
                    var user = await _userRepository.GetById(uc.UserId);
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Any(r => r == "teacher"))
                    {
                        teacherEmail = user.Email;
                        teacherName = user.Name;
                        break;
                    }
                }
                
                result.Add(new StudentClassViewModel()
                {
                    Code = cl.Code,
                    Id = cl.Id,
                    Labs = cl.Labs,
                    UserClasses = cl.UserClasses,
                    Title = cl.Title,
                    TeacherEmail = teacherEmail,
                    TeacherName =  teacherName
                });
            }

            return result;
        }
        
        /// <summary>
        /// Получение детальной информации класса
        /// </summary>
        /// <returns></returns>
        [HttpGet("class/{classId}")]
        public async Task<ActionResult<StudentClassInfoViewModel>> Get(int classId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, classId);
                
            var cl = await _classRoomRepository.GetById(classId);
            
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

            
            return new StudentClassInfoViewModel()
            {
                Id = cl.Id,
                Title = cl.Title,
                Code = cl.Code,
                Users = users,
                Labs =  cl.Labs.Select(  (x) =>
                {
                    var a =  _solutionLab.GetById(user.Id, x.Id);
                    a.Wait();
               
                    return new StudentLabInfoViewModel()
                    {
                        ClassRoomId = x.ClassRoomId,
                        Deadline = x.Deadline,
                        Id = x.Id,
                        MaxGrade = x.MaxGrade,
                        SolutionLabs = null,
                        Title = x.Title,
                        TaskLabId = x.TaskLabId,
                        MySolution = a.Result
                    };
                }).ToList()
            };
        }

        /// <summary>
        /// Получение детальной информации о задании лабораторной и его решении
        /// </summary>
        /// <returns></returns>
        [HttpGet("lab/{labId}")]
        public async Task<ActionResult<LabInfoViewModel>> GetLabInfo(int labId)
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            var lab = await _labRepository.GetById(labId);
            var task = await _taskLab.GetById(lab.TaskLabId);
            var solution = user.SolutionLabs.FirstOrDefault(s => s.LabId == labId);

            return new LabInfoViewModel()
            {
                Id = lab.Id,
                Title = lab.Title,
                Task = new TaskLabViewModel()
                {
                    Id = task.Id,
                    Description = task.Description,
                    LinkToManual = task.LinkToManual,
                    Equipment = task.Equipment,
                    Name = task.Name,
                    Theme = task.Theme,
                    RecommendedClass = task.RecommendedClass
                },
                ClassRoomId = lab.ClassRoomId,
                MaxGrade = lab.MaxGrade,
                Deadline = lab.Deadline,
                Solution = solution
            };
        }

        /// <summary>
        /// Получение списка активных и завершенных решений
        /// </summary>
        /// <returns></returns>
        [HttpGet("solution")]
        public async Task<ActionResult<SolutionsViewModel>> GetSolutions()
        {
            var user = await _userRepository.GetById(this.GetUserIdFromToken());

            var activeSolutions = new List<SolutionViewModel>();
            var finishedSolutions = new List<SolutionViewModel>();
            
            foreach (var s in user.SolutionLabs)
            {
                var lab = await _labRepository.GetById(s.LabId);
                var task = await _taskLab.GetById(lab.TaskLabId);
                
                var sol = new SolutionViewModel()
                {
                    DateOfDownload = s.DateOfDownload,
                    Grade = s.Grade,
                    Lab = new LabInfo()
                    {
                        ClassRoomId = lab.ClassRoomId,
                        Title = lab.Title,
                        Deadline = lab.Deadline,
                        Id =lab.Id,
                        MaxGrade = lab.MaxGrade,
                        Task = new TaskLabViewModel()
                        {
                            Description = task.Description,
                            Equipment = task.Equipment,
                            LinkToManual = task.LinkToManual,
                            Id =task.Id,
                            Name = task.Name,
                            Theme = task.Theme,
                            RecommendedClass = task.RecommendedClass
                        }
                    },
                    Solution = s.Solution,
                    Status = s.Status,
                    TimeSpan = s.TimeSpan,
                    UserId = s.UserId,
                    VideoPath = s.VideoPath
                };
                
                if (s.Status == LabStatusEnums.Assigned)
                {
                    activeSolutions.Add(sol);
                }
                else
                {
                    finishedSolutions.Add(sol);
                }
            }

            return new SolutionsViewModel()
            {
                ActiveSolutions = activeSolutions,
                FinishedSolutions = finishedSolutions
            };
        }
        
        /// <summary>
        /// Изменение выполненой лаб. работы студента
        /// </summary>
        /// <returns></returns>
        [HttpPut("solution/{labId}")]
        public async Task<ActionResult<SolutionLab>> UpdateUserLab(int labId, [FromBody] UserLabSolutionForm form)
        {
            var lab = await _labRepository.GetById(labId);
            var user = await _userRepository.GetById(this.GetUserIdFromToken());
            await this.CheckUserInClass(user, lab.ClassRoomId);

            var userLab = await _solutionLab.GetById(user.Id, labId);
            Ensure.IsNotNull(userLab, nameof(_solutionLab.GetById));

            userLab.Solution = form.Solution;
            userLab.DateOfDownload = DateTime.Now;
            userLab.TimeSpan = form.TimeSpan;
            userLab.VideoPath = form.Video;

            if (lab.Deadline < DateTime.Now)
            {
                userLab.Status = LabStatusEnums.Overdue;
            }

            return await _solutionLab.Update(userLab);
        }
        
        /// <summary>
        /// Присоединение к классу по коду
        /// </summary>
        /// <returns></returns>
        [HttpPut("class/{code}")]
        public async Task<ActionResult<ClassRoom>> AddToClass(Guid code)
        {
            var cl = await _classRoomRepository.GetByCode(code);
            Ensure.IsNotNull(cl, nameof(_classRoomRepository.GetByCode));
            
            var addedUser = await _userManager.FindByIdAsync(this.GetUserIdFromToken().ToString());
            
            await _userClass.Create(cl.Id, addedUser.Id);

            foreach (var lab in cl.Labs)
            {
                var compareValue = lab.Deadline.CompareTo(DateTime.Now);
                var status = compareValue < 0 ? LabStatusEnums.Overdue : LabStatusEnums.Assigned;
                
                await _solutionLab.Create(new SolutionLab()
                {
                    DateOfDownload = null,
                    Grade = null,
                    LabId = lab.Id,
                    Solution = null,
                    Status = status,
                    UserId = addedUser.Id
                });
            }

            return cl;
        }
        
        
    }
}