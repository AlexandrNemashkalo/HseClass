using HseClass.Core.Entities;
using HseClass.Core.Infrastructure;
using HseClass.Data.Models;

namespace HseClass.Data.Mappers
{
    public class ClassMapper : IMapper<ClassRoom, ClassRoomEntity>, IMapper<ClassRoomEntity, ClassRoom>
    {
        public ClassRoomEntity Map(ClassRoom source)
        {
            return new ClassRoomEntity()
            {
                Id = source.Id,
                Title = source.Title,
                Code = source.Code
            };
        }
        
        public ClassRoom Map(ClassRoomEntity source)
        {
            return new ClassRoom()
            {
                Id = source.Id,
                Title = source.Title,
                Code = source.Code
            };
        }
    }
}