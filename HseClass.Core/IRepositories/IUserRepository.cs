using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Core.Entities;

namespace HseClass.Core.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> CheckUserInClass(int userId, int classId);
        
        Task DeleteFromClass(int userId, int classId);
        
        Task AddToClass(int userId, int classId);
        
        Task<List<string>> GetRoles(User user);
        
        Task<User> FindByEmail(string email);

        Task<bool> CheckPasswordSignIn(User user, string password);
        
        Task SignIn(User user, bool isPersistent);
            
        Task<bool> AddToRole(User user, string role);
            
        Task<bool> Create(User user, string password);
        
        Task<User> GetById(int userId);
        
        Task<Teacher> GetTeacherById(int userId);
        
        Task<List<User>> GetByClassId(int classId);
    }
}