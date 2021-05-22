using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface ILabRepository
    {
        Task<Lab> Create(Lab lab);
        
        Task Delete(int labId);

        Task<List<Lab>> GetByClassId(int classId);

        Task<Lab> Update(Lab lab);
        
        Task<Lab> GetById(int labId);
        Task<Lab> GetByIdWithSolutions(int labId);
    }
}