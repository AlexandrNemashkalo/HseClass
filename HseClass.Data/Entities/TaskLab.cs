using System.Collections.Generic;

namespace HseClass.Data.Entities
{
    public class TaskLab
    {
        public int Id { get; set; }

        public string Description { get; set; }
        
        public string Equipment { get; set; }
        
        public string Name { get; set; }
        
        public string Theme { get; set; }
        
        public string CorrectSolution { get; set; }
        
        public string RecommendedClass { get; set; }

        public List<Lab> Labs { get; set; } = new List<Lab>();
    }
}