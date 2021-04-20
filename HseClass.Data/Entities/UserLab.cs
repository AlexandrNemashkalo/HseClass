using System;
using HseClass.Data.Enums;

namespace HseClass.Data.Entities
{
    public class UserLab
    {
        public int UserId { get; set; }
        
        public string Solution { get; set; }
        
        public int LabId { get; set; }
        
        public int? Grade { get; set; }
        
        public LabStatusEnums Status { get; set; }
        
        public DateTime? DateOfDownload { get; set; }
    }
}