using System.Threading.Tasks;
using HseClass.Data.Enums;

namespace HseClass.Api.ViewModels
{
    public class UserLabForm
    {
        public int? Grade { get; set; }
        
        public LabStatusEnums Status { get; set; }
    }
}