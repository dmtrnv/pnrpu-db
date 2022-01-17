using System.Collections.Generic;
using System.Threading.Tasks;
using DbLab5.Misc;
using MySql.Data.MySqlClient;

namespace DbLab5.Data
{
    public interface IDbContext
    {
        string WorkingTableName { get; set; }
        string DbName { get; }
        Task<bool> TableExistsAsync(string tableName);
        Task<SelectResult> SelectAllAsync(string tableName);
        Task<SelectResult> SelectColumnsAsync(string tableName, List<string> columnNames);
        Task<int> InsertAsync(string tableName, List<string> values);
        Task<SelectResult> ExecuteReaderAsync(string sqlCommandText);
        Task<SelectResult> ExecuteReaderAsync(MySqlCommand command);
        Task<int> ExecuteNonQueryAsync(string sqlCommandText);
        Task<object> ExecuteScalarAsync(string sqlCommandText);
        Task<List<string>> GetTableNamesAsync();
        Task<List<TriggerModel>> GetTriggersAsync();
        Task<int> RemoveTriggerAsync(string triggerName);
        Task<List<FieldModel>> DescribeTableAsync(string tableName);
        Task CreateTableAsync(string tableName, List<CreateTableRowModel> rowsModels);
        Task RenameTableAsync(string tableName, string newName);
        Task RemoveTableAsync(string tableName);
    }
}