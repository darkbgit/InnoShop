using AutoMapper;
using UserManagement.Core.DTOs;
using UserManagement.Domain.Entities;

namespace UserManagement.Core.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, UserWithRolesDto>();
    }
}
