CREATE TABLE DimDate (
 	date_id varchar(50) primary key NOT NULL,
	year int NOT NULL,
	month varchar(50) NOT NULL,
	day varchar(50) NOT NULL,
	quarter int NOT NULL,
	day_of_week varchar(16) NOT NULL,
);
