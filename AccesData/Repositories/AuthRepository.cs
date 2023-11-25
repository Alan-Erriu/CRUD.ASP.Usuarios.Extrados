using AccesData.DTOs;
using AccesData.InputsRequest;
using AccesData.Interfaces;
using AccesData.Models;
using Configuration.BDConfiguration;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace AccesData.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private BDConfig _bdConfig;

        private string _sqlInsertUser = "INSERT INTO [user] (name_user, mail_user, password_user ) VALUES (@Name, @Mail, @Password)";

        private string _sqlSelectUserID = "SELECT id_user from [user] where mail_user =@Mail and password_user =@Password";

        private string _sqlSelectAuthUser = "SELECT id_user, name_user, mail_user, password_user,role_user FROM [user] WHERE mail_user = @Mail";

        public AuthRepository(IOptions<BDConfig> bdConfig)
        {
            _bdConfig = bdConfig.Value;

        }

        public async Task<User> DataSignIn(LoginRequest loginRequestDTO)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { Mail = loginRequestDTO.mail_user };
                    var user = await connection.QueryFirstOrDefaultAsync<User>(_sqlSelectAuthUser, parameters);
                    if (user == null) return null;

                    return user;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error data base: " + ex.Message);

                return null;

            }


        }
        // registrarse como usuario, el rol obtenido es "User", esta por defecto en la bd
        public async Task<CreateUserDTO> DataSignUp(CreateUserRequest newUser)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {

                    var parameters = new { Name = newUser.name_user, Mail = newUser.mail_user, Password = newUser.password_user };
                    var queryInsert = await connection.ExecuteAsync(_sqlInsertUser, parameters);
                    var querySelect = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectUserID, new { Mail = newUser.mail_user, Password = newUser.password_user });
                    return new CreateUserDTO { id_user = querySelect, name_user = newUser.name_user, mail_user = newUser.mail_user, msg = "ok" };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return new CreateUserDTO { msg = "error database" };
            }


        }

    }

}




