using AccesData.DTOs;
using AccesData.InputsRequest;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private IHashService _hashService;

        private IAuthenticationService _authorizationService;

        private IUserService _userService;

        public AuthController(IHashService hashService, IAuthenticationService authorizationService, IUserService userService)
        {

            _hashService = hashService;
            _authorizationService = authorizationService;
            _userService = userService;
        }

        [HttpPost("signin")]

        public async Task<IActionResult> SignIn([FromBody] LoginRequest authorizationRequest)
        {
            try
            {

                LoginDTO userData = await _authorizationService.SignInService(authorizationRequest);
                if (userData.msg == "User Not Found") return NotFound("User Not Found");
                if (userData.msg == "Incorrect password") return BadRequest("Incorrect password");
                return Ok(userData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al autenticar usuario {ex.Message}");
                return StatusCode(500, "Error al autenticar usuario" + ex.Message);
            }

        }
        // registrarse como usuario

        [HttpPost("signup")]

        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            if (string.IsNullOrEmpty(createUserRequest.name_user) || string.IsNullOrEmpty(createUserRequest.mail_user) || string.IsNullOrEmpty(createUserRequest.password_user))
                return BadRequest("Name, mail, and password are required");
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
    }

}
