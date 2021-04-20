﻿using System.Collections.Generic;

namespace HseClass.Data.Entities
{
    public class Class
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public List<UserClass> UserClasses { get; set; } = new List<UserClass>();
        
        public List<Lab> Labs { get; set; } = new List<Lab>();
    }
}