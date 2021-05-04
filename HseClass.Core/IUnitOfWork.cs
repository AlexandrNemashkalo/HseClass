using HseClass.Core.IRepositories;

namespace HseClass.Core
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }
        
        //public ILabRepository Lab { get; }
        
        //public ISolutionLabRepository SolutionLab { get; }
        
        public IClassRoomRepository ClassRoom { get; }
        
        //public ITaskLabRepository TaskLab { get; }
        
        //public IStudentRepository Student { get; }
    }
}