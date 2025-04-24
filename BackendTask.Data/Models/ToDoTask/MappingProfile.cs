using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BackendTask.Domain.Models.TasksDomain;

namespace BackendTask.Data.Models.ToDoTask
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateTaskDomain, ToDoTask>();
            CreateMap<UpdateTaskDomain, ToDoTask>();
            CreateMap<TaskDomain, ToDoTask>().ReverseMap();
        }
    }
}
