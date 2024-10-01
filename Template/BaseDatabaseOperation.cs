using System.Data.SqlClient;

namespace Client.Template
{
    public abstract class BaseDatabaseOperation
    {
        protected readonly string ConnectionString = @"Server=DESKTOP-2J0EALH\SQLEXPRESS;Database=Synex_Outlet_Store;Trusted_Connection=True;";

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}