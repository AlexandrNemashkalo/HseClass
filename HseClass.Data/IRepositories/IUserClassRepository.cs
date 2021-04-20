using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface IUserClassRepository
    {
        Task<UserClass> Create(int classId, int userId);
        
        Task Delete(int classId, int userId);
    }
}