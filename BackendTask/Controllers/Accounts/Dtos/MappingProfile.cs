using AutoMapper;
using BackendTask.Domain.Models.UserAccount;

namespace BackendTask.API.Controllers.Accounts.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterInputDto, RegisterInputDomain>();
            CreateMap<UpdateUserDto, UpdateUserDomain>();
        }
    }
}
