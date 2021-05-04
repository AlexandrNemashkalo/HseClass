using System.Collections.Generic;

namespace HseClass.Core.Entities
{
    public class ClassRoom
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Code { get; set; }
        
        public List<Student> Students { get; set; }
        
        public Teacher Teacher { get; set; }
        
        public List<Lab> Labs { get; set; }
        
    }
}