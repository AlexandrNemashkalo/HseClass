using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HseClass.Data.Models
{
    public class UserEntity : IdentityUser<int>
    {
        public string Name { get; set; }

        public List<UserClassEntity> UserClasses { get; set; } = new List<UserClassEntity>();
        
        public List<SolutionLabEntity> UserLabs { get; set; } = new List<SolutionLabEntity>();
        
    }
}