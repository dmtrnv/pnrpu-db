using System.Collections.Generic;

namespace DbLab4.Misc
{
    public static class SqlCommands
    {
        public static Dictionary<string, string> CommandsReader = new()
        {
            ["SelectAllProducts"] = "SELECT * FROM Products;",
            ["SelectAllManufacturers"] = "SELECT * FROM Manufacturers;",
            ["SelectProductsInnerJoinManufacturers"] = "SELECT p.Name AS ProductName, p.Cost, p.Count, m.Name AS ManufacturerName, m.City AS ManufacturerCity FROM Products AS p INNER JOIN Manufacturers AS m ON p.ManufacturerId = m.Id ORDER BY Cost;"
        };
        
        public static Dictionary<string, string> CommandsNonQuery = new()
        {
            ["CreateManufacturers"] = "CREATE TABLE Manufacturers(Id int AUTO_INCREMENT, Name varchar(255), City varchar(255), CONSTRAINT PK_Manufacturers PRIMARY KEY (Id));",
            ["CreateProducts"] = "CREATE TABLE Products(Id int AUTO_INCREMENT, Name varchar(255), Cost decimal(10, 2), Count int, ManufacturerId int, CONSTRAINT PK_Products PRIMARY KEY (Id));",
            ["AlterProductAddFK"] = "ALTER TABLE Products ADD CONSTRAINT FK_ManufacturerId FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(Id);",
            ["InsertIntoManufacturers"] = "INSERT INTO Manufacturers (Name, City) VALUES ('Обои из Краснодара', 'Краснодар'), ('Фабричные обои', 'Москва');",
            ["InsertIntoProducts"] = "INSERT INTO Products (Name, Cost, Count, ManufacturerId) VALUES ('Речной закат', 549, 200, 1), ('Ежик в тумане', 799, 120, 2), ('Сингулярность бытия', 1199, 347, 2), ('Рассвет на севере', 399, 850, 1), ('Синева в глуши', 459, 421, 2);",
            ["UpdateProduct"] = "UPDATE Products SET ManufacturerId = 1 WHERE Id = 3;",
            ["DeleteProduct"] = "DELETE FROM Products WHERE Name = 'Синева в глуши';",
            ["TruncateProducts"] = "TRUNCATE TABLE Products;",
            ["TruncateManufacturers"] = "TRUNCATE TABLE Manufacturers;",
            ["DropProducts"] = "DROP TABLE Products;",
            ["DropManufacturers"] = "DROP TABLE Manufacturers;"
        };
    }
}