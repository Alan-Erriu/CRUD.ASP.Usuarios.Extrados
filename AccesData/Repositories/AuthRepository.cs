
using AccesData.DTOs;
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


        
        private string _sqlSelectAuthUser = "SELECT id_user, name_user, mail_user, password_user FROM [user] WHERE mail_user = @Mail";

        public AuthRepository(IOptions<BDConfig> bdConfig)
        {
            _bdConfig = bdConfig.Value;

        }

        public async Task<User> DataLogin(LoginRequestDTO loginRequestDTO)
        {
            try
            {

            using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
            {
                var parameters = new { Mail = loginRequestDTO.mail_user};
                var user = await connection.QueryFirstOrDefaultAsync<User>(_sqlSelectAuthUser, parameters);
                    if (user == null) return null;
                    
                    return new User { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, password_user = user.password_user };
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error al " + ex.Message);
                Console.WriteLine("cande de conexion: " + _bdConfig.ConnectionStrings);
                return null;
              
            }


        }
    }
}
