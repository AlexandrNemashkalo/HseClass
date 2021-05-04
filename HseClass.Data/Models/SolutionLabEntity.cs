using System;

namespace HseClass.Data.Models
{
    public class SolutionLabEntity
    {
        public int UserId { get; set; }
        
        public string Solution { get; set; }
        
        public int LabId { get; set; }
        
        public int? Grade { get; set; }
        
        public int LabStatusId { get; set; }
        
        public DateTime? DateOfDownload { get; set; }
    }
}