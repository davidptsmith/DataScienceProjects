CREATE TABLE DimWeatherStn (
 	station_id varchar(50) primary key NOT NULL,
	station_name varchar(100) NOT NULL,
	country varchar(50) NOT NULL,
	region varchar(50) NOT NULL,
	elevation int,
	daily_start_date varchar(50),
	daily_end_date varchar(50),
	longitude float(24),
	latitude float(24),
);
