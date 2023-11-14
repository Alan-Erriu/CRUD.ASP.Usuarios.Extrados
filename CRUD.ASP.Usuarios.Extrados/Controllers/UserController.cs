using AccesData.Data;
using AccesData.DTOs;
using AccesData.Interfaces;
using AccesData.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class UserController : ControllerBase
    {

        private IConfigSqlConnect _dbConnection;

        public UserController(IConfigSqlConnect dbConnection)
        {
            _dbConnection = dbConnection;
        }
        // crear usuario 
        [HttpPost("create")]

        public async Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            try
            {
                if (createUserRequest.name_user == null || createUserRequest.mail_user == null || createUserRequest.password_user == null) return StatusCode(400, "Incorrect request: name, mail and password is required");
                if (createUserRequest.name_user == "" || createUserRequest.mail_user == "" || createUserRequest.password_user == "") return StatusCode(400, "Incorrect request: name, mail and password is empty");
                DataUser dataUser = new DataUser(_dbConnection.chainSQL());
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
        [HttpPost("get")]
        public async Task<IActionResult> GetUserById([FromBody] GetUserByIdRequest getUserByIdRequest)
        {
            try
            {
                DataUser dataUser = new DataUser(_dbConnection.chainSQL());
                GetUserByIdResponse user = await dataUser.DGetUserByIdProtected(getUserByIdRequest);
                if (user.msg == "User not found") return StatusCode(404, user);
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error getting user {Ex.Message}");
                return StatusCode(500, "Server error:"  + Ex.Message);
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
                DataUser dataUser = new DataUser(_dbConnection.chainSQL());
               var userModifycated = await dataUser.DUpdateUserById(updateUserByIdRequest);
                if (userModifycated == 0) { return StatusCode(404, $"User not found {updateUserByIdRequest.id_user}");}
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
            try
            {
                DataUser dataUser = new DataUser(_dbConnection.chainSQL());
               var user =  await dataUser.DDeleteUserById(id_user);
              
                if (user == 0) { return StatusCode(404, $"User not found {id_user}");}

                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error deleting user {Ex.Message}");
                return StatusCode(500, "Server Error" + Ex.Message);
            }
        }




    }
}
