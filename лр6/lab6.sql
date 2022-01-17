CREATE TABLE Test (
	Id INT AUTO_INCREMENT,
	Name VARCHAR(50),
    PRIMARY KEY (Id));

CREATE TABLE TestLog (
	Id INT AUTO_INCREMENT,
	Log VARCHAR(255),
    Date DATETIME,
    PRIMARY KEY (Id));


delimiter //
CREATE TRIGGER update_test
AFTER UPDATE ON Test
FOR EACH ROW
BEGIN
	INSERT INTO TestLog (Log, Date) VALUES (Concat('Updated ', old.Name, ' to ', new.Name, ' in row with Id = ', old.Id), NOW());
END;//
delimiter ;


delimiter //
CREATE TRIGGER insert_test
AFTER INSERT ON Test
FOR EACH ROW
BEGIN
	INSERT INTO TestLog (Log, Date) VALUES (Concat('Added row with Name = ', new.Name), NOW());
END;//
delimiter ;


delimiter //
CREATE TRIGGER delete_test
BEFORE DELETE ON Test
FOR EACH ROW
BEGIN
	INSERT INTO TestLog (Log, Date) VALUES (Concat('Row with name ', old.Name, ' will be deleted'), NOW());
END;//
delimiter ;


INSERT INTO Test (Name) VALUES
('Саша'),
('Паша'),
('Даша');

UPDATE Test SET Name = 'Лёва' WHERE Name = 'Паша';

DELETE FROM Test;

SELECT * FROM Test;
SELECT * FROM TestLog;


DROP TRIGGER update_test;
DROP TRIGGER insert_test;
DROP TRIGGER delete_test;