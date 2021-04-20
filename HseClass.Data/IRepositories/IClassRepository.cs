using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface IClassRepository
    {
        Task<Class> Create(Class cl);

        Task Delete(int classId);

        Task<List<Class>> GetByUserId(int userId);
        
        Task<Class> GetById(int classId);

        Task<Class> Update(Class cl);
    }
}