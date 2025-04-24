using System.ComponentModel.DataAnnotations.Schema;
using BackendTask.Data.Models.Identity;
using BackendTask.Shared;
using BackendTask.Shared.Enums;
using ToDoTaskStatus = BackendTask.Shared.Enums.ToDoTaskStatus;

namespace BackendTask.Data.Models.ToDoTask
{
    internal class ToDoTask : AuditedEntity
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.Midium;
        public ToDoTaskStatus Status { get; set; } = ToDoTaskStatus.InCommpleted;
        public long OwnerId { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public UserAccount Owner { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
