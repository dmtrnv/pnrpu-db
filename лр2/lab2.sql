use mydb;

CREATE TABLE Employees (
  Id INT NOT NULL AUTO_INCREMENT,
  FullName VARCHAR(50) NOT NULL,
  Town VARCHAR(100) NOT NULL,
  PhoneNumber VARCHAR(15) NOT NULL,
  WorkExperience INT NOT NULL,
  Salary DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (Id));

INSERT INTO Employees (FullName, Town, PhoneNumber, WorkExperience, Salary)
VALUES 
('Павел Попов', 'Ульяновск', '89523248230', 5, 5000), 
('Иван Иванов', 'Самара', '89332585821', 3, 3000),
('Александр Владимирович', 'Пермь', '89523220907', 5, 4500),
('Игорь Романович', 'Пермь', '89523220907', 3, 1500),
('Павел Терентьевич', 'Пермь', '89457112433', 6, 3500),
('Викентий Григорьевич', 'Сочи', '89513245222', 2, 2500),
('Владимир Владимирович', 'Владивосток', '89525679123', 4, 4500);

SELECT * FROM Employees;

SELECT FullName, PhoneNumber, Salary FROM Employees;

SELECT FullName, Town FROM Employees 
ORDER BY Town;

SELECT FullName, WorkExperience FROM Employees
WHERE WorkExperience > 4;
