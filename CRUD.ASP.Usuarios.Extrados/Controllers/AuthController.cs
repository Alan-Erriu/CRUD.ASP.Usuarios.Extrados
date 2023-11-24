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

        public AuthController(IHashService hashService, IAuthenticationService authorizationService)
        {

            _hashService = hashService;
            _authorizationService = authorizationService;
        }

        [HttpPost("auth")]

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
    }
}
