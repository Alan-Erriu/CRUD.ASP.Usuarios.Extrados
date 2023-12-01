using AccesData.DTOs.BookDTOs;
using AccesData.Interfaces;
using Configuration.BDConfiguration;
using Dapper;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace AccesData.Repositories
{
    public class BookRepository : IBookRepository
    {
        private BDConfig _bdConfig;

        private string _sqlInsertBook = "INSERT INTO [book] (name_book ) VALUES (@Name)";

        public string _sqlSelectBookId = "SELECT id_book from [book] where name_book =@Name";

        private string _sqlSelectAllNameBook = "SELECT name_book FROM [book] where name_book = @Name";

        private string _sqlDeleteBook = "delete from [book] where name_book = @Name";
        public BookRepository(IOptions<BDConfig> bdConfig)
        {
            _bdConfig = bdConfig.Value;
        }


        public async Task<CreateBookDTO> DataCreateBook(string nameBook)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { Name = nameBook };
                    var queryInsert = await connection.ExecuteAsync(_sqlInsertBook, parameters);
                    var querySelect = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectBookId, parameters);

                    return new CreateBookDTO { id_book = querySelect, name_book = nameBook, msg = "ok" };
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return new CreateBookDTO { msg = "error database" };
            }


        }
        public async Task<int> DataDeleteBookByName(string nameBook)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { Name = nameBook };
                    var queryDelete = await connection.ExecuteAsync(_sqlDeleteBook, parameters);


                    return queryDelete;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return 0;
            }


        }

        public async Task<int> DataGetIdBook(string nameBook)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { Name = nameBook };
                    var querySelect = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectBookId, parameters);

                    return querySelect;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"error database: {ex.Message}");
                return 0;
            }


        }


        public async Task<string> DataCompareNameBook(string NameBook)
        {
            try
            {

                using (var connection = new SqlConnection(_bdConfig.ConnectionStrings))
                {
                    var parameters = new { Name = NameBook };
                    var nameBookFound = await connection.QueryFirstOrDefaultAsync<string>(_sqlSelectAllNameBook, parameters);
                    return nameBookFound;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DataCompareNameBook errror: {ex.Message}");
                throw new Exception("error getting name book");
            }


        }

    }
}
