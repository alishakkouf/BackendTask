using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Domain.Models.TasksDomain;
using BackendTask.Shared;

namespace BackendTask.Domain.Contract.TasksContract
{
    public interface IToDoTaskManager
    {
        Task<TaskDomain> GetByIdAsync(int id);
        Task<PagedResultDto<TaskDomain>> GetAllAsync(PagedAndSortedResultRequestDto request);
        Task<IEnumerable<TaskDomain>> GetByOwnerAsync(long ownerId);
        Task<IEnumerable<TaskDomain>> GetByStatusAsync(Shared.Enums.ToDoTaskStatus status);
        Task AddAsync(CreateTaskDomain task);
        Task SetAsCompletedAsync(int id);
        Task UpdateAsync(UpdateTaskDomain task);
        Task DeleteAsync(int id);
    }
}
