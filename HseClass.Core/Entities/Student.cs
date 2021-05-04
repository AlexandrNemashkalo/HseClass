using System.Collections.Generic;

namespace HseClass.Core.Entities
{
    public class Student : User
    {
        public List<ClassRoom> Classes { get; set; }
        
        public List<SolutionLab> SolutionLabs { get; set; }
    }


}