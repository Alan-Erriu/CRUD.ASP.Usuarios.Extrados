using System.Data.SqlClient;

namespace ConnectionDB
{
    public class Connection : IDisposable
    {

        private string connectionString = @"Data Source=DESKTOP-KCGGJDV\SQLEXPRESS;Initial Catalog=user_extrados;Integrated Security=True;";

        private SqlConnection dbConnection;
       

        public Connection()
        {
            dbConnection = new SqlConnection(connectionString);
            
        }

        public void Dispose()
        {
            dbConnection.Dispose();
        }
    }
}


   


  