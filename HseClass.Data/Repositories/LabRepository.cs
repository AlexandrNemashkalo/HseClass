using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Data.EF;
using HseClass.Data.Models;
using HseClass.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Data.Repositories
{
    public class LabRepository
    {
        private readonly HseClassContext _context;

        public LabRepository(HseClassContext context)
        {
            _context = context;
        }
        
        public async Task<LabEntity> GetById(int labId)
        {
            return await _context.Labs.FirstOrDefaultAsync(l => l.Id == labId);
        }
        
        public async Task<LabEntity> Create(LabEntity labEntity)
        {
            var result = await _context.Labs.AddAsync(labEntity);
            await _context.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task Delete(int labId)
        {
            var company = await _context.Labs.FirstOrDefaultAsync(c => c.Id == labId);
            _context.Labs.Remove(company);
            await _context.SaveChangesAsync();
        }

        public async Task<List<LabEntity>> GetByClassId(int classId)
        {
            return await _context.Labs.Where(l => l.ClassRoomId == classId).ToListAsync();
        }

        public async Task<LabEntity> Update(LabEntity labEntity)
        {
            var result = _context.Labs.Update(labEntity);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}