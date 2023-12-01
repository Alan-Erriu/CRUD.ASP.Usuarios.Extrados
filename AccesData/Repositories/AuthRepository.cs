using AccesData.DTOs;
using AccesData.DTOs.AuthDTOs;
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

        private string _sqlInsertRefreshToken = "INSERT INTO [token_history] (iduser_tokenhistory,token_tokenhistory,refresh_Token_tokenhistory," +
            " expiration_date_tokenhistory) VALUES (@IdUser,@Token,@RefreshToken,@ExpirationDate)";

        private string _SqlSelectRefreshToken = "SELECT * FROM [token_history] WHERE iduser_tokenhistory = @Id";

        private string _sqlDeleteRefreshTokenExpired = "delete from [token_history] where iduser_tokenhistory = @Id";





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
                    await connection.ExecuteAsync(_sqlInsertUser, parameters);
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


        public async Task<LoginDTO> DataInsertRefreshToken(TokenHistory tokenRequest)
        {
            try
            {
                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { IdUser = tokenRequest.iduser_tokenhistory, Token = tokenRequest.token_tokenhistory, RefreshToken = tokenRequest.refreshToken_tokenhistory, ExpirationDate = tokenRequest.expiration_date_tokenhistory };
                    await connection.ExecuteAsync(_sqlInsertRefreshToken, parameters);
                    return new LoginDTO { token = tokenRequest.token_tokenhistory, refreshToken = tokenRequest.refreshToken_tokenhistory, msg = "ok" };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return new LoginDTO { msg = "error database" };
            }

        }

        //devuelve un objeto con todos los campos de la tabla token_history 
        public async Task<RefreshTokenDTO> DataSelectRefreshToken(int id_user)
        {
            try
            {
                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {

                    var parameters = new { Id = id_user };
                    RefreshTokenDTO refreshToken = await connection.QueryFirstOrDefaultAsync<RefreshTokenDTO>(_SqlSelectRefreshToken, parameters);
                    if (refreshToken == null) return new RefreshTokenDTO { msg = "token not found" };
                    refreshToken.msg = "ok";

                    return refreshToken;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return new RefreshTokenDTO { msg = "error database" };
            }

        }

        public async Task<int> DataDeleteRefreshTokenExpired(int id_user)
        {


            try
            {
                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { @Id = id_user };
                    var rowsAffected = await connection.ExecuteAsync(_sqlDeleteRefreshTokenExpired, parameters);

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }

        }

    }

}






