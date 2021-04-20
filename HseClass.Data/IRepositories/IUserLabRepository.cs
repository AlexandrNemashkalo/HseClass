using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface IUserLabRepository
    {
        Task<UserLab> Create(UserLab userLab);
        
        Task<UserLab> Update(UserLab userLab);

        Task Delete(int userId, int labId);

        Task<List<UserLab>> GetByUserId(int userId);

        Task<List<UserLab>> GetByLabId(int labId);
        
        Task<UserLab> GetById(int userId, int labId);

    }
}