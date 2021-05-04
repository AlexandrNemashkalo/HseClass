using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface ISolutionLabRepository
    {
        Task<SolutionLab> Create(SolutionLab solutionLab);
        
        Task<SolutionLab> Update(SolutionLab solutionLab);

        Task Delete(int userId, int labId);

        Task<List<SolutionLab>> GetByUserId(int userId);

        Task<List<SolutionLab>> GetByLabId(int labId);
        
        Task<SolutionLab> GetById(int userId, int labId);

    }
}