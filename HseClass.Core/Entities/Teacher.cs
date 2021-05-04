using System.Collections.Generic;
using System.Threading.Tasks;
using HseClass.Core.IEntities;

namespace HseClass.Core.Entities
{
    public class Teacher : User
    {
        public List<ClassRoom> Classes { get; set; }
        
    }
}