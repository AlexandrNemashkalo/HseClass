using System.Threading.Tasks;

namespace HseClass.Core.Services
{
    public interface IAuthService
    {
        Task<object> Login(string email, string password);

        Task<object> Register(string email, string password, string name, RoleEnums role);
    }
}