namespace TaskTracker.Cli.Models;

    public enum TaskStatus{
        Todo,
        InProgress,
        Done
    }
    public class TaskItem
    {
        public int Id { get; set; }
        public string Description { get; set; }= string.Empty;
        public TaskStatus Status { get; set; }= TaskStatus.Todo;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt{ get; set; }
    }
