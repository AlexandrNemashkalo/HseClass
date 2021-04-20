using System;
using System.ComponentModel.DataAnnotations;

namespace HseClass.Api.ViewModels
{
    public class LabForm
    {
        public string Task { get; set; }

        [Required]
        public int ClassId { get; set; }
        
        public DateTime Deadline { get; set; }
    }
}