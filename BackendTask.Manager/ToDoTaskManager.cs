using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Domain.Contract.TasksContract;
using BackendTask.Domain.Models.TasksDomain;
using BackendTask.Shared;

namespace BackendTask.Manager
{
    public class ToDoTaskManager(ITaskProvider provider) : IToDoTaskManager
    {
        private readonly ITaskProvider _provider = provider;

        public async Task AddAsync(CreateTaskDomain task)
        {
            await _provider.AddAsync(task);
        }

        public async Task DeleteAsync(int id)
        {
            await _provider.DeleteAsync(id);
        }

        public async Task<PagedResultDto<TaskDomain>> GetAllAsync(PagedAndSortedResultRequestDto request)
        {
           return await _provider.GetAllAsync(request);
        }

        public async Task<TaskDomain> GetByIdAsync(int id)
        {
            return await _provider.GetByIdAsync(id);
        }

        public async Task<IEnumerable<TaskDomain>> GetByOwnerAsync(long ownerId)
        {
            return await _provider.GetByOwnerAsync(ownerId);
        }

        public async Task<IEnumerable<TaskDomain>> GetByStatusAsync(Shared.Enums.ToDoTaskStatus status)
        {
            return await _provider.GetByStatusAsync(status);
        }

        public async Task SetAsCompletedAsync(int id)
        {
            await _provider.SetAsCompletedAsync(id);
        }

        public async Task UpdateAsync(UpdateTaskDomain task)
        {
            await _provider.UpdateAsync(task);
        }
    }
}
