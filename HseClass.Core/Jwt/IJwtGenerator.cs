using HseClass.Core.Entities;
using System.Threading.Tasks;

namespace HseClass.Core.Jwt
{
    public interface IJwtGenerator
    {
        Task<object> GenerateJwt(User userEntity);
    }
}