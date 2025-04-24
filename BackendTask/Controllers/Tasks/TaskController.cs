using AutoMapper;
using BackendTask.API.Controllers.Tasks.TasksDto;
using BackendTask.Common;
using BackendTask.Domain;
using BackendTask.Domain.Authorization;
using BackendTask.Domain.Contract.TasksContract;
using BackendTask.Domain.Models.TasksDomain;
using BackendTask.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace BackendTask.API.Controllers.Tasks
{
    public class TaskController(IToDoTaskManager TaskManager, ICurrentUserService currentUserService, IMapper mapper, IStringLocalizerFactory factory)
        : BaseApiController(factory)
    {
        private readonly IToDoTaskManager _TaskManager = TaskManager;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IMapper _mapper = mapper;

        [HttpGet("Get")]
        [Authorize(Permissions.ToDoTask.View)]
        public async Task<ActionResult<TaskDto>> GetById(int id)
        {
            var task = await _TaskManager.GetByIdAsync(id);
            
            return Ok(_mapper.Map<TaskDto>(task));
        }

        [HttpGet("GetAll")]
        [Authorize(Permissions.ToDoTask.View)]
        public async Task<ActionResult<PagedResultDto<TaskDto>>> GetAll([FromQuery] PagedAndSortedResultRequestDto request)
        {
            var tasks = await _TaskManager.GetAllAsync(request);
            
            return Ok(_mapper.Map<PagedResultDto<TaskDto>>(tasks));
        }

        [HttpGet("MyTasks")]
        [Authorize(Permissions.ToDoTask.View)]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetMyTasks()
        {
            var tasks = await _TaskManager.GetByOwnerAsync((_currentUserService.GetUserId()).Value);
           
            return Ok(_mapper.Map<List<TaskDto>>(tasks));
        }

        [HttpGet("GetByStatus")]
        [Authorize(Permissions.ToDoTask.View)]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetByStatus(Shared.Enums.ToDoTaskStatus status)
        {
            var tasks = await _TaskManager.GetByStatusAsync(status);
            
            return Ok(_mapper.Map<List<TaskDto>>(tasks));
        }

        [HttpPost("Create")]
        [Authorize(Permissions.ToDoTask.Create)]
        public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto createTaskDto)
        {
            var ownerId = (_currentUserService.GetUserId()).Value;

            var input = _mapper.Map<CreateTaskDomain>(createTaskDto);

            input.OwnerId = ownerId;

            await _TaskManager.AddAsync(input);

            return Ok();

        }


        [HttpPost("SetAsCompleted")]
        [Authorize(Permissions.ToDoTask.Update)]
        public async Task<ActionResult<TaskDto>> SetAsCompletedAsync(int id)
        {
            await _TaskManager.SetAsCompletedAsync(id);

            return Ok();

        }

        [HttpPut("Update")]
        [Authorize(Permissions.ToDoTask.Update)]
        public async Task<ActionResult<TaskDto>> Update([FromBody] UpdateTaskDto updateTaskDto)
        {
            var ownerId = (_currentUserService.GetUserId()).Value;

            var input = _mapper.Map<UpdateTaskDomain>(updateTaskDto);

            input.OwnerId = ownerId;

            await _TaskManager.UpdateAsync(input);

            return Ok();
        }

        [HttpDelete("Delete")]
        [Authorize(Permissions.ToDoTask.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            await _TaskManager.DeleteAsync(id);

            return Ok();
        }
    }
}


