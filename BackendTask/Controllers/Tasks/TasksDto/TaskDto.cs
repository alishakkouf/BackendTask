using BackendTask.Shared.Enums;
using ToDoTaskStatus = BackendTask.Shared.Enums.ToDoTaskStatus;

namespace BackendTask.API.Controllers.Tasks.TasksDto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityLevel Priority { get; set; }
        public ToDoTaskStatus Status { get; set; }
        public string OwnerId { get; set; }
        public string OwnerName { get; set; }

        public int CategoryId { get; set; }
    }
}
