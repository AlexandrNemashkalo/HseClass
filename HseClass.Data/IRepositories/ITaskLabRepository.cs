using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Data.IRepositories
{
    public interface ITaskLabRepository
    {
        Task<TaskLab> Create(TaskLab taskLab);

        Task Delete(int taskLabId);

        Task<List<TaskLab>> GetAll();

        Task<TaskLab> Update(TaskLab lab);
        
        Task<TaskLab> GetById(int labId);
    }
}