using System;
using System.Threading.Tasks;
using HseClass.Core.Services;
using HseClass.Data.Entities;
using HseClass.Data.Enums;
using HseClass.Data.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace HseClass.Core.EF
{
    public static class DbInitializer
    {
        private const string TeacherName = "Teacher Test";
        private const string TeacherEmail = "teach@teach.teach";
        private const string TeacherPassword = "teach1234";
        
        private const string StudentName = "Student Test";
        private const string StudentEmail = "student@stu.dent";
        private const string StudentPassword = "student1234";
        
        private static readonly Guid ClassCode = Guid.Parse("2D28709A-1BAE-4FB3-9705-2E534A88DC6A");

        private static IUserRepository _userRepo;
        private static IClassRoomRepository _classRepo;
        private static ILabRepository _labRepo;
        private static UserManager<User> _userManager;
        private static ITaskLabRepository _taskLab;
        private static ISolutionLabRepository _solutionLab;
        private static IUserClassRepository _userClass;
        private static IAuthService _authService;
        private static IConfiguration _config;
        private static RoleManager<IdentityRole<int>> _roleManager;

        public static async Task Initialize(IServiceProvider services)
        {
            _userRepo = services.GetService<IUserRepository>();
            _classRepo = services.GetService<IClassRoomRepository>();
            _labRepo = services.GetService<ILabRepository>();
            _userManager = services.GetService<UserManager<User>>();
            _roleManager = services.GetService<RoleManager<IdentityRole<int>>>();
            _taskLab = services.GetService<ITaskLabRepository>();
            _solutionLab = services.GetService<ISolutionLabRepository>();
            _userClass = services.GetService<IUserClassRepository>();
            _authService = services.GetService<IAuthService>();
            _config = services.GetService<IConfiguration>();

            await InitializeRolesAndAdmin();
            await InitializeTasksLab();
            await InitializeTeacher();
            await InitializeStudent();
            await InitializeClass();
        }

        #region Initializers
        private static async Task InitializeRolesAndAdmin()
        {
            var adminEmail = _config["AdminEmail"];
            var password = _config["AdminPassword"];
            
            if (await _roleManager.FindByNameAsync("admin") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole<int>("admin"));
            }

            if (await _roleManager.FindByNameAsync("student") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole<int>("student"));
            }
            
            if (await _roleManager.FindByNameAsync("teacher") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole<int>("teacher"));
            }

            if (await _userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    Name = "SuperAdmin"
                };
                var result = await _userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }

        private static async Task InitializeTasksLab()
        {
            await _taskLab.Create(new TaskLab()
            {
                CorrectSolution = "9.88",
                Description = "Определение центростремительного ускорения шарика при его равномерном движении по окружности",
                Equipment = "Штатив с муфтой и лапкой, лента измерительная, циркуль, динамометр лабораторный, весы с разновесами, шарик на нити, кусочек пробки с отверстием, лист бумаги, линейка",
                Theme = "Механика",
                Name = "Изучение движения тел по окружности под действием силы упругости и тяжести"
            });
            
            await _taskLab.Create(new TaskLab()
            {
                CorrectSolution = "100",
                Description = "Проверить справедливость законов электрического тока для последовательного и параллельного соединения проводников",
                Equipment = "Источник тока, два проволочных резистора, амперметр, вольтметр, реостат",
                Theme = "Электричество",
                Name = "Изучение последовательного и параллельного соединения проводников"
            });
            
            await _taskLab.Create(new TaskLab()
            {
                CorrectSolution = "y=x^2",
                Description = "Освоить метод измерения ускорения свободного падения с помощью нитяного маятника",
                Equipment = "Штатив с муфтой и лапкой,груз, нить, секундомер",
                Theme = "Механика",
                Name = "Определение ускорения свободного падения при помощи нитяного маятника"
            });
        }

        private static async Task InitializeTeacher()
        {
            if (await _userManager.FindByNameAsync(TeacherEmail) == null)
            {
                var teacher = new User
                {
                    Email = TeacherEmail,
                    UserName = TeacherEmail,
                    Name = TeacherName
                };
                var result = await _userManager.CreateAsync(teacher, TeacherPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(teacher, "teacher");
                }
            }
        }

        private static async Task InitializeStudent()
        {
            if (await _userManager.FindByNameAsync(StudentEmail) == null)
            {
                var student = new User
                {
                    Email = StudentEmail,
                    UserName = StudentEmail,
                    Name = StudentName
                };
                var result = await _userManager.CreateAsync(student, StudentPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(student, "student");
                }
            }
        }

        private static async Task InitializeClass()
        {
            if (await _classRepo.GetByCode(ClassCode) == null)
            {
                var cl = await _classRepo.Create(new ClassRoom()
                {
                    Code = ClassCode,
                    Title = "Класс - Физика"
                });

                var user = await _userManager.FindByEmailAsync(TeacherEmail);
                await _userClass.Create(cl.Id, user.Id);

                var tasksLab = await _taskLab.GetAll();

                var lab1 = await _labRepo.Create(new Lab()
                {
                    TaskLabId = tasksLab[0].Id,
                    ClassRoomId = cl.Id,
                    MaxGrade = 10,
                    Title = "Laba 1",
                    Deadline = DateTime.Now.AddDays(30)
                });

                var lab2 = await _labRepo.Create(new Lab()
                {
                    TaskLabId = tasksLab[1].Id,
                    ClassRoomId = cl.Id,
                    MaxGrade = 100,
                    Title = "Laba 2",
                    Deadline = DateTime.Now.AddDays(20)
                });

                var student = await _userManager.FindByEmailAsync(StudentEmail);
                await _userClass.Create(cl.Id, student.Id);

                await _solutionLab.Create(new SolutionLab()
                {
                    DateOfDownload = null,
                    Grade = null,
                    LabId = lab1.Id,
                    Solution = null,
                    Status = LabStatusEnums.Assigned,
                    TimeSpan = null,
                    UserId = student.Id,
                    VideoPath = null
                });
                
                await _solutionLab.Create(new SolutionLab()
                {
                    DateOfDownload = null,
                    Grade = null,
                    LabId = lab2.Id,
                    Solution = null,
                    Status = LabStatusEnums.Assigned,
                    TimeSpan = null,
                    UserId = student.Id,
                    VideoPath = null
                });
            }
        }
        
        #endregion
    }
}