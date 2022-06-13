using System;

namespace BoatStation.Misc
{
    public static class ConnectionStringFactory
    {
        public static string Create(string dbName, string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException();
            }
            
            return $"Server=localhost;Port=5432;Database={dbName};User Id={username};Password={password};";
        }
    }
}