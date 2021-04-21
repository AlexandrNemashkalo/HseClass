using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class ClassRepository: IClassRepository
    {
        private readonly HseClassContext _context;

        public ClassRepository(HseClassContext context)
        {
            _context = context;
        }
        
        public async Task<Team> Create(Team cl)
        {
            var result = await _context.Classes.AddAsync(cl);
            await _context.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task Delete(int classId)
        {
            var cl = await _context.Classes.FirstOrDefaultAsync(c => c.Id == classId);
            _context.Classes.Remove(cl);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Team>> GetByUserId(int userId)
        {
            var query =
                from c in _context.Classes 
                join p in _context.UserTeams
                    on c.Id equals p.TeamId
                join u in _context.Users
                    on p.UserId equals u.Id
                where p.UserId == userId
                select c;

            return await query.ToListAsync();
        }

        public async Task<Team> GetById(int classId)
        {
            return await _context.Classes
                .Include(c => c.Labs)
                .Include(c => c.UserClasses)
                .FirstOrDefaultAsync(c => c.Id == classId);
        }

        public async Task<Team> Update(Team cl)
        {
            var result = _context.Classes.Update(cl);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}