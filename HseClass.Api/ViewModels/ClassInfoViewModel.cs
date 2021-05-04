using System;
using System.Collections.Generic;
using HseClass.Data.Entities;

namespace HseClass.Api.ViewModels
{
    public class ClassInfoViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public Guid Code { get; set; }
        
        public List<UserViewModel> Users { get; set; }

        public List<Lab> Labs { get; set; }
    }
}