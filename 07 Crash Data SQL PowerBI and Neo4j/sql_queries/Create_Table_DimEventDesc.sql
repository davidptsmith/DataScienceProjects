CREATE TABLE DimEventDesc (
 	event_id varchar(50) primary key NOT NULL,
	event_nature varchar(50),
	event_type varchar(50),
	severity varchar(50),
	longitude float(24),
	latitude float(24),
);
