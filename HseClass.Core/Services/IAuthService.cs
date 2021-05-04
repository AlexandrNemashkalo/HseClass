using System.Threading.Tasks;

namespace HseClass.Core.Services
{
    public interface IAuthService
    {
        Task<object> Login(string email, string password);

        Task<object> RegisterStudent(string email, string password, string name);
        
        Task<object> RegisterTeacher(string email, string password, string name);
    }
}