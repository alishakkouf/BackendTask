using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BackendTask.Domain.Models.UserAccount;

namespace BackendTask.Data.Models.Identity
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserAccount, UserAccountDomain>()
                .ForMember(x=>x.FullName, o=> o.MapFrom(x=>$"{x.FirstName} {x.LastName}"));
            CreateMap<RegisterInputDomain, UserAccount>();
            CreateMap<UpdateUserDomain, UserAccount>();
            CreateMap<UserRole, RoleDomain>();
        }
    }
}
