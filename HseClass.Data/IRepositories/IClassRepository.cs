using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface IClassRepository
    {
        Task<Team> Create(Team cl);

        Task Delete(int classId);

        Task<List<Team>> GetByUserId(int userId);
        
        Task<Team> GetById(int classId);

        Task<Team> Update(Team cl);
    }
}