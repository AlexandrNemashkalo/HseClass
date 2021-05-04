using HseClass.Core.Infrastructure;
using HseClass.Core.Entities;
using HseClass.Data.Models;

namespace HseClass.Data.Mappers
{
    public class UserMapper : IMapper<User, UserEntity>, IMapper<UserEntity, User>
    {
        public UserEntity Map(User source)
        {
            return new UserEntity()
            {
                Id = source.Id,
                UserName = source.UserName,
                Email = source.Email,
                Name = source.Name
            };
        }
        
        public User Map(UserEntity source)
        {
            return new User()
            {
                Id = source.Id,
                UserName = source.UserName,
                Email = source.Email,
                Name = source.Name
            };
        }
    }
}