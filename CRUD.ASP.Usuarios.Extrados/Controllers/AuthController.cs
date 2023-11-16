using AccesData.DTOs;
using AccesData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Net;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {

        private IHashService _hashService;

        private IAuthenticationService _authorizationService;

        public AuthController(IConfigSqlConnect dbConnection, IHashService hashService, IAuthenticationService authorizationService)
        {
           
            _hashService = hashService;
            _authorizationService = authorizationService;
        }

        [HttpPost("auth")]

        public async Task<IActionResult> Auth([FromBody] LoginRequestDTO authorizationRequest)
        {
            try
            {

                LoginDTO userData = await _authorizationService.ReturnToken(authorizationRequest);
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
