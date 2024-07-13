using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TaskManager.DbContextApp;
using TaskManager.Models;
using TaskManager.Repositories;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace TaskManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepo _taskRepo;
        private readonly IConfiguration _configuration;
        private readonly MyDbContext _context;

        public TaskController(ITaskRepo taskRepo, IConfiguration configuration, MyDbContext context)
        {
            _taskRepo = taskRepo;
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("GetAllTasks")]
        public async Task<ActionResult<List<TaskItem>>> GetAllTasks()
        {
            var tasks = await _taskRepo.GetAllTasksAsync();
            return Ok(tasks);
        }

        //// GET api/Task/5
        //[HttpGet("GetTaskById{id}")]
        //public IActionResult GetTaskById(int id)
        //{
        //    var task = _taskRepo.GetTaskByIdAsync(id); // Assuming GetTaskByIdAsync returns TaskItem
        //    if (task == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(task);
        //}

        // POST api/Task/AddTask
        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] AddTaskModel model)
        {
            TaskReposDAL Trepo = new TaskReposDAL(_context, _configuration.GetConnectionString("DefaultConnection"));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = Trepo.AddTaskRepo(model); 
                return Ok(new { message = "Task added successfully", data = result });
            }
            catch (SqlException sqlEx)
            {
             
                return StatusCode(500, new { message = "A database error occurred.", details = sqlEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal server error occurred.", details = ex.Message });
            }
        }


        [HttpPut("UpdateTask/{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var existingTask = await _context.TaskMaster.FindAsync(model.TaskId);
                // Convert UpdateTaskModel to TaskItem
                TaskItem taskToUpdate = new TaskItem
                {
                    TaskId = model.TaskId,
                    Status = model.Status,
                    Title= existingTask.Title,
                    Description= existingTask.Description,
                    CreatedBy=existingTask.CreatedBy,
                    DueDate=existingTask.DueDate,


                };

                await _taskRepo.UpdateTaskAsync(taskToUpdate);
                return Ok(taskToUpdate); 
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception or handle as appropriate
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpPut("DeleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskRepo.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            await _taskRepo.DeleteTaskAsync(id);
            return Ok("Deleted"); // Successfully deleted
        }
        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] AddUserModels obj)
        {
            TaskReposDAL Trepo = new TaskReposDAL(_context, _configuration.GetConnectionString("DefaultConnection"));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = Trepo.AddUser(obj);

            if (result == "User added successfully")
            {
                return Ok(result);
            }
            else if (result == "User already exists")
            {
                return Conflict(result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpPost("Login")]
        public IActionResult LoginUser([FromBody] AddUserModels obj)
        {
            TaskReposDAL Trepo = new TaskReposDAL(_context, _configuration.GetConnectionString("DefaultConnection"));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = Trepo.Login(obj);

                if (result == "1")
                {
                    return Ok(result);
                }
                else
                {
                    return Unauthorized("username and  password not matching");
                }
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, new { message = "A database error occurred.", details = sqlEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal server error occurred.", details = ex.Message });
            }
        }


    }
}
