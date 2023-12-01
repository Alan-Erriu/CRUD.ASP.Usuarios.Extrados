using AccesData.DTOs;
using AccesData.DTOs.AuthDTOs;
using AccesData.InputsRequest;
using AccesData.Interfaces;
using AccesData.Models;
using Configuration.JWTConfiguration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private IHashService _hashService;
        private readonly IAuthRepository _authRepository;
        private IUserRepository _userRepository;

        private JWTConfig _jwtConfig;
        public AuthenticationService(IOptions<JWTConfig> jwtConfi, IAuthRepository authRepository, IHashService hashService, IUserRepository userRepository)
        {
            _hashService = hashService;
            _authRepository = authRepository;
            _jwtConfig = jwtConfi.Value;
            _userRepository = userRepository;


        }

        //iniciar sesion-------------------
        public async Task<LoginDTO> SignInService(LoginRequest loginRequestDTO)
        {


            try
            {
                User user = await _authRepository.DataSignIn(loginRequestDTO);

                if (user == null) return new LoginDTO { msg = "User Not Found" };
                if (!_hashService.VerifyPassword(loginRequestDTO.password_user, user.password_user)) return new LoginDTO { msg = "Incorrect password" };

                var tokenCreated = CreateToken(user.id_user.ToString(), user.name_user, user.mail_user, user.role_user);
                string refreshToken = CreateRefreshToken();
                //antes de guardar el reresh token en la bd, siempre se borra el anterior, de forma que no se repita mas de un registro por usuaior en la bd
                await DeleteRefreshTokenExpiredFromBd(user.id_user);
                var credencialUser = await SaveHistoryRefreshToken(user.id_user, tokenCreated, refreshToken);

                return credencialUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginDTO { msg = "Error during authentication", token = null };
            }

        }
        //registrarse, el rol por defecto en la BD es "Usuario"
        public async Task<CreateUserDTO> SignUpService(CreateUserRequest createUserRequest)
        {
            try
            {
                var emailAlreadyExists = await _userRepository.DataCompareEmailUserByMail(createUserRequest.mail_user);

                if (emailAlreadyExists != null) return new CreateUserDTO { msg = "The email is already in use" };
                createUserRequest.password_user = _hashService.HashPasswordUser(createUserRequest.password_user);
                CreateUserDTO newUser = await _authRepository.DataSignUp(createUserRequest);
                if (newUser.msg == "error database") return new CreateUserDTO { msg = "server error" };
                return new CreateUserDTO { id_user = newUser.id_user, name_user = newUser.name_user, mail_user = newUser.mail_user, msg = "Ok" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error {ex.Message}");
                return new CreateUserDTO { msg = "server error" };
            }
        }



        //funciones JWT----------------------------------------------------------

        // crear refresh token-------------
        private string CreateRefreshToken()
        {
            var byteArray = new byte[64];
            string refreshToken = "";
            using (var mg = RandomNumberGenerator.Create())
            {
                mg.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }

        //crear token jwt
        private string CreateToken(string id_user, string name_user, string mail_user, string role_user)
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

        //insertar en la tabla tokenHistory, el refresh token, el id_user y el Accesstoken
        private async Task<LoginDTO> SaveHistoryRefreshToken(int id_user, string token, string refreshToken)
        {
            var tokenHistory = new TokenHistory
            {
                iduser_tokenhistory = id_user,
                token_tokenhistory = token,
                refreshToken_tokenhistory = refreshToken,
                expiration_date_tokenhistory = DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeMilliseconds()
            };
            LoginDTO tokenSaved = await _authRepository.DataInsertRefreshToken(tokenHistory);
            return tokenSaved;
        }

        //recibe un access token y retorna un nuevo access token y un refresh token
        public async Task<LoginDTO> ReturnRefreshToken(RefreshTokenRequest tokenRequest)
        {
            try
            {
                var user = GetUserFromExpiredToken(tokenRequest.expiredToken);
                var refreshTokenCreated = CreateRefreshToken();
                //crear un nuevo token de acceso a partir de los claims del token vencido
                var tokenCreated = CreateToken(user.id_user.ToString(), user.name_user, user.mail_user, user.role_user);
                await SaveHistoryRefreshToken(user.id_user, tokenCreated, refreshTokenCreated);
                return new LoginDTO { token = tokenCreated, refreshToken = refreshTokenCreated, msg = "ok" };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new LoginDTO { msg = "server error" };
            }

        }

        public async Task<bool> CompareRefreshTokens(int id_user, string refreshToken)
        {

            var refreshTokenBd = await _authRepository.DataSelectRefreshToken(id_user);
            Console.WriteLine("refresh token bd" + refreshTokenBd.refresh_Token_tokenhistory);
            if (refreshTokenBd.refresh_Token_tokenhistory != refreshToken) return false;
            return true;
        }

        public async Task<int> DeleteRefreshTokenExpiredFromBd(int id_user)
        {
            var rowsAffected = await _authRepository.DataDeleteRefreshTokenExpired(id_user);
            return rowsAffected;
        }


        //obtener el refresh token de la bd y checkear que no este vencido
        public async Task<bool> RefreshTokenIsActive(int id_user)
        {

            RefreshTokenDTO refreshTokenBd = await _authRepository.DataSelectRefreshToken(id_user);
            long currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            if (refreshTokenBd.expiration_date_tokenhistory < currentEpochTime) return false;
            return true;
        }




        // obtener todos los datos del usuario por medio de los claims del token
        public CreateUserWithRoleDTO GetUserFromExpiredToken(string token)
        {
            var key = _jwtConfig.Secret;
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            var user = new CreateUserWithRoleDTO
            {
                id_user = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                name_user = principal.FindFirst(ClaimTypes.Name)?.Value,
                mail_user = principal.FindFirst(ClaimTypes.Email)?.Value,
                role_user = principal.FindFirst(ClaimTypes.Role)?.Value
            };


            return user;
        }


    }
}

