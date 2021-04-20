using System.Threading.Tasks;
using HseClass.Data.Entities;

namespace HseClass.Core.Jwt
{
    public interface IJwtGenerator
    {
        Task<object> GenerateJwt(User user);
    }
}