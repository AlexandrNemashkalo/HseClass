using System;
using System.Collections.Generic;
using HseClass.Data.Entities;

namespace HseClass.Api.ViewModels.TeacherViewModels
{
    public class TeacherLabInfo
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public TeacherTaskLabViewModel Task { get; set; }

        public int ClassRoomId { get; set; }
        
        public int MaxGrade { get; set; }
        
        public DateTime Deadline { get; set; }
        
        public List<SolutionLab> Solutions { get; set; }
    }
}