using Microsoft.VisualBasic;

namespace TaskManager
{
    public class AddUserModels
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        = string.Empty; 
    }
    public class UpdateUserModels
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        = string.Empty;
    }
    public  class AddTaskModel
    {
        public required string TaskName { get; set;}
        public required string Description { get; set; }
        public required string Status { get; set; }
        public required string CreatedBy { get; set; }
        public required DateTime? DueDate  { get; set;}
        public DateTime CreatedOn { get; set;}
    }
    public class UpdateTaskModel
    {
        public int TaskId { get; set; }
        
        public required string Status { get; set; }
    }
    public class UserModel
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
     
    }
}

