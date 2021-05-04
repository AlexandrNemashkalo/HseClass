using System;
using Microsoft.AspNetCore.Http;

namespace HseClass.Api.ViewModels
{
    public class UserLabSolutionForm
    {
        public string Solution { get; set; }
        
        public string TimeSpan { get; set; }
        
        public IFormFile Video { get; set; }
    }
}