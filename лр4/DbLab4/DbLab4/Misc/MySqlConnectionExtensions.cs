using MySql.Data.MySqlClient;

namespace DbLab4.Misc
{
    public static class MySqlConnectionExtensions
    {
        public static bool TableExists(this MySqlConnection connection, string dbName, string tableName)
        {
            var cmd = new MySqlCommand($"SELECT COUNT(*) FROM information_schema.Tables WHERE table_schema = '{dbName}' AND table_name='{tableName}';", connection);
            var scalar = cmd.ExecuteScalar();
            
            if (scalar != null)
            {
                return (long)scalar != 0;   
            }
            
            return false;
        }
    }
}