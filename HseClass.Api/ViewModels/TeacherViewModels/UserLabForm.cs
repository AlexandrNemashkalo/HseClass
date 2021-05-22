using HseClass.Data.Enums;

namespace HseClass.Api.ViewModels.TeacherViewModels
{
    public class UserLabForm
    {
        public int? Grade { get; set; }
        
        public LabStatusEnums Status { get; set; }
    }
}