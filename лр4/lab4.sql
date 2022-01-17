use mydb;

SELECT * FROM Products;
SELECT * FROM Manufacturers;

DROP TABLE Products;
DROP TABLE Manufacturers;

CREATE TABLE Manufacturers(
	Id int AUTO_INCREMENT,
    Name varchar(255),
    City varchar(255),
    CONSTRAINT PK_Manufacturers PRIMARY KEY (Id)
);

CREATE TABLE Products(
	Id int AUTO_INCREMENT,
    Name varchar(255),
    Cost decimal(10, 2),
    Count int,
    ManufacturerId int, 
    CONSTRAINT PK_Products PRIMARY KEY (Id)
);

ALTER TABLE Products
ADD CONSTRAINT FK_ManufacturerId
FOREIGN KEY (ManufacturerId) REFERENCES Manufacturers(Id);

INSERT INTO Manufacturers (Name, City)
VALUES 
	('Обои из Краснодара', 'Краснодар'),
    ('Фабричные обои', 'Москва');
    
INSERT INTO Products (Name, Cost, Count, ManufacturerId)
VALUES
	('Речной закат', 549, 200, 1),
    ('Ежик в тумане', 799, 120, 2),
    ('Сингулярность бытия', 1199, 347, 2),
    ('Рассвет на севере', 399, 850, 1),
    ('Синева в глуши', 459, 421, 2);
    
UPDATE Products
SET ManufacturerId = 1
WHERE Id = 3;

DELETE FROM Products WHERE Name = 'Синева в глуши';

SELECT p.Name AS ProductName, p.Cost, p.Count, m.Name as ManufacturerName, m.City as ManufacturerCity
FROM Products as p
INNER JOIN Manufacturers as m
ON p.ManufacturerId = m.Id
ORDER BY Cost;

TRUNCATE TABLE Products;
DROP TABLE Products;

TRUNCATE TABLE Manufacturers;
DROP TABLE Manufacturers;


