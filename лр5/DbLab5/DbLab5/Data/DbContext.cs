using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbLab5.Misc;
using DbLab5.Pages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace DbLab5.Data
{
    public class DbContext : IDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        
        public string WorkingTableName { get; set; } = string.Empty;
        
        public string DbName { get; }

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            DbName = _connectionString.GetDbName();
        }
        
        public async Task<bool> TableExistsAsync(string tableName)
        {
            return (await GetTableNamesAsync()).Contains(tableName.ToLower());
        }
        
        public async Task<SelectResult> SelectAllAsync(string tableName)
        {
            if (!await TableExistsAsync(tableName))
            {
                throw new ArgumentException(nameof(tableName));
            }
            
            await using var command = new MySqlCommand
            {
                CommandText = $"SELECT * FROM {tableName};"
            };
            
            return await ExecuteReaderAsync(command);
        }
        
        public async Task<SelectResult> SelectColumnsAsync(string tableName, List<string> columnNames)
        {
            await using var command = new MySqlCommand
            {
                CommandText = "SELECT @columns FROM @tableName;"
            };
            
            var columns = string.Join(", ", columnNames);

            command.Parameters.AddWithValue("@columns", columns);
            command.Parameters.AddWithValue("@tableName", tableName);

            return await ExecuteReaderAsync(command);
        }
        
        public async Task<int> InsertAsync(string tableName, List<string> values)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand("INSERT INTO @tableName VALUES (@valuesToInsert);", connection);
            
            var valuesToInsert = string.Join(", ", values);
            
            command.Parameters.AddWithValue("@tableName", tableName);
            command.Parameters.AddWithValue("@valuesToInsert", valuesToInsert);
            
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<SelectResult> ExecuteReaderAsync(string sqlCommandText)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(sqlCommandText, connection);

            await using var reader = await command.ExecuteReaderAsync();
            
            var selectResult = new SelectResult
            {
                Headers = Enumerable
                    .Range(0, reader.FieldCount)
                    .Select(reader.GetName)
                    .ToList()
            };

            while (await reader.ReadAsync())
            {
                var row = new List<string>(selectResult.Headers.Count);
                
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetValue(i).ToString() ?? string.Empty);
                }
                
                selectResult.Values.Add(row);
            }
                        
            return selectResult;
        }
        
        public async Task<SelectResult> ExecuteReaderAsync(MySqlCommand command)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            command.Connection = connection;
            
            await using var reader = await command.ExecuteReaderAsync();
            
            var selectResult = new SelectResult
            {
                Headers = Enumerable
                    .Range(0, reader.FieldCount)
                    .Select(reader.GetName)
                    .ToList()
            };

            while (await reader.ReadAsync())
            {
                var row = new List<string>(selectResult.Headers.Count);
                
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetValue(i).ToString() ?? string.Empty);
                }
                
                selectResult.Values.Add(row);
            }
                        
            return selectResult;
        }
        
        public async Task<int> ExecuteNonQueryAsync(string sqlCommandText)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new MySqlCommand(sqlCommandText, connection);
            
            return await command.ExecuteNonQueryAsync();
        }
        
        public async Task<object> ExecuteScalarAsync(string sqlCommandText)
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new MySqlCommand(sqlCommandText, connection);
            
            return await command.ExecuteScalarAsync();
        }
        
        public async Task<List<string>> GetTableNamesAsync()
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new MySqlCommand(
                "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = @dbName;", connection);
            
            command.Parameters.AddWithValue("@dbName", _connectionString.GetDbName());
            
            await using var reader = await command.ExecuteReaderAsync();

            var tableNames = new List<string>();
            
            while (await reader.ReadAsync())
            {
                tableNames.Add(reader.GetFieldValue<string>(0));
            }
                        
            return tableNames;
        }

        public async Task<List<TriggerModel>> GetTriggersAsync()
        {
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new MySqlCommand(
                "SELECT trigger_name, action_timing, event_manipulation, event_object_table, action_statement FROM information_schema.triggers WHERE trigger_schema = @dbName;", 
                connection);
            
            command.Parameters.AddWithValue("@dbName", _connectionString.GetDbName());
            
            await using var reader = await command.ExecuteReaderAsync();
            
            var triggers = new List<TriggerModel>();
            
            while (await reader.ReadAsync())
            {
                triggers.Add(new TriggerModel
                {
                    Name = reader.GetFieldValue<string>(0),
                    Time = reader.GetFieldValue<string>(1),
                    Event = reader.GetFieldValue<string>(2),
                    EventObjectTableName = reader.GetFieldValue<string>(3),
                    Statement = reader.GetFieldValue<string>(4)
                });
            }
            
            return triggers;
        }

        public async Task<int> RemoveTriggerAsync(string triggerName)
        {
            if (!await TriggerExists(triggerName))
            {
                throw new ArgumentException($"There is no trigger with name {triggerName}");
            }
            
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new MySqlCommand($"DROP TRIGGER {triggerName}", connection);

            return await command.ExecuteNonQueryAsync();
        }
        
        public async Task<bool> TriggerExists(string triggerName)
        {
            return (await GetTriggersAsync()).Select(t => t.Name).Contains(triggerName);
        }
        
        public async Task CreateTableAsync(string tableName, List<CreateTableRowModel> rowsModels)
        {
            if (await TableExistsAsync(tableName))
            {
                throw new ArgumentException($"Table with name {tableName} already exists");
            }
            
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            var rows = new StringBuilder();
            
            foreach (var row in rowsModels)
            {
                rows.Append($"{row.Name} {row.Type} {(row.NotNull ? "NOT NULL" : string.Empty)} {(row.AutoIncrement ? "AUTO_INCREMENT" : string.Empty)} {(row.PrimaryKey ? "PRIMARY KEY" : string.Empty)},");
            }
            rows.Remove(rows.Length - 1, 1);
            
            await using var command = new MySqlCommand($"CREATE TABLE {tableName}({rows});", connection);
            
            await command.ExecuteNonQueryAsync();
        }

        public async Task<List<FieldModel>> DescribeTableAsync(string tableName)
        {
            if (!await TableExistsAsync(tableName))
            {
                throw new ArgumentException(nameof(tableName));
            }
            
            await using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();
            
            await using var command = new MySqlCommand($"DESCRIBE {tableName}", connection);

            await using var reader = await command.ExecuteReaderAsync();
            
            var fields = new List<FieldModel>();
            
            while (await reader.ReadAsync())
            {
                fields.Add(new FieldModel
                {
                    Name = reader.GetFieldValue<string>(0),
                    Type = reader.GetFieldValue<string>(1),
                    Null = reader.GetFieldValue<string>(2),
                    Key = reader.GetFieldValue<string>(3),
                    Default = reader.GetValue(4) is DBNull ? string.Empty : reader.GetFieldValue<string>(4),
                    Extra = reader.GetFieldValue<string>(5)
                });
            }
            
            return fields;
        }
        
        public async Task RenameTableAsync(string tableName, string newName)
        {
            if (!await TableExistsAsync(tableName))
            {
                throw new ArgumentException(nameof(tableName));
            }
            
            if (await TableExistsAsync(newName))
            {
                throw new ArgumentException($"Table with name {newName} already exists");
            }
            
            await ExecuteNonQueryAsync($"ALTER TABLE {tableName} RENAME TO {newName};");
        }
        
        public async Task RemoveTableAsync(string tableName)
        {
            if (!await TableExistsAsync(tableName))
            {
                throw new ArgumentException(nameof(tableName));
            }
            
            await ExecuteNonQueryAsync($"DROP TABLE {tableName}");
        }
    }
}