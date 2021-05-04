using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Core.Entities;

namespace HseClass.Core.Services
{
    public interface StudentService
    {
        Task<List<ClassRoom>> GetClasses();
                
        Task<List<Lab>> GetLabsFromClass();
                
        Task UpdateSolutionLab(Lab lab);
                
        Task<List<SolutionLab>> GetLab();
    }
}