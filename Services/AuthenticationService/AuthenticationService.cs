using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccesData.DTOs;
using AccesData.Interfaces;
using AccesData.Models;
using AccesData.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using Services.Security;

namespace Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private IHashService _hashService;
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;
        public AuthenticationService( IConfiguration configuration, IAuthRepository authRepository, IHashService hashService)
        {
             _hashService = hashService;
             _authRepository = authRepository;
             _configuration = configuration;
        }

        public string CreateToken(string id_user)
        {

            var key = _configuration.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, id_user));

            var credentialsToken = new SigningCredentials(
               new SymmetricSecurityKey(keyBytes),
               SecurityAlgorithms.HmacSha256Signature
               );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = credentialsToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string createdToken = tokenHandler.WriteToken(tokenConfig);

            return createdToken;
        }
        public async Task<LoginDTO> ReturnToken(LoginRequestDTO LoginRequestDTO)
        {
            try
            {
              

                User user = await _authRepository.DataLogin(LoginRequestDTO);
              
                if (user == null) return new LoginDTO { msg = "User Not Found", result = false, };

                if (!_hashService.VerifyPassword(LoginRequestDTO.password_user, user.password_user)) return new LoginDTO { msg = "Incorrect password", result = false };

                var tokenCreated = CreateToken(user.id_user.ToString());
               
                return new LoginDTO { token = tokenCreated, result = true, msg = "Ok" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginDTO { msg = "Error during authentication", result = false, token = null };
            }

        }

    }
}
