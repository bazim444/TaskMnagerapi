using Microsoft.EntityFrameworkCore;
using TaskManager.DbContextApp;
using TaskManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Repositories
{
    public class TaskRepo : ITaskRepo
    {
        private readonly MyDbContext _context;

        public TaskRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return (IEnumerable<TaskItem>)await _context.TaskMaster.ToListAsync();
        }



        public async Task UpdateTaskAsync(TaskItem obj)
        {
            var existingTask = await _context.TaskMaster.FindAsync(obj.TaskId);

            if (existingTask == null)
            {
                throw new ArgumentException($"Task with ID {obj.TaskId} not found.");
            }

            // Update properties based on the passed UpdateTaskModel
            existingTask.Status = obj.Status;

            // Update the entity in the DbContext and save changes
            _context.TaskMaster.Update(existingTask);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.TaskMaster.FindAsync(id);
            if (task != null)
            {
                _context.TaskMaster.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id) => await _context.TaskMaster.FindAsync(id);

        public Task AddTaskAsync(TaskItem task)
        {
            throw new NotImplementedException();
        }

    }
}
