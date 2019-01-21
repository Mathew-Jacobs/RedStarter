using AutoMapper;
using RedBadgeStarter.Business.DataContract.Authorization.DTOs;
using RedBadgeStarter.Business.DataContract.Authorization.Interfaces;
using RedBadgeStarter.Database.DataContract.Authorization.Interfaces;
using RedBadgeStarter.Database.DataContract.Authorization.RAOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedBadgeStarter.Business.Managers.Authorization
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly IAuthRepository _authRepository;

        public AuthManager(IMapper mapper, IAuthRepository authRepository)
        {
            _mapper = mapper;
            _authRepository = authRepository;
        }

        public async Task<ReceivedExistingUserDTO> LoginUser(QueryForExistingUserDTO queryForUserDTO)
        {
            var queryForUserRAO = _mapper.Map<QueryForExistingUserRAO>(queryForUserDTO);

            var receivedUser = await _authRepository.Login(queryForUserRAO);

            return _mapper.Map<ReceivedExistingUserDTO>(receivedUser);
        }

        public async Task<ReceivedExistingUserDTO> RegisterUser(RegisterUserDTO userDTO)
        {
            var rao = _mapper.Map<RegisterUserRAO>(userDTO);

            var returnedRAO = await _authRepository.Register(rao, userDTO.Password);

            if (returnedRAO != null) return _mapper.Map<ReceivedExistingUserDTO>(returnedRAO);

            return null;
        }

        public async Task<string> GenerateTokenString(QueryForExistingUserDTO userDTO)
        {
            var rao = _mapper.Map<QueryForExistingUserRAO>(userDTO);

            return await _authRepository.GenerateTokenString(rao);
        }

        public async Task<bool> UserExists(string username)
        {
            return await _authRepository.UserExists(username);
        }
    }
}