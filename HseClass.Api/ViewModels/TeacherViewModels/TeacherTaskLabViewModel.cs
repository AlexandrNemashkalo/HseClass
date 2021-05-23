using System;
using HseClass.Data.Enums;

namespace HseClass.Api.ViewModels.TeacherViewModels
{
    public class TeacherTaskLabViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }
        
        public string RecommendedClass { get; set; }
        
        public string Equipment { get; set; }
        
        public string LinkToManual { get; set; }
        
        public string Name { get; set; }
        
        public string Theme { get; set; }
        
        public string CorrectSolution { get; set; }
    }
    
    
    public class TeacherLabSolutionViewModel
    {
        public int UserId { get; set; }
        
        public string UserEmail { get; set; }
        
        public string UserName { get; set; }
        
        public string Solution { get; set; }
        
        public int LabId { get; set; }
        
        public int? Grade { get; set; }
        
        public string VideoPath { get; set; }
        
        public string TimeSpan { get; set; }
        
        public LabStatusEnums Status { get; set; }
        
        public DateTime? DateOfDownload { get; set; }
    }
}