using System;
using System.Collections.Generic;

namespace HseClass.Core.Entities
{
    public class Lab
    {
        public int Id { get; set; }
        
        public ClassRoom ClassRoom { get; set; }

        public List<SolutionLab> Solutions { get; set; }
        
        public TaskLab TaskLab { get; set; }

        public DateTime Deadline { get; set; }
        
        public int MaxGrade { get; set; }
    }
}