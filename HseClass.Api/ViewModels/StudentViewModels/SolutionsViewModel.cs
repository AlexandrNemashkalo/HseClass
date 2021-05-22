using System;
using System.Collections.Generic;
using HseClass.Data.Enums;

namespace HseClass.Api.ViewModels.StudentViewModels
{
    public class SolutionsViewModel
    {
        public List<SolutionViewModel> ActiveSolutions { get; set; }
        
        public List<SolutionViewModel> FinishedSolutions { get; set; }
    }

    public class SolutionViewModel
    {
        public int UserId { get; set; }
        
        public string Solution { get; set; }
        
        public LabInfo Lab { get; set; }
        
        public int? Grade { get; set; }
        
        public string VideoPath { get; set; }
        
        public string TimeSpan { get; set; }
        
        public LabStatusEnums Status { get; set; }
        
        public DateTime? DateOfDownload { get; set; }
    }

    public class LabInfo
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public TaskLabViewModel Task { get; set; }

        public int ClassRoomId { get; set; }
        
        public int MaxGrade { get; set; }
        
        public DateTime Deadline { get; set; }
    }
}