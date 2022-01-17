using System.Collections.Generic;
using System.Data;
using DbLab3.Misc;
using DbLab3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace DbLab3.Pages
{
    public class Index : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<Employee> Employees { get; set; } = new();
        
        public List<string> NotNullPropertiesNames { get; set; } = new();

        public Index(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void OnGet()
        {
            
        }
        
        public PartialViewResult OnGetQueryResultPartial([FromQuery] string commandName)
        {
            if (commandName != null && SqlCommands.Commands.TryGetValue(commandName, out var commandText))
            {
                // Получение строки подключения из файла конфигурации
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Создание подключения с базой данных
                using var connection = new MySqlConnection(connectionString);

                // Открытие подключения
                connection.Open();

                // Создание запроса
                using var cmd = new MySqlCommand(commandText, connection);

                // Выполнение запроса
                using var reader = cmd.ExecuteReader();
                
                // Обработка результата запроса
                while (reader.Read())
                {
                    var employee = new Employee
                    {
                        Id = reader.HasColumn("Id") ? reader.GetFieldValue<int>("Id") : null,
                        FullName = reader.HasColumn("FullName") ? reader.GetFieldValue<string>("FullName") : null,
                        Address = reader.HasColumn("Address") ? reader.GetFieldValue<string>("Address") : null,
                        PhoneNumber = reader.HasColumn("PhoneNumber") ? reader.GetFieldValue<string>("PhoneNumber") : null,
                        WorkExperience = reader.HasColumn("WorkExperience") ? reader.GetFieldValue<int>("WorkExperience") : null,
                        Salary = reader.HasColumn("Salary") ? reader.GetFieldValue<decimal>("Salary") : null
                    };
                                
                    Employees.Add(employee);
                }
            }
           
            if (Employees.Count != 0)
            {
                NotNullPropertiesNames = GetNotNullPropertiesNames(Employees[0]);
            }
            
            // Возвращение частичной разметки
            return Partial("_QueryResultPartial", this);
        }

        private List<string> GetNotNullPropertiesNames(Employee employee)
        {
            var notNullPropertiesNames = new List<string>();
            var props = employee.GetType().GetProperties();
            
            foreach (var prop in props)
            {
                if (prop.GetValue(employee) != null)
                {
                    notNullPropertiesNames.Add(prop.Name);
                }
            }
            
            return notNullPropertiesNames;
        }
    }
}