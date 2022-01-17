use mydb;

delimiter //
CREATE PROCEDURE CountEmployeesAvgExperience()
BEGIN
	SELECT AVG(WorkExperience) AS AverageWorkExperience FROM Employees;
END;//
delimiter ;

CALL CountEmployeesAvgExperience();




delimiter //
CREATE PROCEDURE CountPeopleFromTown(IN in_town VARCHAR(30), OUT out_count INT)
BEGIN
	SELECT count(*) INTO out_count
    FROM Employees
	WHERE Town = in_town;
END;//
delimiter ;

CALL CountPeopleFromTown('Пермь', @out_count);
SELECT @out_count;



ALTER TABLE Employees ADD Status VARCHAR(50);

SELECT * FROM Employees;

delimiter //
CREATE PROCEDURE SetEmployeeStatus(IN employeeId INT)
BEGIN
	DECLARE val DECIMAL(10, 2);
    
    SELECT Salary
    INTO val
    FROM Employees
    WHERE Id = employeeId;
    
    IF val > 3000 THEN
        UPDATE Employees SET Status = 'Высокооплачиваемый' WHERE Id = employeeId;
	ELSE
		UPDATE Employees SET Status = 'Низкооплачиваемый' WHERE Id = employeeId;
	END IF;
END;//
delimiter ;

CALL SetEmployeeStatus(1);
CALL SetEmployeeStatus(3);
CALL SetEmployeeStatus(4);
CALL SetEmployeeStatus(5);

SELECT * FROM Employees;