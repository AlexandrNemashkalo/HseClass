using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetById(int userId);
        
        Task<List<User>> GetByClassId(int classId);
    }
}