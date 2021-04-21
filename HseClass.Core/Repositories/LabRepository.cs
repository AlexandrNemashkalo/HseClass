using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class LabRepository : ILabRepository
    {
        private readonly HseClassContext _context;

        public LabRepository(HseClassContext context)
        {
            _context = context;
        }
        
        public async Task<Lab> GetById(int labId)
        {
            return await _context.Labs.FirstOrDefaultAsync(l => l.Id == labId);
        }
        
        public async Task<Lab> Create(Lab lab)
        {
            var result = await _context.Labs.AddAsync(lab);
            await _context.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task Delete(int labId)
        {
            var company = await _context.Labs.FirstOrDefaultAsync(c => c.Id == labId);
            _context.Labs.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Lab>> GetByClassId(int classId)
        {
            return await _context.Labs.Where(l => l.TeamId == classId).ToListAsync();
        }

        public async Task<Lab> Update(Lab lab)
        {
            var result = _context.Labs.Update(lab);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}