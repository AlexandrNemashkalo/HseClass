using System;
using System.ComponentModel.DataAnnotations;

namespace HseClass.Api.ViewModels
{
    public class LabForm
    {
        public string Title { get; set; }
        
        [Required]
        public int TaskLabId { get; set; }

        [Required]
        public int ClassRoomId { get; set; }
        
        [Required]
        public int MaxGrade { get; set; }
        
        public DateTime Deadline { get; set; }
    }
}