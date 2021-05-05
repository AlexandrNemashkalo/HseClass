using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HseClass.Api.Helpers;
using HseClass.Api.ViewModels;
using HseClass.Data.Entities;
using HseClass.Data.Enums;
using HseClass.Data.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HseClass.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AdminController : ControllerBase
    {
        private readonly ITaskLabRepository _taskLabRepository;
        
        public AdminController( 
            ITaskLabRepository taskLabRepository)
        {
            _taskLabRepository = taskLabRepository;
        }
        
        /// <summary>
        /// Создание задания (лабораторной)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TaskLab>> Post([FromBody] TaskLabForm form)
        {
            var taskLab = await _taskLabRepository.Create(new TaskLab()
            {
                CorrectSolution = form.CorrectSolution,
                Description = form.Description,
                Equipment = form.Equipment,
                Name = form.Name,
                Theme = form.Theme
                
            });
            
            return taskLab;
        }
        
        /// <summary>
        ///  Изменение задания
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPut("{taskId}")]
        public async Task<ActionResult<TaskLab>> Update([FromBody] TaskLabForm form, int taskId)
        {
            var t = await _taskLabRepository.GetById(taskId);
            
            t.Description = form.Description;
            t.Equipment = form.Equipment;
            t.Name = form.Name;
            t.Theme = form.Theme;
            t.CorrectSolution = form.CorrectSolution;
            
            return  await _taskLabRepository.Update(t);
        }

        /// <summary>
        ///  Удаление задания
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpDelete("{taskId}")]
        public async Task<ActionResult<bool>> Delete( int taskId)
        {
            try
            {
                await _taskLabRepository.Delete(taskId);
            }
            catch
            {
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Получение всех доступных заданий
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<TaskLabViewModel>>> Get()
        {
            var tasks = await _taskLabRepository.GetAll();

            return tasks.Select(t => new TaskLabViewModel()
            {
                Id = t.Id,
                Name = t.Name,
                Theme = t.Theme,
                Description = t.Description,
                Equipment = t.Equipment
            }).ToList();
        }
        
        /// <summary>
        /// Получение подробной информации о конкретном задании
        /// </summary>
        /// <returns></returns>
        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskLab>> Get(int taskId)
        {
            return await _taskLabRepository.GetById(taskId);
        }
    }
}