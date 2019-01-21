using AutoMapper;
using RedBadgeStarter.API.DataContract.Authorization;
using RedBadgeStarter.Business.DataContract.Authorization.DTOs;
using RedBadgeStarter.Database.DataContract.Authorization.RAOs;
using RedBadgeStarter.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedBadgeStarter.API.MappingProfiles.Authorization
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            //<-- Registration-oriented
            CreateMap<RegisterUserRequest, RegisterUserDTO>();
            CreateMap<RegisterUserDTO, RegisterUserRAO>();     //TODO: Need to work on improving our AutoMapper stuff ->  Action Handlers....Organize by theme
            CreateMap<RegisterUserRAO, UserEntity>();
            CreateMap<LoginUserRequest, QueryForExistingUserDTO>();

            //<-- Login-oriented
            CreateMap<QueryForExistingUserDTO, QueryForExistingUserRAO>();
            CreateMap<QueryForExistingUserRAO, UserEntity>();
            CreateMap<UserEntity, ReceivedExistingUserRAO>();
            CreateMap<ReceivedExistingUserRAO, ReceivedExistingUserDTO>();
            CreateMap<ReceivedExistingUserDTO, ReceivedExistingUserResponse>();
            CreateMap<LoginUserRequest, ReceivedExistingUserResponse>();

            //--  Authcheck-oriented
            CreateMap<RegisterUserRAO, QueryForExistingUserRAO>();
        }
    }
}
