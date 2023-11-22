using AccesData.DTOs;
using AccesData.Interfaces;
using AccesData.Models;
using Configuration.BDConfiguration;

using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Data.SqlClient;

namespace AccesData.Repositories
{
    public class UserRepository : IUserRepository
    {


        private BDConfig _bdConfig;

        private string _sqlInsertUser = "INSERT INTO [user] (name_user, mail_user, password_user ) VALUES (@Name, @Mail, @Password)";

        public string _sqlSelectUserID = "SELECT id_user from [user] where mail_user =@Mail and password_user =@Password";

        public string _sqlSelectUser = "SELECT id_user, name_user, mail_user, password_user FROM [user] WHERE id_user = @Id";

        public string _sqlEditUserName = "UPDATE [user] SET name_user = @Name WHERE id_user = @Id";

        public string _sqlDeleteUser = "delete from [user] WHERE id_user = @Id";

        public string _sqlSelectAllUsersMail = "SELECT mail_user FROM [user] where mail_user = @Mail";


        public UserRepository(IOptions<BDConfig> bdConfig)
        {
            _bdConfig = bdConfig.Value;

        }
        public async Task<User> DataCreateUser(CreateUserRequestDTO newUser)
        {
            using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
            {
                var parameters = new { Name = newUser.name_user, Mail = newUser.mail_user, Password = newUser.password_user };
                var queryInsert = await connection.ExecuteAsync(_sqlInsertUser, parameters);
                var querySelect = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectUserID, new { Mail = newUser.mail_user, Password = newUser.password_user });
                return new User { id_user = querySelect, name_user = newUser.name_user, mail_user = newUser.mail_user };
            }


        }

        public async Task<User> DataGetUserByID(int id_user)
        {
            using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
            {
                var parameters = new { Id = id_user };
                var user = await connection.QueryFirstOrDefaultAsync<User>(_sqlSelectUser, parameters);
                if (user == null) return null;
                return new User { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, password_user = user.password_user };
            }


        } 
        public async Task<User> DataCompareEmailUserByMail( string mail_user)
        {
            try
            {

            using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
            {
                var parameters = new { Mail = mail_user };
                var mailFound = await connection.QueryFirstOrDefaultAsync<User>(_sqlSelectAllUsersMail, parameters);
                    return mailFound;
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("error getting user mail");
            }


        }

        public async Task<int> DataUpdateUserById(UpdateUserRequestDTO updateUserRequestDTO)
        {
            var rowsAffected = 0;

            using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
            {
                var parameters = new { Name = updateUserRequestDTO.name_user, Id = updateUserRequestDTO.id_user };


                rowsAffected = await connection.ExecuteAsync(_sqlEditUserName, parameters);


            }
            return rowsAffected;


        }

        public async Task<int> DataDeleteUserById(int id)
        {
            var rowsAffected = 0;
            using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
            {
                rowsAffected = await connection.ExecuteAsync(_sqlDeleteUser, new { Id = id });

                Console.WriteLine($"{rowsAffected} fila afectada");

                return rowsAffected;
            }
        }
    }

}
