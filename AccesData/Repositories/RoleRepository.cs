using AccesData.DTOs.RoleDTOs;
using AccesData.InputsRequest;
using AccesData.Interfaces;
using Configuration.BDConfiguration;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace AccesData.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private BDConfig _bdConfig;

        private string _sqlInsertRole = "INSERT INTO [role] (name_role, description_role ) VALUES (@Name, @Description)";

        private string _sqlSelectAllNameRole = "SELECT name_role FROM [role] where name_role = @Name";


        public RoleRepository(IOptions<BDConfig> bdConfig)
        {
            _bdConfig = bdConfig.Value;
        }

        public async Task<CreateRoleDTO> DataCreateRole(CreateRoleRequest roleRequest)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { Name = roleRequest.name_role, Description = roleRequest.description_role };
                    var queryInsert = await connection.ExecuteAsync(_sqlInsertRole, parameters);


                    return new CreateRoleDTO
                    {
                        name_role = roleRequest.name_role,
                        description_role = roleRequest.description_role,
                        msg = "ok"
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return new CreateRoleDTO { msg = "error database" };
            }
        }


        public async Task<string> DataCompareNameRole(string nameRole)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { Name = nameRole };
                    var nameRoleFound = await connection.QueryFirstOrDefaultAsync<string>(_sqlSelectAllNameRole, parameters);
                    return nameRoleFound;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DataCompareNameRole errror: {ex.Message}");
                throw new Exception("error getting name role");
            }


        }
    }

}
