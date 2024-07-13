using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models
{
    [Table("Task_Master")]
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public required string Title { get; set; }

        public required string Description { get; set; }

        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public required string Status { get; set; }

        [Required(ErrorMessage = "Created By is required")]
        public required string CreatedBy { get; set; }
    }
}
[Table("Users")]
public class UserItem
{
    [Key]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    public required string UserName { get; set; }

    public required string Email { get; set; }


}

