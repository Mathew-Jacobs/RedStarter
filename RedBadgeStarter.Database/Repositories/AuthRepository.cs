using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RedBadgeStarter.Database.DataContract.Authorization.Interfaces;
using RedBadgeStarter.Database.DataContract.Authorization.RAOs;
using RedBadgeStarter.Database.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RedBadgeStarter.Database.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly RBStarterContext _context;

        public AuthRepository(RBStarterContext context, IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _context = context;
            _config = config;
        }

        public Task<ReceivedExistingUserRAO> GetUserById(int ownerId)
        {
            throw new NotImplementedException();
        }

        public async Task<ReceivedExistingUserRAO> Login(QueryForExistingUserRAO queryRao)
        {
            var user = await _context.UserTableAccess.FirstOrDefaultAsync(x => x.UserName == queryRao.UserName);

            if (user == null) return null;

            if (!VerifyPasswordHash(queryRao.Password, user.PasswordHash, user.PasswordSalt)) return null;

            return _mapper.Map<ReceivedExistingUserRAO>(user);
        }

        public async Task<ReceivedExistingUserRAO> Register(RegisterUserRAO regUserRAO, string password)
        {
            var user = _mapper.Map<UserEntity>(regUserRAO);

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            Normalize(user, out string normalizedUserName, out string normalizedEmail);
            user.NormalizedUserName = normalizedUserName;
            user.NormalizedEmail = normalizedEmail;

            user.Role = Role.User;

            await _context.UserTableAccess.AddAsync(user);
            var result = await _context.SaveChangesAsync();

            if (result == 1)
            {
                return _mapper.Map<ReceivedExistingUserRAO>(user);
            }
            
            throw new NotImplementedException();
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.UserTableAccess.AnyAsync(x => x.NormalizedUserName == username.ToUpper())) return true;

            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private void Normalize(UserEntity user, out string normalizedUserName, out string normalizedEmail)
        {
            normalizedUserName = user.UserName.ToUpper();
            normalizedEmail = user.Email.ToUpper();
        }

        public async Task<string> GenerateTokenString(QueryForExistingUserRAO queryRAO)
        {
            var user = await _context.UserTableAccess.FirstOrDefaultAsync(x => x.UserName == queryRAO.UserName);

            if (user == null) return null;

            //TODO: Roles Stuff
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }
    }
}