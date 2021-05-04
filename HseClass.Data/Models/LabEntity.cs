using System;

namespace HseClass.Data.Models
{
    public class LabEntity
    {
        public int Id { get; set; }
        
        public string Task { get; set; }

        public int ClassRoomId { get; set; }
        
        public DateTime Deadline { get; set; }
    }
}