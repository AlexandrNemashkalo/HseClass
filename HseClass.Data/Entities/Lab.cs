using System;
using HseClass.Data.Enums;

namespace HseClass.Data.Entities
{
    public class Lab
    {
        public int Id { get; set; }
        
        public string Task { get; set; }

        public int ClassId { get; set; }
        
        public DateTime Deadline { get; set; }
    }
}