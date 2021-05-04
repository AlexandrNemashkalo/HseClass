using System;

namespace HseClass.Core.Entities
{
    public class SolutionLab
    {
        public (int, int) Id { get; set; }
        
        public Student User { get; set; }
        
        public string Solution { get; set; }
        
        public Lab Lab { get; set; }
        
        public int? Grade { get; set; }
        
        public LabStatus Status { get; set; }
        
        public DateTime? DateOfDownload { get; set; }
    }
}