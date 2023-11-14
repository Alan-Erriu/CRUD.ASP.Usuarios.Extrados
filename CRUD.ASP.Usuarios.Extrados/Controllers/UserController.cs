using AccesData.Data;
using AccesData.DTOs;
using AccesData.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Security;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class UserController : ControllerBase
    {

        private IConfigSqlConnect _dbConnection;

        private IHashService _hashService;

        public UserController(IConfigSqlConnect dbConnection, IHashService hashService)
        {
            _dbConnection = dbConnection;
            _hashService = hashService;
        }
        // crear usuario 
        [HttpPost("create")]

        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            if (string.IsNullOrEmpty(createUserRequest.name_user) || string.IsNullOrEmpty(createUserRequest.mail_user) || string.IsNullOrEmpty(createUserRequest.password_user))
                return BadRequest(new CreateUserResponse { msg = "Name, mail, and password are required", result = false });

            try
            {
                DataUser dataUser = new DataUser(_dbConnection.chainSQL(), _hashService);
                var user = await dataUser.DCreateUser(createUserRequest);
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error creating a new user {Ex.Message}");
                return StatusCode(500, "Server error:" + Ex.Message);
            }


        }

        // obtener usuario por id
        [HttpPost("getuser")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdRequest getUserByIdRequest)
        {
            if (getUserByIdRequest.id_user == 0 || string.IsNullOrEmpty(getUserByIdRequest.password_user))
                return BadRequest(new GetUserByIdResponse { msg = "id and password are required", result = false });

            try
            {
                DataUser dataUser = new DataUser(_dbConnection.chainSQL(), _hashService);
                GetUserByIdResponse user = await dataUser.DGetUserByIdProtected(getUserByIdRequest);
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
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserById([FromBody] UpdateUserByIdRequest updateUserByIdRequest)
        {
            if (updateUserByIdRequest.id_user == 0 || updateUserByIdRequest.name_user == "")
                return StatusCode(400, new GetUserByIdResponse { msg = "Name and id are required", result = false });
            try
            {
                DataUser dataUser = new DataUser(_dbConnection.chainSQL(), _hashService);
                var userModifycated = await dataUser.DUpdateUserById(updateUserByIdRequest);
                if (userModifycated == 0) { return StatusCode(404, $"User not found {updateUserByIdRequest.id_user}"); }
                return Ok($"User {updateUserByIdRequest.id_user}, Name modified to {updateUserByIdRequest.name_user} ");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error editing user {Ex.Message}");
                return StatusCode(500, "Server Error" + Ex.Message);
            }
        }

        // borrar usuario por id
        [HttpDelete("delete/{id_user}")]
        public async Task<IActionResult> DeleteUserById(int id_user)
        {
            if (id_user == 0) return BadRequest("id is required");
            try
            {
                DataUser dataUser = new DataUser(_dbConnection.chainSQL(), _hashService);
                var user = await dataUser.DDeleteUserById(id_user);

                if (user == 0) { return StatusCode(404, $"User not found {id_user}"); }

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
