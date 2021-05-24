using System;
using System.Collections.Generic;
using HseClass.Data.Entities;

namespace HseClass.Api.ViewModels.StudentViewModels
{
    public class StudentClassInfoViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public Guid Code { get; set; }
        
        public List<UserViewModel> Users { get; set; }

        public List<StudentLabInfoViewModel> Labs { get; set; }
    }

    public class StudentLabInfoViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public int TaskLabId { get; set; }

        public int ClassRoomId { get; set; }
        
        public int MaxGrade { get; set; }
        
        public DateTime Deadline { get; set; }
        
        public SolutionLab MySolution { get; set; }
        
        public List<SolutionLab> SolutionLabs { get; set; } = new List<SolutionLab>();
    }
}