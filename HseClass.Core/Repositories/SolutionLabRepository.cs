using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class SolutionLabRepository : ISolutionLabRepository
    {
        private readonly HseClassContext _context;

        public SolutionLabRepository(HseClassContext context)
        {
            _context = context;
            
        }
        public async Task<SolutionLab> Create(SolutionLab solutionLab)
        {
            var result =await _context.SolutionLabs.AddAsync(solutionLab);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<SolutionLab> Update(SolutionLab solutionLab)
        {
            var result = _context.SolutionLabs.Update(solutionLab);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task Delete(int userId, int labId)
        {
            var userLab = await _context.SolutionLabs
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LabId ==labId);
            _context.SolutionLabs.Remove(userLab);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SolutionLab>> GetByUserId(int userId)
        {
            return await _context.SolutionLabs.Where(ul => ul.UserId == userId).ToListAsync();
        }

        public async Task<List<SolutionLab>> GetByLabId(int labId)
        {
            return await _context.SolutionLabs.Where(ul => ul.LabId == labId).ToListAsync();
        }

        public async Task<SolutionLab> GetById(int userId, int labId)
        {
            return  await _context.SolutionLabs
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LabId ==labId);
        }
    }
}