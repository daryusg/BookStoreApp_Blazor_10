using Microsoft.Data.SqlClient;

namespace BookStoreApp.API.Static
{
    public static class Misc
    {
        public static string GetRedactedConnectionString(string connString) //cip...71 troubleshooting deployment connection issues
        {
            try
            {
                var connBuilder = new SqlConnectionStringBuilder(connString)
                {
                    UserID = "###",
                    Password = "###"
                };
                return connBuilder.ConnectionString;
            }
            catch
            {
                return $"Invalid connection string format: {connString}.";
            }
        }
    }
}
