using System.Collections.Generic;

namespace HseClass.Data.Entities
{
    public class Team
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public List<UserTeam> UserClasses { get; set; } = new List<UserTeam>();
        
        public List<Lab> Labs { get; set; } = new List<Lab>();
    }
}