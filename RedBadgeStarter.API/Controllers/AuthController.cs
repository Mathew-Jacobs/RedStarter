using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedBadgeStarter.API.DataContract.Authorization;
using RedBadgeStarter.Business.DataContract.Authorization.DTOs;
using RedBadgeStarter.Business.DataContract.Authorization.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedBadgeStarter.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthManager _authManager;
        private readonly IMapper _mapper;

        public AuthController(IAuthManager authManager, IMapper mapper)
        {
            _authManager = authManager;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest userForRegister)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userDTO = _mapper.Map<RegisterUserDTO>(userForRegister);

            if (await _authManager.UserExists(userDTO.UserName))
                return BadRequest("Username is already taken");

            var returnedUser = await _authManager.RegisterUser(userDTO);

            if (returnedUser != null) return StatusCode(201);

            return StatusCode(500);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest userForLogin)
        {
            var userDTO = _mapper.Map<QueryForExistingUserDTO>(userForLogin);

            var returnedUser = await _authManager.LoginUser(userDTO);

            if (returnedUser == null) return Unauthorized();

            var token = await _authManager.GenerateTokenString(userDTO);

            return Ok(new { token });
        }
    }
}
