using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Core.Infrastructure;
using HseClass.Data.EF;
using HseClass.Data.Models;
using HseClass.Core.IRepositories;
using HseClass.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HseClassContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<UserEntity> _signInManager;

        public UserRepository(
            HseClassContext context,
            UserManager<UserEntity> userManager,
            IMapper mapper,
            SignInManager<UserEntity> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        public async Task<User> GetById(int userId)
        {
            var userEntity = await _context.Users
                .Include(u => u.UserClasses)
                .Include(u => u.UserLabs)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return _mapper.Map<UserEntity, User>(userEntity);
        }

        public async  Task<Teacher> GetTeacherById(int userId)
        {
            var userEntity = await _context.Users
                .Include(u => u.UserClasses)
                .Include(u => u.UserLabs)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return _mapper.Map<UserEntity, Teacher>(userEntity);
        }

        public async Task<List<User>> GetByClassId(int classId)
        {
            var query =
                from c in _context.Users 
                join p in _context.UserClasses
                    on c.Id equals p.ClassRoomId
                join u in _context.ClassRooms
                    on p.UserId equals u.Id
                where p.ClassRoomId == classId
                select c;
            
            return query.Select(u => _mapper.Map<UserEntity, User>(u)).ToList();
        }

        public async Task<bool> CheckUserInClass(int userId, int classId)
        {
            var userEntity = await _context.Users
                .Include(u => u.UserClasses)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return userEntity != null && userEntity.UserClasses.Any(uc => uc.ClassRoomId == classId);
        }

        public async Task DeleteFromClass(int userId, int classId)
        {
            var userClass = await _context.UserClasses
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClassRoomId == classId);
            _context.UserClasses.Remove(userClass);
            await _context.SaveChangesAsync();
        }

        public async Task AddToClass(int userId, int classId)
        {
            var result = await _context.UserClasses
                .AddAsync(new UserClassEntity()
                {
                    ClassRoomId = classId,
                    UserId = userId
                });
            
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> GetRoles(User user)
        {
            var userEntity = _mapper.Map<User, UserEntity>(user);
            var result = await _userManager.GetRolesAsync(userEntity);
            return result.ToList();
        }

        public async Task<User> FindByEmail(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return _mapper.Map<UserEntity, User>(result);
        }

        public async Task<bool> CheckPasswordSignIn(User user, string password)
        {
            var userEntity = await _userManager.FindByEmailAsync(user.Email);
            var result = await _signInManager.CheckPasswordSignInAsync(userEntity, password, false);
            return result.Succeeded;
        }

        public async Task SignIn(User user, bool isPersistent)
        {
            var userEntity = await _userManager.FindByEmailAsync(user.Email);
            await _signInManager.SignInAsync(userEntity, false);
        }

        public async Task<bool> AddToRole(User user, string role)
        {
            var userEntity = await _userManager.FindByEmailAsync(user.Email);
            var result = await _userManager.AddToRoleAsync(userEntity, role);
            return result.Succeeded;
        }

        public async Task<bool> Create(User user, string password)
        {
            var userEntity = _mapper.Map<User, UserEntity>(user);
            var result = await _userManager.CreateAsync(userEntity, password);
            return result.Succeeded;
        }

    }
}