using HseClass.Core.IRepositories;

namespace HseClass.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IUserRepository _userRepo;
        //private readonly ILabRepository _labRepo;
        //private readonly ISolutionLabRepository _solutionLabRepo;
        private readonly IClassRoomRepository _classRepo;
        //private readonly ITaskLabRepository _taskLabRepo;
        //private readonly IStudentRepository _studentRepo;
        
        public UnitOfWork(
            IUserRepository userRepo,
            //ILabRepository labRepo,
            //ISolutionLabRepository solutionLabRepo, 
            IClassRoomRepository classRepo
            //ITaskLabRepository taskLabRepo,
            //IStudentRepository studentRepo
            )
        {
            _userRepo = userRepo;
            //_labRepo = labRepo;
            //_solutionLabRepo = solutionLabRepo;
            _classRepo = classRepo;
            //_taskLabRepo = taskLabRepo;
            //_studentRepo = studentRepo;*/
        }

        public IUserRepository User => _userRepo;

        //public ILabRepository Lab => _labRepo;

        //public ISolutionLabRepository SolutionLab => _solutionLabRepo;

        public IClassRoomRepository ClassRoom => _classRepo;

        //public ITaskLabRepository TaskLab => _taskLabRepo;

        //public IStudentRepository Student => _studentRepo;
    }
}