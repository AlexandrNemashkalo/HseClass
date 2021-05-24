using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly HseClassContext _context;

        public UserRepository(HseClassContext context)
        {
            _context = context;
        }
        
        public async Task<User> GetById(int userId)
        {
            return await _context.Users
                .Include(u => u.UserClasses)
                .Include(u => u.SolutionLabs)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetByClassId(int classId)
        {
            var query =
                from c in _context.Users 
                join p in _context.UserClasses
                    on c.Id equals p.UserId
                where p.ClassRoomId == classId
                select c;

            return await query.ToListAsync();
        }
    }
}