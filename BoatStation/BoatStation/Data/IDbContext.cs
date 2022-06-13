using System.Collections.Generic;
using System.Threading.Tasks;
using BoatStation.Data.Models;
using Npgsql;

namespace BoatStation.Data
{
    public interface IDbContext
    {
        void ChangeUser(string username, string password);
        Task<bool> TableExistsAsync(string tableName);
        Task<SelectResult> SelectAllAsync(string tableName);
        Task<SelectResult> SelectColumnsAsync(string tableName, List<string> columnNames);
        Task<int> InsertAsync(string tableName, List<string> values);
        Task<SelectResult> ExecuteReaderAsync(string sqlCommandText);
        Task<SelectResult> ExecuteReaderAsync(NpgsqlCommand command);
        Task<int> ExecuteNonQueryAsync(string sqlCommandText);
        Task<object> ExecuteScalarAsync(string sqlCommandText);
        Task<List<string>> GetTableNamesAsync();
    }
}