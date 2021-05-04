using System;
using System.Collections.Generic;
using HseClass.Data.Enums;

namespace HseClass.Data.Entities
{
    public class Lab
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public int TaskLabId { get; set; }

        public int ClassRoomId { get; set; }
        
        public int MaxGrade { get; set; }
        
        public DateTime Deadline { get; set; }
        
        public List<SolutionLab> SolutionLabs { get; set; } = new List<SolutionLab>();
    }
}