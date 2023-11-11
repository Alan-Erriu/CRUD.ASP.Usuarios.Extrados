using AccesData.Models;
using Dapper;
using System.Data.SqlClient;

namespace AccesData.Data

{
    public class DataUser
    {


        private string _chainSQL { get; set; }
        public DataUser(string chainSQL)
        {
            _chainSQL = chainSQL;
        }
        public async Task<User> DCreateUser(string name, string mail, string password)
        {
            var user = new User();
            try
            {

                using (var connection = new SqlConnection(_chainSQL))
                {
                    var parameters = new { Name = name, Mail = mail, Password = password };
                    var sqlInsert = "INSERT INTO [user] (name_user, mail_user, password_user ) VALUES (@Name, @Mail, @Password)";
                    var sqlSelect = "select id_user from [user] where mail_user =@Mail and password_user =@Password";

                    var queryInsert = await connection.ExecuteAsync(sqlInsert, parameters);
                    var querySelect = await connection.QueryFirstOrDefaultAsync<int>(sqlSelect, new { Mail = mail, Password = password });


                    return new User { id_user = querySelect, name_user = name, mail_user = mail };
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
                using (var connection = new SqlConnection(_chainSQL))
                {
                    var parameters = new { Id = id };
                    var sql = "SELECT id_user, name_user, mail_user FROM [user] WHERE id_user = @Id";
                    user = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
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
                using (var connection = new SqlConnection(_chainSQL))
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
                using (var connection = new SqlConnection(_chainSQL))
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
