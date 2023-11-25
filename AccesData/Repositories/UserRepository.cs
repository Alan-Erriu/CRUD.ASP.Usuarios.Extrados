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
    public class UserRepository : IUserRepository
    {


        private BDConfig _bdConfig;

        private string _sqlInsertUser = "INSERT INTO [user] (name_user, mail_user, password_user ) VALUES (@Name, @Mail, @Password)";

        private string _sqlInsertUserWithRole = "INSERT INTO [user] (name_user, mail_user, password_user,role_user ) VALUES (@Name, @Mail, @Password,@Role)";

        private string _sqlSelectUserID = "SELECT id_user from [user] where mail_user =@Mail and password_user =@Password";

        private string _sqlSelectUser = "SELECT id_user, name_user, mail_user, password_user FROM [user] WHERE id_user = @Id";

        private string _sqlEditUserName = "UPDATE [user] SET name_user = @Name WHERE id_user = @Id";

        private string _sqlDeleteUser = "delete from [user] WHERE id_user = @Id";

        private string _sqlSelectAllUsersMail = "SELECT mail_user FROM [user] where mail_user = @Mail";


        public UserRepository(IOptions<BDConfig> bdConfig)
        {
            _bdConfig = bdConfig.Value;

        }




        //crear un nuevo usuario con roles, los roles solo pueden coincidir con los registrados en la tabla "role"
        //Solo el usuario "Admin" va a tener acceso a este metodo
        public async Task<CreateUserWithRoleDTO> DataCreateUserWithRole(CreateUserWithRoleRequest newUser)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {

                    var parameters = new { Name = newUser.name_user, Mail = newUser.mail_user, Password = newUser.password_user, @Role = newUser.role_user };
                    var queryInsert = await connection.ExecuteAsync(_sqlInsertUserWithRole, parameters);
                    var querySelect = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectUserID, new { Mail = newUser.mail_user, Password = newUser.password_user });
                    return new CreateUserWithRoleDTO { id_user = querySelect, name_user = newUser.name_user, mail_user = newUser.mail_user, role_user = newUser.role_user, msg = "ok" };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return new CreateUserWithRoleDTO { msg = "error database" };
            }


        }

        public async Task<User> DataGetUserByID(int id_user)
        {
            try
            {
                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { Id = id_user };
                    var user = await connection.QueryFirstOrDefaultAsync<User>(_sqlSelectUser, parameters);
                    if (user == null) return null;
                    return new User { id_user = user.id_user, name_user = user.name_user, mail_user = user.mail_user, password_user = user.password_user };
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"error database: {ex.Message}");
                return null;

            }


        }
        public async Task<User> DataCompareEmailUserByMail(string mail_user)
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

        public async Task<int> DataUpdateUserById(UpdateUserRequest updateUserRequestDTO)
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
