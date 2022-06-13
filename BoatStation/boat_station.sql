CREATE TABLE watercraft_condition
(
	condition_id SERIAL PRIMARY KEY,
	name VARCHAR(32) NOT NULL
);

INSERT INTO watercraft_condition(name)
VALUES 
('Удовлетворительное'),
('В ремонте'),
('Списано');

CREATE TABLE watercraft_type
(
	type_id SERIAL PRIMARY KEY,
	name VARCHAR(32) NOT NULL
);

INSERT INTO watercraft_type (name)
VALUES
('Катер'),
('Яхта'),
('Лодка'),
('Катамаран'),
('Байдарка'),
('Водные лыжи');

CREATE TABLE life_saving_equipment
(
	equimpent_id SERIAL PRIMARY KEY,
	name VARCHAR(64) NOT NULL
);

INSERT INTO life_saving_equipment (name)
VALUES
('Круг'),
('Жилет'),
('Линь');

CREATE TABLE pond_type
(
	type_id SERIAL PRIMARY KEY,
	name VARCHAR(32) NOT NULL
);

INSERT INTO pond_type (name)
VALUES
('Река'),
('Озеро'),
('Пруд');

CREATE TABLE pond
(
	pond_id SERIAL PRIMARY KEY,
	type_id SERIAL REFERENCES pond_type(type_id) NOT NULL,
	name VARCHAR(128) NOT NULL
);

INSERT INTO pond (type_id, name)
VALUES
(1, 'Березка'),
(2, 'Петровское'),
(3, 'У озера');

CREATE TABLE watercraft
(
	watercraft_id SERIAL PRIMARY KEY,
	name VARCHAR(64),
	model VARCHAR(128) NOT NULL,
	type_id SERIAL REFERENCES watercraft_type(type_id) NOT NULL,
	condition_id SERIAL REFERENCES watercraft_condition(condition_id) NOT NULL,
	registration_number VARCHAR(8) CONSTRAINT chk_reg_num_length CHECK (char_length(registration_number) = 8),
	inventory_number VARCHAR(8) CONSTRAINT chk_inv_num_length CHECK (char_length(inventory_number) = 8) NOT NULL,
	-- Водоем, в котором может быть использовано плав. средство.
	pond_id SERIAL REFERENCES pond(pond_id) NOT NULL,
	comissioning_date DATE NOT NULL,
	next_repair_date DATE NOT NULL,
	operation_period_by_passport_in_months INT NOT NULL
);

CREATE FUNCTION set_dates_on_watercraft() RETURNS trigger AS $$
BEGIN
	NEW.comissioning_date = now();
	NEW.next_repair_date = NEW.comissioning_date + interval '1 month' * 6;
	RETURN NEW;
END
$$ LANGUAGE plpgsql;

CREATE TRIGGER set_dates_on_watercraft_before_insert BEFORE INSERT ON watercraft
	FOR EACH ROW EXECUTE PROCEDURE set_dates_on_watercraft();
	
INSERT INTO watercraft
(name, model, type_id, condition_id, registration_number, inventory_number, 
pond_id, operation_period_by_passport_in_months)
VALUES
('Заря', 'ИЖ-35М', 3, 1, 'Р79-91СФ', '32718902', 2, 120),
('Уралочка', 'ИЖ-35М', 3, 1, 'Ж32-11КГ', '32111230', 2, 120),
('Первый', 'Родина 588', 2, 1, 'У22-31ЦЦ', '11234452', 1, 240),
(null, 'Б-ОП-2М', 5, 1, null, '65432214', 1, 36),
(null, 'Б-ОП-2М', 5, 1, null, '34443223', 1, 36),
(null, 'Б-ОП-2М', 5, 2, null, '22344115', 1, 36),
('Восток', 'КУБ 22', 1, 1, 'О99-87ЕК', '66432213', 2, 120),
('Запад', 'КУБ 22', 1, 1, 'Л65-32ДБ', '11245346', 2, 120),
('Север', 'КУБ 22', 1, 1, 'З21-55КК', '44124213', 2, 120),
(null, 'К-1', 4, 1, null, '14564323', 3, 60),
(null, 'К-1', 4, 2, null, '11244410', 3, 60),
(null, 'К-1', 4, 3, null, '10000001', 3, 60),
(null, 'ЛП-4-21', 6, 1, null, '13221910', 2, 12),
(null, 'ЛП-4-21', 6, 1, null, '22113020', 2, 12),
(null, 'ЛП-4-21', 6, 1, null, '36177212', 2, 12),
(null, 'ЛП-4-21', 6, 1, null, '41123098', 2, 12),
(null, 'ЛП-4-21', 6, 3, null, '23455112', 2, 12),
(null, 'ЛП-4-21', 6, 3, null, '11432111', 2, 12);

UPDATE watercraft
SET comissioning_date = '2021-11-11'
WHERE inventory_number = '44124213';

UPDATE watercraft
SET comissioning_date = '2021-10-28'
WHERE inventory_number = '10000001';

UPDATE watercraft
SET comissioning_date = '2016-11-15', next_repair_date = '2021-11-15'
WHERE inventory_number = '23455112';

UPDATE watercraft
SET comissioning_date = '2020-11-09', next_repair_date = '2021-11-21'
WHERE inventory_number = '11432111';

CREATE TABLE watercraft_equipment
(
	watercraft_type_id SERIAL REFERENCES watercraft_type(type_id) NOT NULL,
	equipment_id SERIAL REFERENCES life_saving_equipment(equipment_id) NOT NULL,
	equipment_count INT CONSTRAINT chk_positive_count CHECK (equipment_count > 0) NOT NULL,
	PRIMARY KEY (watercraft_type_id, equipment_id)
);

INSERT INTO watercraft_equipment 
VALUES
(1, 1, 1),
(1, 2, 4),
(2, 1, 2),
(2, 2, 8),
(2, 3, 6),
(3, 1, 1),
(3, 2, 4),
(4, 2, 2),
(5, 2, 1),
(6, 2, 1);

CREATE TABLE pond_danger
(
	pond_type_id SERIAL REFERENCES  pond_type(type_id),
	watercraft_type_id SERIAL REFERENCES watercraft_type(type_id),
	danger INT CONSTRAINT chk_danger_in_range CHECK (danger > 0 AND danger < 6),
	PRIMARY KEY (pond_type_id, watercraft_type_id)
);

INSERT INTO pond_danger
VALUES
(1, 1, 2),
(1, 2, 1),
(1, 3, 3),
(1, 4, 4),
(1, 5, 5),
(1, 6, 5),
(2, 1, 2),
(2, 2, 1),
(2, 3, 2),
(2, 4, 3),
(2, 5, 3),
(2, 6, 3),
(3, 1, 2),
(3, 2, 1),
(3, 3, 2),
(3, 4, 2),
(3, 5, 2),
(3, 6, 2);

CREATE TABLE address
(
	address_id SERIAL PRIMARY KEY,
	region VARCHAR(64),
	city VARCHAR(64) NOT NULL,
	street VARCHAR(64),
	house_number VARCHAR(16),
	apartment_number VARCHAR(16)
);

CREATE TYPE gender AS ENUM ('М', 'Ж');

CREATE TABLE passport
(
	passport_id SERIAL PRIMARY KEY,
	client_id SERIAL REFERENCES client(client_id) NOT NULL,
	serial_number VARCHAR(4) NOT NULL,
	number VARCHAR(6) NOT NULL,
	issued_by VARCHAR(128) NOT NULL,
	division_code VARCHAR(7) NOT NULL,
	issue_date DATE NOT NULL,
	birth_date DATE NOT NULL,
	gender GENDER NOT NULL
);

CREATE TABLE client
(
	client_id SERIAL PRIMARY KEY,
	name VARCHAR(32) NOT NULL,
	surname VARCHAR(32) NOT NULL,
	patronymic VARCHAR(32) NOT NULL,
	address_id SERIAL REFERENCES address(address_id) NOT NULL
);

CREATE TABLE client_watercraft_contract
(
	contract_id SERIAL PRIMARY KEY,
	client_id SERIAL REFERENCES client(client_id) NOT NULL UNIQUE,
	watercraft_id SERIAL REFERENCES watercraft(watercraft_id) NOT NULL UNIQUE,
	briefing_id SERIAL REFERENCES briefing(briefing_id) NOT NULL,
	issue_datetime TIMESTAMP NOT NULL,
	use_duration_contract_in_minutes INT NOT NULL,
	use_duration_fact_in_minutes INT
);

CREATE TABLE client_watercraft_contract_archive
(
	contract_id INT NOT NULL,
	client_id INT NOT NULL,
	watercraft_id INT NOT NULL,
	briefing_id INT NOT NULL,
	issue_datetime TIMESTAMP NOT NULL,
	use_duration_contract_in_minutes INT NOT NULL,
	use_duration_fact_in_minutes INT NOT NULL
);

CREATE FUNCTION save_contract_to_archive() RETURNS trigger AS $$
BEGIN
	INSERT INTO client_watercraft_contract_archive
	SELECT * FROM old_table;
	
	RETURN NULL;
END
$$ LANGUAGE plpgsql;

CREATE TRIGGER archive_contract_on_delete AFTER DELETE ON client_watercraft_contract
REFERENCING OLD TABLE AS old_table
FOR EACH STATEMENT EXECUTE PROCEDURE save_contract_to_archive();

CREATE TABLE instructor
(
	instructor_id SERIAL PRIMARY KEY,
	name VARCHAR(32) NOT NULL,
	surname VARCHAR(32) NOT NULL,
	patronymic VARCHAR(32) NOT NULL,
	position VARCHAR(64) NOT NULL
);

INSERT INTO instructor (name, surname, patronymic, position)
VALUES
('Александр', 'Железов', 'Петрович', 'Инструктор по безопасности на воде');

CREATE VIEW get_instructors_as_strings AS
	SELECT CONCAT(surname, ' ', name, ' ', patronymic, ', ', position)
	FROM instructor;

CREATE TABLE briefing_type
(
	type_id SERIAL PRIMARY KEY,
	name VARCHAR(32) NOT NULL UNIQUE
);

INSERT INTO briefing_type (name)
VALUES
('Первичный'),
('Повторный');

CREATE TABLE briefing
(
	briefing_id SERIAL PRIMARY KEY,
	briefing_datetime TIMESTAMP NOT NULL,
	type_id SERIAL REFERENCES briefing_type(type_id) NOT NULL,
	instructor_id SERIAL REFERENCES instructor(instructor_id) NOT NULL,
	client_id SERIAL REFERENCES client(client_id) NOT NULL
);

CREATE TABLE watercraft_order_request
(
	request_id SERIAL PRIMARY KEY,
	watercraft_id SERIAL REFERENCES watercraft(watercraft_id) NOT NULL
);

CREATE TABLE watercraft_order
(
	order_id SERIAL PRIMARY KEY,
	request_id SERIAL REFERENCES watercraft_order_request(request_id) NOT NULL,
	request_date DATE NOT NULL
);

CREATE VIEW watercraft_orders AS
	SELECT watercraft_type.name AS Тип, model AS Модель, request_date AS Дата_заказа
	FROM watercraft_order
	INNER JOIN watercraft_order_request USING(request_id)
	INNER JOIN watercraft USING(watercraft_id)
	INNER JOIN watercraft_type USING(type_id);

CREATE VIEW watercraft_requests_to_order AS
	SELECT watercraft_type.name AS Тип, model AS Модель, inventory_number AS Инвентарный_№
	FROM watercraft_order_request
	INNER JOIN watercraft USING(watercraft_id)
	INNER JOIN watercraft_type USING(type_id)
	WHERE request_id NOT IN (SELECT request_id FROM watercraft_order);

CREATE FUNCTION create_order(inv_num VARCHAR(8)) RETURNS void AS $$
	INSERT INTO watercraft_order (request_id, request_date)
	VALUES 
	((SELECT request_id FROM watercraft_order_request
	 INNER JOIN watercraft USING(watercraft_id)
	 WHERE watercraft.inventory_number = inv_num),
	 now());
$$ LANGUAGE sql;

CREATE FUNCTION check_watercraft_operation_period_and_make_request_to_order() RETURNS void AS $$
	INSERT INTO watercraft_order_request (watercraft_id)
	(SELECT watercraft_id FROM watercraft
		WHERE 
	 	comissioning_date + operation_period_by_passport_in_months * interval '1 month' <= now() - interval '1 month' * 6
	 	AND	
	 	watercraft_id NOT IN (SELECT watercraft_id FROM watercraft_order_request));
$$ LANGUAGE sql;

CREATE VIEW watercraft_to_repair AS 
	SELECT watercraft_type.name AS Тип, model AS Модель, inventory_number AS Инвентарный_№
	FROM watercraft
	INNER JOIN watercraft_type USING(type_id)
	WHERE next_repair_date <= now();

CREATE FUNCTION make_repair(inv_num VARCHAR(8)) RETURNS void AS $$
	UPDATE watercraft
	SET next_repair_date = now() + interval '1 month' * 6
	WHERE watercraft.inventory_number = inv_num;
$$ LANGUAGE sql;

CREATE ROLE supply_department;
CREATE ROLE repair_department;
CREATE ROLE reception;
CREATE ROLE database_admin;

CREATE USER supplier_petrova WITH PASSWORD 'qwe';
CREATE USER repair_suslov WITH PASSWORD 'qwe';
CREATE USER reception_eleseeva WITH PASSWORD 'qwe';
CREATE USER admin_kluev WITH PASSWORD 'qwe';

REVOKE CREATE ON SCHEMA public FROM public;
REVOKE ALL ON DATABASE boat_station FROM public;

REVOKE ALL ON SCHEMA public FROM supply_department;
REVOKE ALL ON DATABASE boat_station FROM supply_department;

	GRANT CONNECT ON DATABASE boat_station TO supply_department;
	GRANT SELECT ON TABLE 
	public.watercraft_requests_to_order,
	public.watercraft_orders
	TO supply_department;
	GRANT USAGE, SELECT ON SEQUENCE watercraft_order_order_id_seq TO supply_department;
	GRANT ALL ON FUNCTION create_order TO supply_department;
	GRANT ALL ON FUNCTION check_watercraft_operation_period_and_make_request_to_order TO supply_department;

REVOKE ALL ON SCHEMA public FROM repair_department;
REVOKE ALL ON DATABASE boat_station FROM repair_department;

GRANT CONNECT ON DATABASE boat_station TO repair_department;
GRANT SELECT ON TABLE watercraft_to_repair TO repair_department;
GRANT ALL ON FUNCTION make_repair TO repair_department;

REVOKE ALL ON SCHEMA public FROM reception;
REVOKE ALL ON DATABASE boat_station FROM reception;

GRANT CONNECT ON DATABASE boat_station TO reception;
GRANT SELECT, UPDATE, INSERT, DELETE, REFERENCES, TRIGGER ON TABLE
public.address,
public.passport,
public.client,
public.client_watercraft_contract,
public.client_watercraft_contract_archive,
public.briefing
TO reception;
GRANT SELECT, REFERENCES ON TABLE
public.briefing_type,
public.watercraft,
public.watercraft_type,
public.watercraft_equipment,
public.life_saving_equipment,
public.watercraft_condition,
public.pond,
public.pond_type,
public.pond_danger,
public.instructor
TO reception;
GRANT SELECT ON TABLE pond_type TO reception;
GRANT SELECT ON TABLE get_instructors_as_strings TO reception;
GRANT SELECT ON TABLE client_base TO reception;
GRANT SELECT ON TABLE contract_archive TO reception;
GRANT SELECT ON TABLE watercraft_base TO reception;
GRANT SELECT ON TABLE pond_base TO reception;
GRANT SELECT ON TABLE active_contracts TO reception;
GRANT ALL ON FUNCTION create_client TO reception;
GRANT ALL ON FUNCTION create_contract TO reception;
GRANT ALL ON FUNCTION make_briefing TO reception;
GRANT ALL ON FUNCTION complete_contract TO reception;
GRANT USAGE, SELECT ON SEQUENCE passport_passport_id_seq TO reception;
GRANT USAGE, SELECT ON SEQUENCE address_address_id_seq TO reception;
GRANT USAGE, SELECT ON SEQUENCE client_client_id_seq TO reception;
GRANT USAGE, SELECT ON SEQUENCE briefing_briefing_id_seq TO reception;
GRANT USAGE, SELECT ON SEQUENCE client_watercraft_contract_contract_id_seq TO reception;
GRANT USAGE, SELECT ON SEQUENCE client_watercraft_contract_client_id_seq TO reception;
GRANT USAGE, SELECT ON SEQUENCE client_watercraft_contract_watercraft_id_seq TO reception;
GRANT USAGE, SELECT ON SEQUENCE client_watercraft_contract_briefing_id_seq TO reception;

GRANT CONNECT ON DATABASE boat_station TO database_admin;
GRANT ALL PRIVILEGES ON SCHEMA public TO database_admin;
GRANT ALL PRIVILEGES ON DATABASE boat_station to database_admin;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO database_admin;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO database_admin;
GRANT ALL PRIVILEGES ON ALL FUNCTIONS IN SCHEMA public TO database_admin;

GRANT supply_department TO supplier_petrova;
GRANT repair_department TO repair_suslov;
GRANT reception TO reception_eleseeva;
GRANT database_admin TO admin_kluev;

SELECT * FROM pg_roles;

SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname = 'public'
UNION
SELECT viewname FROM pg_catalog.pg_views WHERE schemaname = 'public';

CREATE FUNCTION create_client
(ser_num VARCHAR(4), num VARCHAR(6), iss_by VARCHAR(128), div_code VARCHAR(7), iss_date DATE, b_date DATE, 
gend GENDER, reg VARCHAR(64), cty VARCHAR(64), strt VARCHAR(64), h_num VARCHAR(16), ap_num VARCHAR(16), 
nm VARCHAR(32), surnm VARCHAR(32), patronmc VARCHAR(32)) 
RETURNS int AS $$
DECLARE
	p_id INT;
	a_id INT;
	c_id INT;
BEGIN
	IF (SELECT COUNT(*) > 0 FROM passport
	   WHERE serial_number = ser_num AND number = num) 
	THEN
		SELECT client_id INTO c_id FROM passport
		WHERE serial_number = ser_num AND number = num;
		
		RETURN c_id;
	END IF;
	
	INSERT INTO address(region, city, street, house_number, apartment_number)
	VALUES (reg, cty, strt, h_num, ap_num)
	RETURNING address_id INTO a_id;
			
	INSERT INTO client(name, surname, patronymic, address_id)
	VALUES (nm, surnm, patronmc, a_id)
	RETURNING client_id INTO c_id;
	
	INSERT INTO passport(client_id, serial_number, number, issued_by, division_code, issue_date, birth_date, gender)
	VALUES (c_id, ser_num, num, iss_by, div_code, iss_date, b_date, gend)
	RETURNING passport_id INTO p_id;
	
	RETURN c_id;
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION make_briefing (b_type_id INT, i_id INT, c_id INT)
RETURNS INT AS $$
DECLARE
	b_id INT;
BEGIN
	INSERT INTO briefing (briefing_datetime, type_id, instructor_id, client_id)
	VALUES (now(), b_type_id, i_id, c_id)
	RETURNING briefing_id INTO b_id;
	
	RETURN b_id;
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION make_briefing_and_contract 
(b_type_id INT, i_id INT, c_id INT, w_id INT, use_dur_contract_in_min INT) 
RETURNS void AS $$
DECLARE
	b_id INT;
BEGIN
	INSERT INTO briefing (briefing_datetime, type_id, instructor_id, client_id)
	VALUES (now(), b_type_id, i_id, c_id)
	RETURNING briefing_id INTO b_id;
	
	INSERT INTO client_watercraft_contract (client_id, watercraft_id, briefing_id, issue_datetime, use_duration_contract_in_minutes)
	VALUES (c_id, w_id, b_id, now(), use_dur_contract_in_min);
END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION create_contract
(c_id INT, w_id INT, b_id INT, use_dur_contract_in_min INT)
RETURNS void AS $$
	INSERT INTO client_watercraft_contract (client_id, watercraft_id, briefing_id, issue_datetime, use_duration_contract_in_minutes)
	VALUES (c_id, w_id, b_id, now(), use_dur_contract_in_min);
$$ LANGUAGE SQL;

SELECT unnest(enum_range(NULL::gender));

CREATE FUNCTION get_available_watercraft_by_pond(pond_type_name VARCHAR(128)) 
RETURNS TABLE(Идентификатор INT, Название VARCHAR, Тип VARCHAR, Модель VARCHAR,
			  Регистрационный_№ VARCHAR, Спас_средства TEXT) AS $$
	SELECT watercraft_id, watercraft.name, watercraft_type.name, model, watercraft.registration_number, 
	array_to_string(array_agg(life_saving_equipment.name), ', ')
	FROM watercraft
	LEFT JOIN client_watercraft_contract USING(watercraft_id)
	INNER JOIN watercraft_type USING(type_id)
	INNER JOIN watercraft_equipment ON type_id = watercraft_type_id
	INNER JOIN life_saving_equipment USING(equipment_id)
	INNER JOIN watercraft_condition USING(condition_id)
	INNER JOIN pond USING(pond_id)
	INNER JOIN pond_type ON pond.type_id = pond_type.type_id
	WHERE pond_type.name = pond_type_name AND watercraft_condition.name = 'Удовлетворительное'
	GROUP BY watercraft_id, watercraft_type.name;											  
$$ LANGUAGE SQL;

SELECT array_to_string(array_agg(life_saving_equipment.name), ', '), array_agg(equipment_count)
FROM watercraft_equipment
INNER JOIN life_saving_equipment USING (equipment_id)
WHERE watercraft_type_id = 1;	

SELECT pond_type.name FROM pond_type;

SELECT * FROM get_available_watercraft_by_pond('Озеро');

CREATE VIEW client_base AS
	SELECT CONCAT(surname, ' ', name, ' ', patronymic) AS ФИО, birth_date AS Дата_рождения,
	CONCAT(serial_number, ' ', number) AS Серия_и_номер_паспорта, gender AS Пол,
	CONCAT(region, ', ', city, ', ', street, ', ', house_number, ', ', apartment_number) AS Адрес
	FROM client
	JOIN passport USING(client_id)
	JOIN address USING(address_id);
	
CREATE VIEW contract_archive AS
	SELECT CONCAT(surname, ' ', client.name, ' ', patronymic) AS ФИО, birth_date AS Дата_рождения,
	CONCAT(serial_number, ' ', number) AS Серия_и_номер_паспорта, watercraft_type.name AS Тип,
	watercraft.model AS Модель, inventory_number AS Инвентарный_№, issue_datetime AS Дата_выдачи,
	use_duration_fact_in_minutes AS Время_использования_мин
	FROM client_watercraft_contract_archive
	JOIN client USING(client_id)
	JOIN address USING(address_id)
	JOIN passport USING(client_id)
	JOIN watercraft USING(watercraft_id)
	JOIN watercraft_type USING(type_id);

CREATE VIEW watercraft_base AS
	SELECT watercraft_type.name AS Тип, watercraft.model AS Модель, watercraft.name AS Название,
	inventory_number AS Инвентарный_№, registration_number AS Регистрационный_№,
	watercraft_condition.name AS Состояние, CONCAT(pond_type.name, ' ', pond.name) AS Водоем,
	pond_danger.danger AS Опасность,
	array_to_string(array_agg(life_saving_equipment.name), ', ') AS Спас_средства
	FROM watercraft
	JOIN watercraft_type ON watercraft.type_id = watercraft_type.type_id
	JOIN watercraft_condition USING(condition_id)
	JOIN pond USING(pond_id)
	JOIN pond_type ON pond.type_id = pond_type.type_id
	JOIN pond_danger ON pond.type_id = pond_danger.pond_type_id AND watercraft.type_id = pond_danger.watercraft_type_id
	INNER JOIN watercraft_equipment ON watercraft.type_id = watercraft_equipment.watercraft_type_id
	INNER JOIN life_saving_equipment USING(equipment_id)
	GROUP BY watercraft_type.name, watercraft.model, watercraft.name, inventory_number, registration_number,
	watercraft_condition.name, pond_type.name, pond.name, pond_danger.danger
	ORDER BY watercraft_type.name;
	
CREATE VIEW pond_base AS
	SELECT pond_type.name AS Тип, pond.name AS Название
	FROM pond
	JOIN pond_type USING(type_id);
	
CREATE VIEW active_contracts AS
	SELECT CONCAT(surname, ' ', client.name, ' ', patronymic) AS ФИО, birth_date AS Дата_рождения,
	CONCAT(serial_number, ' ', number) AS Серия_и_номер_паспорта, watercraft_type.name AS Тип,
	watercraft.model AS Модель, inventory_number AS Инвентарный_№, issue_datetime AS Дата_выдачи,
	use_duration_contract_in_minutes AS Время_аренды_мин
	FROM client_watercraft_contract
	JOIN client USING(client_id)
	JOIN address USING(address_id)
	JOIN passport USING(client_id)
	JOIN watercraft USING(watercraft_id)
	JOIN watercraft_type USING(type_id);

CREATE FUNCTION complete_contract(inv_num VARCHAR(8))
RETURNS void AS $$
DECLARE
	w_id INT;
	iss_dt TIMESTAMP;
BEGIN
	SELECT watercraft_id INTO w_id
	FROM watercraft
	WHERE inventory_number = inv_num;
	
	SELECT issue_datetime INTO iss_dt FROM client_watercraft_contract
	WHERE watercraft_id = w_id;
	
	UPDATE client_watercraft_contract
	SET use_duration_fact_in_minutes = trunc((SELECT extract(epoch from (now() - iss_dt))) / 60)
	WHERE watercraft_id = w_id;
	
	DELETE FROM client_watercraft_contract
	WHERE watercraft_id = w_id;
END
$$ LANGUAGE plpgsql;

