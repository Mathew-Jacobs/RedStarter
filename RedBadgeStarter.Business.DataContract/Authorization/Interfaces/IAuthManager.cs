using RedBadgeStarter.Business.DataContract.Authorization.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedBadgeStarter.Business.DataContract.Authorization.Interfaces
{
    public interface IAuthManager
    {
        Task<ReceivedExistingUserDTO> RegisterUser(RegisterUserDTO userDTO);
        Task<ReceivedExistingUserDTO> LoginUser(QueryForExistingUserDTO userDTO);
        Task<bool> UserExists(string username);
        Task<string> GenerateTokenString(QueryForExistingUserDTO userDTO);
    }
}
