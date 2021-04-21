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
        
        public async Task<User> GetById(int userId)
        {
            return await _context.Users
                .Include(u => u.UserClasses)
                .Include(u => u.UserLabs)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<User>> GetByClassId(int classId)
        {
            var query =
                from c in _context.Users 
                join p in _context.UserTeams
                    on c.Id equals p.TeamId
                join u in _context.Classes
                    on p.UserId equals u.Id
                where p.TeamId == classId
                select c;

            return await query.ToListAsync();
        }
    }
}