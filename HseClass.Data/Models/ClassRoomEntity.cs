using System.Collections.Generic;

namespace HseClass.Data.Models
{
    public class ClassRoomEntity
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Code { get; set; }
        
        public List<UserClassEntity> UserClasses { get; set; } = new List<UserClassEntity>();
        
        public List<LabEntity> Labs { get; set; } = new List<LabEntity>();
    }
}