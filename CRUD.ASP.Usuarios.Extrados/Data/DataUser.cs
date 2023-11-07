using System.Data.Common;
using System.Data.SqlClient;
using System.Xml.Linq;
using ConnectionDB;
using CRUD.ASP.Usuarios.Extrados.Models;
using Dapper;

namespace CRUD.ASP.Usuarios.Extrados.Data

{
    public class DataUser
    {
        private string connectionString = @"Data Source=DESKTOP-KCGGJDV\SQLEXPRESS;Initial Catalog=user_extrados;Integrated Security=True;";
        public async Task<User> DCreateUser(string name, string mail, string password)
        {
                var user = new User();
            try
            {
                
                using (var connection = new SqlConnection(connectionString))
                {
                    var parameters = new { Name = name, Mail = mail, Password = password };
                    var sqlInsert = "INSERT INTO [user] (name_user, mail_user, password_user ) VALUES (@Name, @Mail, @Password)";
        
                    var rowsAffected = await connection.ExecuteAsync(sqlInsert, parameters);

                    Console.WriteLine($"{rowsAffected} fila afectada");
                    return new User { name_user = name, mail_user = mail };
                }
            }
            catch (Exception ex)
            {
               
                Console.WriteLine($"Error al intentar crear un nuevo usuario {ex.Message}");
                return user;

            }
        }


        public async Task<User> DGetUserById(int id)
        {
            User user = null;
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var parameters = new { Id = id };
                    var sql = "SELECT id_user, name_user, mail_user FROM [user] WHERE id_user = @Id";
                    user =  await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar obtener un usuario por ID: {ex.Message}");
                return null;
            }
        }


        public async Task DUpdateUserById(int id, string name)
        {
            
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var parameters = new { Name = name, Id = id };
                    var sqlEdit = "UPDATE [user] SET name_user = @Name WHERE id_user = @Id";

                    var rowsAffected = await connection.ExecuteAsync(sqlEdit, parameters);

                    Console.WriteLine($"{rowsAffected} fila afectada");
                }
             
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar actualizar  usuario por ID: {ex.Message}");
               
            }
        }
        public async Task DDeleteUserById(int id)
        {

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                   
                    var sql = "delete from [user] WHERE id_user = @Id";

                    var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

                    Console.WriteLine($"{rowsAffected} fila afectada");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar borrar un usuario por ID: {ex.Message}");

            }
        }

    }
}
