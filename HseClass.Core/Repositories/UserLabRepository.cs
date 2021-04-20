using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class UserLabRepository : IUserLabRepository
    {
        private readonly HseClassContext _context;

        public UserLabRepository(HseClassContext context)
        {
            _context = context;
            
        }
        public async Task<UserLab> Create(UserLab userLab)
        {
            var result =await _context.UserLabs.AddAsync(userLab);
            await _context.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<UserLab> Update(UserLab userLab)
        {
            var result = _context.UserLabs.Update(userLab);
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

        public async Task<List<UserLab>> GetByUserId(int userId)
        {
            return await _context.UserLabs.Where(ul => ul.UserId == userId).ToListAsync();
        }

        public async Task<List<UserLab>> GetByLabId(int labId)
        {
            return await _context.UserLabs.Where(ul => ul.LabId == labId).ToListAsync();
        }

        public async Task<UserLab> GetById(int userId, int labId)
        {
            return  await _context.UserLabs
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LabId ==labId);
        }
    }
}