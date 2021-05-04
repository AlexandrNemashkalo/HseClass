using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class TaskLabRepository : ITaskLabRepository
    {
        private readonly HseClassContext _context;

        public TaskLabRepository(HseClassContext context)
        {
            _context = context;
        }
        
        
        public async Task<TaskLab> Create(TaskLab taskLab)
        {
            var result = await _context.TaskLabs.AddAsync(taskLab);
            await _context.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task Delete(int taskLabId)
        {
            var taskLab = await _context.TaskLabs.FirstOrDefaultAsync(c => c.Id == taskLabId);
            _context.TaskLabs.Remove(taskLab);
            await _context.SaveChangesAsync();
        }

        public async  Task<List<TaskLab>> GetAll()
        {
            return await _context.TaskLabs.ToListAsync();
        }

        public async Task<TaskLab> Update(TaskLab taskLab)
        {
            var result = _context.TaskLabs.Update(taskLab);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<TaskLab> GetById(int labId)
        {
            return await _context.TaskLabs.FirstOrDefaultAsync(l => l.Id == labId);
        }
    }
}