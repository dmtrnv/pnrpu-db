CREATE VIEW lab2query1 AS SELECT FullName, PhoneNumber, Salary FROM Employees;
CREATE VIEW lab2query2 AS SELECT FullName, Town FROM Employees ORDER BY Town;
CREATE VIEW lab2query3 AS SELECT FullName, WorkExperience FROM Employees WHERE WorkExperience > 4;

SELECT * FROM Employees;
SELECT * FROM lab2query1;
SELECT * FROM lab2query2;
SELECT * FROM lab2query3;
SELECT FullName FROM lab2query3;