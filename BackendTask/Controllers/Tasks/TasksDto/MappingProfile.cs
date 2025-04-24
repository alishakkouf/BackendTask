using AutoMapper;
using BackendTask.Domain.Models.TasksDomain;
using BackendTask.Shared;

namespace BackendTask.API.Controllers.Tasks.TasksDto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTaskDto, CreateTaskDomain>();
            CreateMap<UpdateTaskDto, UpdateTaskDomain>();
            CreateMap<TaskDomain, TaskDto>();
            CreateMap<PagedResultDto<TaskDomain>, PagedResultDto<TaskDto>>();
        }
    }
}
