CREATE TABLE DimLocation (
 	location_id varchar(50) primary key NOT NULL,
	road_id varchar(50) NOT NULL,
	lga_id varchar(50) NOT NULL,
	region_id varchar(50) NOT NULL,
	is_intersection BIT DEFAULT 0 NOT NULL,
	intersection_no int,
	intersection_desc varchar(500),
	FOREIGN KEY (road_id) REFERENCES DimRoad(road_id),
	FOREIGN KEY (lga_id) REFERENCES DimLGA(lga_id),
	FOREIGN KEY (region_id) REFERENCES DimRegion(region_id),
);
