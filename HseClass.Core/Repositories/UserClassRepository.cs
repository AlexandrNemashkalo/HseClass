using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using HseClass.Data.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HseClass.Core.Repositories
{
    public class UserClassRepository : IUserClassRepository
    {
        private readonly HseClassContext _context;

        public UserClassRepository(HseClassContext context)
        {
            _context = context;
            
        }
        public async Task<UserTeam> Create(int classId, int userId)
        {
            var result =await _context.UserTeams
                .AddAsync(new UserTeam()
                {
                    TeamId = classId,
                    UserId = userId
                });
            
            await _context.SaveChangesAsync();
            
            return result.Entity;
        }

        public async Task Delete(int classId, int userId)
        {
            var userClass = await _context.UserTeams
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.TeamId == classId);
            _context.UserTeams.Remove(userClass);
            await _context.SaveChangesAsync();
        }
    }
}