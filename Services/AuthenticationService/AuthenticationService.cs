using AccesData.DTOs;
using AccesData.InputsRequest;
using AccesData.Interfaces;
using AccesData.Models;
using AccesData.Repositories;
using Configuration.JWTConfiguration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private IHashService _hashService;
        private readonly IAuthRepository _authRepository;
   
        private JWTConfig _jwtConfig;
        public AuthenticationService(IOptions<JWTConfig> jwtConfi, IAuthRepository authRepository, IHashService hashService)
        {
            _hashService = hashService;
            _authRepository = authRepository;
            _jwtConfig = jwtConfi.Value;
            
        }

        public string CreateToken(string id_user, string name_user, string mail_user, string role_user)
        {

            var key = _jwtConfig.Secret;
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, id_user));
            claims.AddClaim(new Claim(ClaimTypes.Name, name_user));
            claims.AddClaim(new Claim(ClaimTypes.Email, mail_user));
            claims.AddClaim(new Claim(ClaimTypes.Role, role_user));

            var credentialsToken = new SigningCredentials(
               new SymmetricSecurityKey(keyBytes),
               SecurityAlgorithms.HmacSha256Signature
               );
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(2),
                SigningCredentials = credentialsToken,
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            string createdToken = tokenHandler.WriteToken(tokenConfig);

            return createdToken;
        }
        public async Task<LoginDTO> SignInService(LoginRequest loginRequestDTO)
        {
            try
            {
                User user = await _authRepository.DataSignIn(loginRequestDTO);

                if (user == null) return new LoginDTO { msg = "User Not Found" };
                if (!_hashService.VerifyPassword(loginRequestDTO.password_user, user.password_user)) return new LoginDTO { msg = "Incorrect password" };

                var tokenCreated = CreateToken(user.id_user.ToString(), user.name_user, user.mail_user, user.role_user);
                return new LoginDTO { token = tokenCreated, msg = "Ok" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginDTO { msg = "Error during authentication", token = null };
            }

        }

    }
}
