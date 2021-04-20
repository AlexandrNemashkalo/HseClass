using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class UserClassRepository : IUserClassRepository
    {
        private readonly HseClassContext _context;

        public UserClassRepository(HseClassContext context)
        {
            _context = context;
            
        }
        public async Task<UserClass> Create(int classId, int userId)
        {
            var result =await _context.UserClasses
                .AddAsync(new UserClass()
                {
                    ClassId = classId,
                    UserId = userId
                });
            
            await _context.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task Delete(int classId, int userId)
        {
            var userClass = await _context.UserClasses
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClassId == classId);
            _context.UserClasses.Remove(userClass);
            await _context.SaveChangesAsync();
        }
    }
}