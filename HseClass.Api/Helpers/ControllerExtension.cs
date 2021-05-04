using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HseClass.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Helpers
{
    public static class ControllerExtension
    {
        /// <summary>
        /// Получить id пользователя по токену
        /// </summary>
        internal static int GetUserIdFromToken(this ControllerBase apiController)
        {
            var nameIdentifier = apiController.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (nameIdentifier == null)
            {
                throw new Exception("User not found");
            }

            return int.Parse(nameIdentifier.Value);
        }
        
        /*internal static async Task CheckUserInClass(this ControllerBase apiController, User userEntity, int classId)
        {
            if (!userEntity.UserClasses.Any(uc => uc.ClassRoomId == classId))
            {
                throw new Exception("ошибка доступа");
            }
        }*/
    }
}