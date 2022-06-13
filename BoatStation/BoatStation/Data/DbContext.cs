using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoatStation.Data.Models;
using BoatStation.Misc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace BoatStation.Data
{
    public class DbContext : IDbContext
    {
        private readonly IConfiguration _configuration;
        
        private readonly string _dbName = "boat_station";
        
        private string ConnectionString { get; set; }

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ChangeUser(string username, string password)
        {
            ConnectionString = ConnectionStringFactory.Create(_dbName, username, password);
        }
        
        public async Task<SelectResult> SelectAllAsync(string tableName)
        {
            if (!await TableExistsAsync(tableName))
            {
                throw new ArgumentException(nameof(tableName));
            }
            
            await using var command = new NpgsqlCommand()
            {
                CommandText = $"SELECT * FROM {tableName};"
            };
            
            return await ExecuteReaderAsync(command);
        }
        
        public async Task<SelectResult> SelectColumnsAsync(string tableName, List<string> columnNames)
        {
            await using var command = new NpgsqlCommand()
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
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand("INSERT INTO @tableName VALUES (@valuesToInsert);", connection);
            
            var valuesToInsert = string.Join(", ", values);
            
            command.Parameters.AddWithValue("@tableName", tableName);
            command.Parameters.AddWithValue("@valuesToInsert", valuesToInsert);
            
            return await command.ExecuteNonQueryAsync();
        }
        
        public async Task<SelectResult> ExecuteReaderAsync(string sqlCommandText)
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(sqlCommandText, connection);

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
        
        public async Task<SelectResult> ExecuteReaderAsync(NpgsqlCommand command)
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
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
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();
            
            await using var command = new NpgsqlCommand(sqlCommandText, connection);
            
            return await command.ExecuteNonQueryAsync();
        }
        
        public async Task<object> ExecuteScalarAsync(string sqlCommandText)
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();
            
            await using var command = new NpgsqlCommand(sqlCommandText, connection);
            
            return await command.ExecuteScalarAsync();
        }
        
        public async Task<bool> TableExistsAsync(string tableName)
        {
            return (await GetTableNamesAsync()).Contains(tableName.ToLower());
        }
        
        public async Task<List<string>> GetTableNamesAsync()
        {
            await using var connection = new NpgsqlConnection(ConnectionString);
            await connection.OpenAsync();

            await using var command = new NpgsqlCommand(
                "SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname = 'public' UNION SELECT viewname FROM pg_catalog.pg_views WHERE schemaname = 'public';", connection);

            await using var reader = await command.ExecuteReaderAsync();

            var tableNames = new List<string>();
            
            while (await reader.ReadAsync())
            {
                tableNames.Add(reader.GetFieldValue<string>(0));
            }
                        
            return tableNames;
        }
    }
}