using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Core.Entities;

namespace HseClass.Core.IRepositories
{
    public interface ILabRepository
    {
        Task<Lab> Create(Lab labEntity);
        
        Task Delete(int labId);

        Task<List<Lab>> GetByClassId(int classId);

        Task<Lab> Update(Lab labEntity);
        
        Task<Lab> GetById(int labId);
    }
}