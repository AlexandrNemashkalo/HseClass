using System.Threading.Tasks;
using HseClass.Core.Entities;

namespace HseClass.Api.ViewModels
{
    public class UserLabForm
    {
        public int? Grade { get; set; }
        
        public LabStatus Status { get; set; }
    }
}