using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HseClass.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public List<UserTeam> UserClasses { get; set; } = new List<UserTeam>();
        
        public List<UserLab> UserLabs { get; set; } = new List<UserLab>();
        
    }
}