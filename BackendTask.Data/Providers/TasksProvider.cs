using AutoMapper;
using BackendTask.Data.Models.ToDoTask;
using BackendTask.Domain.Contract.TasksContract;
using BackendTask.Domain.Models.TasksDomain;
using BackendTask.Shared;
using BackendTask.Shared.Enums;
using BackendTask.Shared.Exceptions;
using BackendTask.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BackendTask.Data.Providers
{
    internal class TasksProvider(IMapper mapper, BackendTaskDbContext context) :
                   GenericProvider<ToDoTask>(context, mapper), ITaskProvider
    {
        public async Task AddAsync(CreateTaskDomain task)
        {
            var toBeInserted = _mapper.Map<ToDoTask>(task);
            
            await _dbContext.Tasks.AddAsync(toBeInserted);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await ActiveDbSet.FirstOrDefaultAsync(x => x.Id == id) ??
                         throw new EntityNotFoundException(nameof(ToDoTask), id.ToString());

            await SoftDeleteAsync(data);
        }

        public async Task<PagedResultDto<TaskDomain>> GetAllAsync(PagedAndSortedResultRequestDto request)
        {
            var data = ActiveDbSet.AsNoTracking();

            // filters
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                data = data.Where(x => x.Title.Contains(request.Keyword)
                                    || x.Description.Contains(request.Keyword));
            }

            // order
            if (!string.IsNullOrWhiteSpace(request.SortingField))
            {
                var sortDirection = request.SortingDir ?? SortingDirection.Asc;
                
                data = data.OrderBy(request.SortingField, sortDirection);
            }
            else
                data = data.OrderBy(_ => _.Id, SortingDirection.Desc);

            // pagination
            var pagedResult = await ApplyPagingAsync<TaskDomain, ToDoTask, PagedAndSortedResultRequestDto>(data, request);

            var result = new PagedResultDto<TaskDomain>(pagedResult);

            return result;
        }

        public async Task<TaskDomain> GetByIdAsync(int id)
        {
            var data = await ActiveDbSet.FirstOrDefaultAsync(x => x.Id == id) ??
                       throw new EntityNotFoundException(nameof(ToDoTask), id.ToString());

            return _mapper.Map<TaskDomain>(data);   
        }

        public async Task<IEnumerable<TaskDomain>> GetByOwnerAsync(long ownerId)
        {
            var data = ActiveDbSet.AsNoTracking().Where(x=>x.OwnerId == ownerId);

            return _mapper.Map<List<TaskDomain>>(await data.ToListAsync());
        }

        public async Task<IEnumerable<TaskDomain>> GetByStatusAsync(Shared.Enums.ToDoTaskStatus status)
        {
            var data = ActiveDbSet.AsNoTracking().Where(x => x.Status == status);

            return _mapper.Map<List<TaskDomain>>(await data.ToListAsync());
        }

        public async Task SetAsCompletedAsync(int id)
        {
            var data = await ActiveDbSet.FirstOrDefaultAsync(x => x.Id == id) ??
           throw new EntityNotFoundException(nameof(ToDoTask), id.ToString());

            data.Status = Shared.Enums.ToDoTaskStatus.Completed;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateTaskDomain task)
        {
            var data = await ActiveDbSet.FirstOrDefaultAsync(x => x.Id == task.Id) ??
            throw new EntityNotFoundException(nameof(ToDoTask), task.Id.ToString());

            _mapper.Map(task, data);

            await _dbContext.SaveChangesAsync();
        }
    }
}
