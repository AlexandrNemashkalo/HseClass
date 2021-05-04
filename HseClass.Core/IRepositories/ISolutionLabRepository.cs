using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Core.Entities;

namespace HseClass.Core.IRepositories
{
    public interface ISolutionLabRepository
    {
        Task<SolutionLab> Create(SolutionLab solutionLabEntity);
        
        Task<SolutionLab> Update(SolutionLab solutionLabEntity);

        Task Delete(int userId, int labId);

        Task<List<SolutionLab>> GetByUserId(int userId);

        Task<List<SolutionLab>> GetByLabId(int labId);
        
        Task<SolutionLab> GetById(int userId, int labId);

    }
}