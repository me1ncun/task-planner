using AutoMapper;
using taskplanner_user_service.DTOs;
using taskplanner_user_service.Models;

namespace taskplanner_user_service.Mappers;

public class UserProfile: Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserRequest, User>();
        CreateMap<LoginUserRequest, User>();
        CreateMap<User, RegisterUserResponse>();
        CreateMap<User, LoginUserResponse>();
        CreateMap<User, LoginUserRequest>();
    }
}