using System;
using HseClass.Data.Entities;

namespace HseClass.Api.ViewModels
{
    public class LabInfoViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public TaskLabViewModel Task { get; set; }

        public int ClassRoomId { get; set; }
        
        public int MaxGrade { get; set; }
        
        public DateTime Deadline { get; set; }
        
        public SolutionLab Solution { get; set; }
    }

    public class TaskLabViewModel
    {
        public int Id { get; set; }

        public string Description { get; set; }
        
        public string RecommendedClass { get; set; }
        
        public string Equipment { get; set; }
        
        public string LinkToManual { get; set; }
        
        public string Name { get; set; }
        
        public string Theme { get; set; }
    }
}