using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface IClassRoomRepository
    {
        Task<ClassRoom> Create(ClassRoom cl);

        Task Delete(int classRoomId);

        Task<List<ClassRoom>> GetByUserId(int userId);
        
        Task<ClassRoom> GetById(int classRoomId);
        
        Task<ClassRoom> GetByCode(Guid code);

        Task<ClassRoom> Update(ClassRoom cl);
        
        
    }
}