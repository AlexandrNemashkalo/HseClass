using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Core.Entities;

namespace HseClass.Core.IRepositories
{
    public interface IClassRoomRepository
    {
        Task<ClassRoom> Create(ClassRoom cl);

        Task Delete(int classRoomId);

        Task<List<ClassRoom>> GetByUserId(int userId);
        
        Task<ClassRoom> GetById(int classRoomId);

        Task<ClassRoom> Update(ClassRoom cl);
    }
}