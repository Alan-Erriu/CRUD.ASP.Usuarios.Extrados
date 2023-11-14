using AccesData.DTOs;
using AccesData.Models;
using Dapper;
using Services.Interfaces;
using Services.Security;
using System.Data.SqlClient;

namespace AccesData.Data

{
    public class DataUser
    {
        private IHashService _hashService;
        private string _chainSQL { get; set; }
        public DataUser(string chainSQL, IHashService hashService)
        {
            _chainSQL = chainSQL;
            _hashService = hashService;
        }
        public async Task<CreateUserResponse> DCreateUser(CreateUserRequest createUserRequest)
        {


            try
            {

                using (var connection = new SqlConnection(_chainSQL))
                {
                    
                    createUserRequest.password_user = _hashService.HashPasswordUser(createUserRequest.password_user);
                    var parameters = new { Name = createUserRequest.name_user, Mail = createUserRequest.mail_user, Password = createUserRequest.password_user };
                    var sqlInsert = "INSERT INTO [user] (name_user, mail_user, password_user ) VALUES (@Name, @Mail, @Password)";
                    var sqlSelect = "select id_user from [user] where mail_user =@Mail and password_user =@Password";

                    var queryInsert = await connection.ExecuteAsync(sqlInsert, parameters);
                    var querySelect = await connection.QueryFirstOrDefaultAsync<int>(sqlSelect, new { Mail = createUserRequest.mail_user, Password = createUserRequest.password_user });


                    return new CreateUserResponse { id_user = querySelect, name_user = createUserRequest.name_user, mail_user = createUserRequest.mail_user, msg = "Ok", result = true };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error al intentar crear un nuevo usuario {ex.Message}");
                return new CreateUserResponse { result = false, msg = ex.Message };

            }
        }


        public async Task<GetUserByIdResponse> DGetUserByIdProtected(GetUserByIdRequest getUserByIdRequest)
        {

            try
            {
                using (var connection = new SqlConnection(_chainSQL))
                {
                    var parameters = new { Id = getUserByIdRequest.id_user };
                    var sql = "SELECT id_user, name_user, mail_user, password_user FROM [user] WHERE id_user = @Id";
                    var user = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
                    if (user == null) return new GetUserByIdResponse { msg = "User not found", result = false };
                   
                    if (!_hashService.VerifyPassword(getUserByIdRequest.password_user, user.password_user)) return new GetUserByIdResponse { msg = "Incorrect password", result = false };

                    return new GetUserByIdResponse { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, msg = "OK", result = true };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when trying to get a user by ID: {ex.Message}");
                return null;
            }
        }
        public async Task<GetUserByIdResponse> DGetUserById(int id_user)
        {
            User user = null;
            try
            {
                using (var connection = new SqlConnection(_chainSQL))
                {
                    var parameters = new { Id = id_user };
                    var sql = "SELECT id_user, name_user, mail_user FROM [user] WHERE id_user = @Id";
                    user = await connection.QueryFirstOrDefaultAsync<User>(sql, parameters);
                }
                if (user == null)

                    return new GetUserByIdResponse { msg = "User not found", result = false };


                return new GetUserByIdResponse { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, msg = "OK", result = true };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when trying to get a user by ID: {ex.Message}");
                return new GetUserByIdResponse { msg = ex.Message, result = false }; ;
            }
        }


        public async Task<int> DUpdateUserById(UpdateUserByIdRequest updateUserByIdRequest)
        {
            var rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(_chainSQL))
                {
                    var parameters = new { Name = updateUserByIdRequest.name_user, Id = updateUserByIdRequest.id_user };
                    var sqlEdit = "UPDATE [user] SET name_user = @Name WHERE id_user = @Id";

                    rowsAffected = await connection.ExecuteAsync(sqlEdit, parameters);


                }
                return rowsAffected;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when trying to update user by ID: {ex.Message}");
                return rowsAffected;
            }
        }
        public async Task<int> DDeleteUserById(int id)
        {
            var rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(_chainSQL))
                {

                    var sql = "delete from [user] WHERE id_user = @Id";

                    rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });

                    Console.WriteLine($"{rowsAffected} fila afectada");

                    return rowsAffected;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar borrar un usuario por ID: {ex.Message}");
                return rowsAffected;
            }
        }

    }
}
