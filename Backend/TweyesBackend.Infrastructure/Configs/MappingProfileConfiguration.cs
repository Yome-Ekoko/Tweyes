using AutoMapper;
using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Entities;

namespace TweyesBackend.Infrastructure.Configs
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<T_User, AuthenticationResponse>(MemberList.None);
            CreateMap<T_User, UserResponse>(MemberList.None);
            CreateMap<AddUserRequest, T_User>(MemberList.None);
            CreateMap<AddTutorRequest, Tutor>(MemberList.None);
            CreateMap<Tutor, AddTutorResponse>(MemberList.None);
           
        }
    }
}
