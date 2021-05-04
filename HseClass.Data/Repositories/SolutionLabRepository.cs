using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Data.EF;
using HseClass.Data.Models;
using HseClass.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Data.Repositories
{
    public class SolutionLabRepository
    {
        private readonly HseClassContext _context;

        public SolutionLabRepository(HseClassContext context)
        {
            _context = context;
            
        }
        public async Task<SolutionLabEntity> Create(SolutionLabEntity solutionLabEntity)
        {
            var result =await _context.UserLabs.AddAsync(solutionLabEntity);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<SolutionLabEntity> Update(SolutionLabEntity solutionLabEntity)
        {
            var result = _context.UserLabs.Update(solutionLabEntity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task Delete(int userId, int labId)
        {
            var userLab = await _context.UserLabs
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LabId ==labId);
            _context.UserLabs.Remove(userLab);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SolutionLabEntity>> GetByUserId(int userId)
        {
            return await _context.UserLabs.Where(ul => ul.UserId == userId).ToListAsync();
        }

        public async Task<List<SolutionLabEntity>> GetByLabId(int labId)
        {
            return await _context.UserLabs.Where(ul => ul.LabId == labId).ToListAsync();
        }

        public async Task<SolutionLabEntity> GetById(int userId, int labId)
        {
            return  await _context.UserLabs
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LabId ==labId);
        }
    }
}