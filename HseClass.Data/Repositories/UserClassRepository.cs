using System.Threading.Tasks;
using HseClass.Data.EF;
using HseClass.Data.Models;
using HseClass.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Data.Repositories
{
    /*public class UserClassRepository : IUserClassRepository
    {
        private readonly HseClassContext _context;

        public UserClassRepository(HseClassContext context)
        {
            _context = context;
            
        }
        public async Task<UserClassEntity> Create(int classId, int userId)
        {
            var result =await _context.UserClasses
                .AddAsync(new UserClassEntity()
                {
                    ClassRoomId = classId,
                    UserId = userId
                });
            
            await _context.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task Delete(int classId, int userId)
        {
            var userClass = await _context.UserClasses
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClassRoomId == classId);
            _context.UserClasses.Remove(userClass);
            await _context.SaveChangesAsync();
        }
    }*/
}