using System;
using System.Threading.Tasks;
using HseClass.Api.ViewModels;
using HseClass.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(object))]
        public async Task<ActionResult<object>> Login([FromBody] LoginForm form)
        {
            try
            {
                return new JsonResult(await _authService.Login(form.Email, form.Password));
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Регистрация студента
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(object))]
        public async Task<ActionResult<object>> RegisterStudent([FromBody] RegisterForm form)
        {
            try
            {
                return new JsonResult(await _authService.RegisterStudent(
                    form.Email,
                    form.Password,
                    form.Name));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        /// <summary>
        /// Регистрация учителя 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(object))]
        public async Task<ActionResult<object>> RegisterTeacher([FromBody] RegisterForm form)
        {
            try
            {
                return new JsonResult(await _authService.RegisterTeacher(
                    form.Email,
                    form.Password,
                    form.Name));
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
