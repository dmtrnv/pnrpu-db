using System;
using System.Collections.Generic;
using System.Data;
using DbLab4.Misc;
using DbLab4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace DbLab4.Pages
{
    public class Index : PageModel
    {
        private readonly IConfiguration _configuration;
        
        public bool ProductsTableExists = false;
        public bool ManufacturerTableExists = false;
        
        public List<Product> Products { get; set; } = new();
        public List<Manufacturer> Manufacturers { get; set; } = new();
        public List<SelectInnerJoin> SelectInnerJoinResults { get; set; } = new();
        
        public List<string> NotNullProductPropertiesNames { get; set; } = new();
        public List<string> NotNullManufacturerPropertiesNames { get; set; } = new();

        public Index(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void OnGet()
        {
            
        }
        
        public void OnGetCommandExecute([FromQuery] string commandName)
        {
            if (commandName != null && SqlCommands.CommandsNonQuery.TryGetValue(commandName, out var commandText))
            {
                // Получение строки подключения к базе данных
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Создание подключения
                using var connection = new MySqlConnection(connectionString);

                // Открытие подключения
                connection.Open();
                
                // Создание команды
                using var cmd = new MySqlCommand(commandText, connection);
                
                // Выполнение команды
                cmd.ExecuteNonQuery();
            }
            
            ProductsTableExists = false;
            ManufacturerTableExists = false;
        }
        
        public PartialViewResult OnGetSelectInnerJoin([FromQuery] string commandName)
        {
            if (commandName != null && SqlCommands.CommandsReader.TryGetValue(commandName, out var commandText))
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                using var connection = new MySqlConnection(connectionString);
                connection.Open();
                
                using var cmd = new MySqlCommand(commandText, connection);
                using var reader = cmd.ExecuteReader();
                
                while (reader.Read())
                {
                    var selectInnerJoinResult = new SelectInnerJoin
                    {
                        ProductName = reader.HasColumn("ProductName") ? reader.GetFieldValue<string>("ProductName") : null,
                        Cost = reader.HasColumn("Cost") ? reader.GetFieldValue<decimal>("Cost") : null,
                        Count = reader.HasColumn("Count") ? reader.GetFieldValue<int>("Count") : null,
                        ManufacturerName = reader.HasColumn("ManufacturerName") ? reader.GetFieldValue<string>("ManufacturerName") : null,
                        ManufacturerCity = reader.HasColumn("ManufacturerCity") ? reader.GetFieldValue<string>("ManufacturerCity") : null
                    };
                    
                    SelectInnerJoinResults.Add(selectInnerJoinResult);
                }
            }
            
            return Partial("_InnerJoinResultPartial", this);
        }
        
        public PartialViewResult OnGetQueryResultPartial()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var c = connectionString.LastIndexOf("database=", StringComparison.InvariantCultureIgnoreCase);
            
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                
                UpdateProductsProperty(connection);
                UpdateManufacturersProperty(connection);
            }

            if (Products.Count != 0)
            {
                NotNullProductPropertiesNames = GetNotNullPropertiesNames(Products[0]);
            }
            
            if (Manufacturers.Count != 0)
            {
                NotNullManufacturerPropertiesNames = GetNotNullPropertiesNames(Manufacturers[0]);
            }
            
            return Partial("_QueryResultPartial", this);
        }

        private void UpdateProductsProperty(MySqlConnection connection)
        {
            var dbName = _configuration.GetValue<string>("Database");
            
            if (connection.TableExists(dbName, "Products"))
            {
                ProductsTableExists = true;
                
                using var getProductsCmd = new MySqlCommand(SqlCommands.CommandsReader["SelectAllProducts"], connection);
                using var productReader = getProductsCmd.ExecuteReader();
            
                while (productReader.Read())
                {
                    var product = new Product
                    {
                        Id = productReader.HasColumn("Id") ? productReader.GetFieldValue<int>("Id") : null,
                        Name = productReader.HasColumn("Name") ? productReader.GetFieldValue<string>("Name") : null,
                        Cost = productReader.HasColumn("Cost") ? productReader.GetFieldValue<decimal>("Cost") : null,
                        Count = productReader.HasColumn("Count") ? productReader.GetFieldValue<int>("Count") : null,
                        ManufacturerId = productReader.HasColumn("ManufacturerId") ? productReader.GetFieldValue<int>("ManufacturerId") : null
                    };
                        
                    Products.Add(product);
                }
            }
        }
        
        private void UpdateManufacturersProperty(MySqlConnection connection)
        {
            var dbName = _configuration.GetValue<string>("Database");
            
            if (connection.TableExists(dbName, "Manufacturers"))
            {
                ManufacturerTableExists = true;
                
                using var getManufacturersCmd = new MySqlCommand(SqlCommands.CommandsReader["SelectAllManufacturers"], connection);
                using var manufacturerReader = getManufacturersCmd.ExecuteReader();
                    
                while (manufacturerReader.Read())
                {
                    var manufacturer = new Manufacturer
                    {
                        Id = manufacturerReader.HasColumn("Id") ? manufacturerReader.GetFieldValue<int>("Id") : null,
                        Name = manufacturerReader.HasColumn("Name") ? manufacturerReader.GetFieldValue<string>("Name") : null,
                        City = manufacturerReader.HasColumn("City") ? manufacturerReader.GetFieldValue<string>("City") : null
                    };
                        
                    Manufacturers.Add(manufacturer);
                }
            }
        }

        private List<string> GetNotNullPropertiesNames(object obj)
        {
            var notNullPropertiesNames = new List<string>();
            var props = obj.GetType().GetProperties();
            
            foreach (var prop in props)
            {
                if (prop.GetValue(obj) != null)
                {
                    notNullPropertiesNames.Add(prop.Name);
                }
            }
            
            return notNullPropertiesNames;
        }
    }
}