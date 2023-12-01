using AccesData.DTOs;
using AccesData.InputsRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {



        private IAuthenticationService _authorizationService;

        private IUserService _userService;

        public AuthController(IAuthenticationService authorizationService, IUserService userService)
        {


            _authorizationService = authorizationService;
            _userService = userService;


        }

        [HttpPost("signin")]
        //iniciar sesion
        public async Task<IActionResult> SignIn([FromBody] LoginRequest authorizationRequest)
        {
            try
            {

                LoginDTO userTokens = await _authorizationService.SignInService(authorizationRequest);
                if (userTokens.msg == "User Not Found") return NotFound("User Not Found");
                if (userTokens.msg == "Incorrect password") return BadRequest("Incorrect password");
                if (userTokens.msg == "error database") return StatusCode(500, "server error");

                var cookieOptions = new CookieOptions
                {
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(1200),
                    HttpOnly = true,
                    SameSite = SameSiteMode.None

                };
                Response.Cookies.Append("refreshToken", userTokens.refreshToken, cookieOptions);

                return Ok(userTokens);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al autenticar usuario {ex.Message}");
                return StatusCode(500, "server error");
            }

        }
        // registrarse como usuario - rol=(user)

        [HttpPost("signup")]

        public async Task<IActionResult> SignUp(CreateUserRequest createUserRequest)
        {

            if (!_userService.IsValidEmail(createUserRequest.mail_user)) return BadRequest("Invalid email format");
            try
            {

                CreateUserDTO user = await _authorizationService.SignUpService(createUserRequest);
                if (user.msg == "The email is already in use") return Conflict("The email is already in use");
                if (user.msg == "server error") return StatusCode(500, user.msg);
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new user {Ex.Message}");
                return StatusCode(500, "server error:");
            }


        }
        [Authorize]
        [HttpPost("logout")]

        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var cookieOptions = new CookieOptions
                {
                    Secure = true,
                    Expires = DateTime.Now,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None

                };
                //se borra el refresh token de las cockies
                Response.Cookies.Append("refreshToken", "", cookieOptions);

                var roswAffected = await _authorizationService.DeleteRefreshTokenExpiredFromBd(userId);
                return Ok("logout successful");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error logout{Ex.Message}");
                return StatusCode(500, "server error:");
            }


        }

        [HttpPost("getrefrehtoken")]
        public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest token)
        {
            try
            {
                //obtener refreshToken de la cokie 
                var Refreshtoken = Request.Cookies["refreshToken"];

                // extraer claims del token vencido
                var user = _authorizationService.GetUserFromExpiredToken(token.expiredToken);
                if (user == null) return BadRequest("invalid expired token");

                // validaciones, por si los campos estan vacios, los campos vienen del access token expirado
                if (user.id_user == 0) BadRequest("invalid access token ");
                if (string.IsNullOrEmpty(user.name_user) || string.IsNullOrEmpty(user.mail_user) || string.IsNullOrEmpty(user.role_user)) return BadRequest("invalid token");
                var refreshToken = Request.Cookies["refreshToken"];

                //validar que el Refreshtoken(el string) de la request sea el mismo que en la bd (en el campo refresh_toke_tokenhistory)
                bool resultCompareRefreshTokens = await _authorizationService.CompareRefreshTokens(user.id_user, Refreshtoken);
                if (!resultCompareRefreshTokens) return BadRequest("invalid refresh token");

                //chequear que el token refresh no este vencido y que este en la BD
                var refreshTokenIsActive = await _authorizationService.RefreshTokenIsActive(user.id_user);
                if (!refreshTokenIsActive) return BadRequest("refresh token expired");



                //elimar token de la bd que coincida con la fk id_user
                var roswAffected = await _authorizationService.DeleteRefreshTokenExpiredFromBd(user.id_user);

                var tokens = await _authorizationService.ReturnRefreshToken(token);
                //guardar el nuevo refresh token en la cokie

                var cookieOptions = new CookieOptions
                {
                    Secure = true,
                    Expires = DateTime.Now.AddMinutes(1200),
                    HttpOnly = true,
                    SameSite = SameSiteMode.None

                };
                Response.Cookies.Append("refreshToken", tokens.refreshToken, cookieOptions);

                if (tokens.msg == "ok") return Ok(tokens);
                return StatusCode(500, "Internal Server Error");

            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound("refresh token not foudn");
            }

            catch (Exception ex)
            {

                //CompareRefreshTokens() lanza este error cuando el token, no es un token o no tiene un formato jwt valido 
                if (ex.Message.Contains("IDX12741"))
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest("Invalid access token format");
                }

                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }


    }

}
