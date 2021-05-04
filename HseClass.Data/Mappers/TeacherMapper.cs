using HseClass.Core;
using HseClass.Core.Entities;
using HseClass.Core.Infrastructure;
using HseClass.Data.Models;

namespace HseClass.Data.Mappers
{
    public class TeacherMapper : IMapper<UserEntity, Teacher>
    {
        private readonly IUnitOfWork _data;

        public TeacherMapper(IUnitOfWork data)
        {
            _data = data;
        }
        
        public Teacher Map(UserEntity source)
        {
            return new Teacher()
            {
                Id = source.Id,
                UserName = source.UserName,
                Email = source.Email,
                Name = source.Name
            };

        }
    }
}