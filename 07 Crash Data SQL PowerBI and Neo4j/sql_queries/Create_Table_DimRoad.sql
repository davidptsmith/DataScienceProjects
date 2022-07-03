CREATE TABLE DimRoad (
 	road_id varchar(50) primary key NOT NULL,
	road_name varchar(500) NOT NULL,
	common_road_name varchar(500),
	cway varchar(50),
	speed_limit varchar(100),
	network_type varchar(50),
);
