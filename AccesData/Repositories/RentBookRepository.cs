using AccesData.DTOs.RentBookDTOs;
using AccesData.InputsRequest;
using AccesData.Interfaces;
using Configuration.BDConfiguration;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace AccesData.Repositories
{
    public class RentBookRepository : IRentBookRepository
    {
        private BDConfig _bdConfig;

        private string _sqlInsertRentBook = "INSERT INTO [rent_book] (id_book,id_user,rent_date_epoch,expiration_date_epoch) VALUES (@IDBOOK,@IDUSER,@RENTDATE,@EXPIRATIONDATE)";
        
      
        public RentBookRepository(IOptions<BDConfig> bdConfig)
        {
            _bdConfig = bdConfig.Value;
        }


        public async Task<CreateRentBookDTO> DataCreateRentBook(CreateRentBookRequest rentBookRequest)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { IDBOOK = rentBookRequest.id_book, IDUSER = rentBookRequest.id_user, RENTDATE = rentBookRequest.rent_date_epoch, EXPIRATIONDATE = rentBookRequest.expiration_date_epoch };
                    var queryInsert = await connection.ExecuteAsync(_sqlInsertRentBook, parameters);
              

                    return new CreateRentBookDTO
                    {
                        id_book = rentBookRequest.id_book,
                        id_user = rentBookRequest.id_user,
                        rent_date_epoch = rentBookRequest.rent_date_epoch,
                        expiration_date_epoch = rentBookRequest.expiration_date_epoch,
                        msg = "ok"
                    };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return new CreateRentBookDTO { msg = "error database" };
            }


        }



    }
}
