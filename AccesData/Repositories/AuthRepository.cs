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

        }

    }

    


