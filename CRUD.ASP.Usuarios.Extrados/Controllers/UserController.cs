using AccesData.DTOs;
using AccesData.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [ApiController]
    [Route("[controller]")]
 

    public class UserController : ControllerBase
    {

        private IUserService _userService;
       
        private IHashService _hashService;

        public UserController( IHashService hashService, IUserService userService)
        {           
            _hashService = hashService;
            _userService = userService;
        }
        // crear usuario
        [Authorize]
        [HttpPost("create")]

        public async Task<IActionResult> CreateUser(CreateUserRequestDTO createUserRequest)
        {
            if (string.IsNullOrEmpty(createUserRequest.name_user) || string.IsNullOrEmpty(createUserRequest.mail_user) || string.IsNullOrEmpty(createUserRequest.password_user))
                return BadRequest(new CreateUserDTO { msg = "Name, mail, and password are required", result = false });
            if (!_userService.IsValidEmail(createUserRequest.mail_user)) return BadRequest(new CreateUserDTO { msg = "Invalid email format", result = false });
            try
            {
                
                CreateUserDTO user = await _userService.CreateUserService(createUserRequest);
                if (user.msg == "The email is already in use") return Conflict(user);
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new user {Ex.Message}");
                return StatusCode(500, "Server error:" + Ex.Message);
            }


        }

        // obtener usuario por id
        [Authorize]
        [HttpPost("getuser")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdRequestDTO getUserByIdRequestDTO)
        {
            if (getUserByIdRequestDTO.id_user == 0 || string.IsNullOrEmpty(getUserByIdRequestDTO.password_user))
                return BadRequest(new GetUserByIdDTO { msg = "id and password are required", result = false });

            try
            {             
                GetUserByIdDTO user = await _userService.GetUserByIdProtectedService(getUserByIdRequestDTO);
                if (user.msg == "User not found") return NotFound(user);
                if (user.msg == "Incorrect password") return BadRequest(user);
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error getting user {Ex.Message}");
                return StatusCode(500, "Server error:" + Ex.Message);
            }
        }

        // actualizar usuario por id
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserById([FromBody] UpdateUserRequestDTO updateUserRequestDTO)
        {
            if (updateUserRequestDTO.id_user == 0 || string.IsNullOrEmpty(updateUserRequestDTO.name_user))
                return StatusCode(400, new GetUserByIdDTO { msg = "Name and id are required", result = false });
            try
            {
             
                var userModifycated = await _userService.UpdateUserByIdService(updateUserRequestDTO);
                if (userModifycated == 0) { return StatusCode(404, $"User not found id: {updateUserRequestDTO.id_user}"); }
                GetUserByIdDTO user = await _userService.GetUserByIdService(updateUserRequestDTO.id_user);
                
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error editing user {Ex.Message}");
                return StatusCode(500, "Server Error" + Ex.Message);
            }
        }

        // borrar usuario por id
        [Authorize]
        [HttpDelete("delete/{id_user}")]
        public async Task<IActionResult> DeleteUserById(int id_user)
        {
            if (id_user == 0) return BadRequest("id is required");
            try
            {
                
                var user = await _userService.DeleteUserByIdService(id_user);

                if (user == 0) { return StatusCode(404, $"User not found id: {id_user}"); }

                return Ok("user deleted");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error deleting user {Ex.Message}");
                return StatusCode(500, "Server Error" + Ex.Message);
            }
        }
  }

}

