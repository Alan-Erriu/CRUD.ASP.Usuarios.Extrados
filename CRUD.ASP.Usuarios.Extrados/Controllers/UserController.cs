using CRUD.ASP.Usuarios.Extrados.Data;
using CRUD.ASP.Usuarios.Extrados.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CRUD.ASP.Usuarios.Extrados.Controllers
{
      [ApiController]
      [Route("[controller]")]
   
    
    public class UserController : ControllerBase
    {

        // crear usuario 
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(User newUser)
        {
            try
            {
                if (newUser.name_user == null || newUser.mail_user == null || newUser.password_user == null) { return StatusCode(400, "Solicitud incorrecta, completar todos los campos"); }
                DataUser dataUser = new DataUser();
                var user = await dataUser.DCreateUser(newUser.name_user, newUser.mail_user, newUser.password_user);
                return Ok(user);

            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error al crear un nuevo usuario {Ex.Message}");
                return StatusCode(500, "Error al crear un nuevo usuario" + Ex.Message);
            }
            
        
        }

        // obtener usuario por id
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserById(int id )
        {
            try
            {
                DataUser dataUser = new DataUser();
                User user = await dataUser.DGetUserById(id);
                if(user == null) { return StatusCode(404, $"usuario no encontrado id:{id}");}

                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error al obtener usuario {Ex.Message}");
                return StatusCode(500, "Error al obtener usuario" + Ex.Message);
            }
        }

        // actualizar usuario por id
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserById([FromBody] User userParameters )
        {
           if (userParameters.id_user == 0 || userParameters.name_user == null ) { return StatusCode(400, "Debe ingresar id y un nuevo nombre"); }
           try
            {
                DataUser dataUser = new DataUser();
              var user = await dataUser.DGetUserById(userParameters.id_user);
              if (user == null) { return StatusCode(404, $"usuario no encontrado {userParameters.id_user}"); }     
              await dataUser.DUpdateUserById(userParameters.id_user, userParameters.name_user);
              return Ok($"Usuario {userParameters.id_user}, Nombre modificado a {userParameters.name_user} ");
            }
            catch (Exception Ex)
            {
              Console.WriteLine($"Error al editar usuario {Ex.Message}");
              return StatusCode(500, "Error al editar usuario" + Ex.Message);
            }
        }

        // borrar usuario por id
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> deleteUserById( int id)
        {
            if (id == 0) { return StatusCode(400, "Debe ingresar un valido id");}
            try
            {
                DataUser dataUser = new DataUser();
                var user = await dataUser.DGetUserById(id);
                if (user == null) { return StatusCode(404, $"usuario no encontrado {id}"); } 

                await dataUser.DDeleteUserById(id);
                return Ok(user);
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error al borrar usuario {Ex.Message}");
                return StatusCode(500, "Error al borrar usuario" + Ex.Message);
            }
        }




    }
}
