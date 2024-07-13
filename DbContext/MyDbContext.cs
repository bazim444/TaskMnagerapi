using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.DbContextApp
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> TaskMaster { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.TaskId); // Define primary key
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description);
                entity.Property(e => e.DueDate);
                entity.Property(e => e.Status);
                entity.Property(e => e.CreatedBy);
            });
        }
    }
}
