﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HseClass.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }

        public List<UserClass> UserClasses { get; set; } = new List<UserClass>();
        
        public List<UserLab> UserLabs { get; set; } = new List<UserLab>();
        
    }
}