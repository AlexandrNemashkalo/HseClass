using System;
using System.Collections.Generic;
using HseClass.Data.Entities;

namespace HseClass.Api.ViewModels.StudentViewModels
{
    public class StudentClassViewModel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string TeacherName { get; set; }
        
        public string TeacherEmail { get; set; }
        
        public Guid Code { get; set; }
        
        public List<UserClass> UserClasses { get; set; } = new List<UserClass>();
        
        public List<Lab> Labs { get; set; } = new List<Lab>();
    }
}