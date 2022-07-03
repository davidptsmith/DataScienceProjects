CREATE TABLE DimData (
 	date_id int primary key NOT NULL,
	year int NOT NULL,
	month int NOT NULL,
	day int NOT NULL,
	quarter int NOT NULL,
	day_of_week varchar(16) NOT NULL,
	is_holiday BIT DEFAULT 0 NOT NULL,
);
