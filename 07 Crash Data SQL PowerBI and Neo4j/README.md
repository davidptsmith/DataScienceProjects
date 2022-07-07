# ETL to SQL Server with Python 

![ETL@4x](https://user-images.githubusercontent.com/76982323/177718282-72433c99-e415-45d5-8de3-2181540e1a3f.png)

![Artboard 7@4x](https://user-images.githubusercontent.com/76982323/177718248-419bc322-296a-4bbe-aeea-beb384ce6ed2.png)

## SQL Database Design 


![Dimension Schemas copy@4x](https://user-images.githubusercontent.com/76982323/177718297-0be1693b-457b-484a-b182-58d710e4b35a.png)

![SnowFlack Schema copy@4x](https://user-images.githubusercontent.com/76982323/177718366-235acbbf-139f-4c89-a6e8-b35190bec624.png)


**Note:**

Do not use the SQL ingestion code - yes I did, and yes I knew it was wrong (woops) but I knew that it was only for this dataset where I knew the origin and had gone through the data very carefully first. 

If you use the python code for ingesting the data to SQL you open youself up to a SQL Injection attack. Don't be like me, do it the right way (or not, it’s really up to you but you have been warned!). 

# Power BI

## Overall

![image](https://user-images.githubusercontent.com/76982323/177719675-8b4cbfe6-cce7-4098-8e02-bcfb187bd62d.png)



## Crash Data for Local Governments
![image](https://user-images.githubusercontent.com/76982323/177718766-7803d137-0ab5-4b28-b81c-0813b4710fbd.png)


## Crash Data by Road

![image](https://user-images.githubusercontent.com/76982323/177719067-c1c84894-5c34-4733-8520-7e0917a6a8d1.png)

![image](https://user-images.githubusercontent.com/76982323/177719126-3d7cf43f-06e0-4bb6-bf5f-78cfda62b8f5.png)


## Speed & Peak Hour

![image](https://user-images.githubusercontent.com/76982323/177719867-aa9957ef-5eda-4451-b190-b5fc94bb0b26.png)

![image](https://user-images.githubusercontent.com/76982323/177719962-443e6883-5f75-4041-be1e-057bfe8836b3.png)

## Region 

![image](https://user-images.githubusercontent.com/76982323/177720029-624eadb1-2cc7-4f95-b562-e33426684e49.png)

## Precipitation 

![image](https://user-images.githubusercontent.com/76982323/177720561-be38486a-c82f-4702-b583-82a937b0033d.png)


<br/>

# Graph Database with Neo4j 

A set of python functions were created to handle the ingesting the data to Neo4j and creating relationships. See the notebook for the details of the functions. 


## Neo4j CSV Files

Create a set of neo4j csv files from source rather than from pervious csv. 


## Design 

I have chosen to model severity, nature, and type as nodes for speed of queries and allow for searches to run from this nodes outwards. This will also allow for the scheme to adapt easily if these values change in future data releases. 

The date has been modeled as a as a node rather than property due to its connection to crash and weather reading. It will also allow for fast searches based on dates which is key for this data.  

Whist it would have been nice to model each individual vehicle in the crash as a node and a relationship between them and a crash node be connected, the data was not collected in such a way that would facilitate this. 

### Pros 

The ability to update the scheme is a huge benefit. It allows for fast ideation and iteration.  

Cypher query reads nicely, it is intuitive to read and write and executes quickly.  

### Cons 

Documentation and community is still growing - not as many stack overflow resources for more complex queries as relational databases. (This might also be due to me being new to graph dbs)

Parsing data from traditional sources can be clunky and unintuitive as important relationships are not captured well. This is a pro for the graph database if the data is coming from source/ purpose built solutions. However, most data sources are setup primarily for relational databases. 

**Note:**

*In the image below some of the relationships are in lowercase or capitalized. This has been updated to all upper case in the modeling of the graph database.* 
The schema depicted below was not the final one used in the data modelling. Some further refinement and optimisation can be achieved in the design of connections and properties. 

![Graph_Design](https://user-images.githubusercontent.com/76982323/177720962-349f1c3b-e70f-4e73-a5ec-de642480d62e.png)


## Cypher Queries 

For this section I have replaced job terms with their closest reference in the crash dataset. Some of the queries have been further altered to make more sense with respect to the chosen dataset. 

## Write Cypher queries to answer the following questions:
- **How many crashes are of a given crash type in a specified local government?**
    ```
    MATCH (s:Severity)<-[:WAS]-(n:Crash)-[:IN]->(p:LGA {local_gov_name: "Belmont (C)"})
    RETURN s.severity AS severity, COUNT(*) AS Crashes;
    ```

    Return:
    ```
    ╒═══════════╤═════════════╕
    │"severity" │"Crashes"    │
    ╞═══════════╪═════════════╡
    │"PDO Major"│27           │
    ├───────────┼─────────────┤
    │"PDO Minor"│22           │
    ├───────────┼─────────────┤
    │"Medical"  │3            │
    └───────────┴─────────────┘
    ```


- **Find crashes that share the same crash type.**
    ```
    MATCH (n:Crash)-[r:WAS]->(p:EventType {event_type: "Involving Overtaking"})
    RETURN n, r, p
    ```

    Response (sample):
    ```
    ╒══════════════════════════════════════════════════════════════════════╤═══╤═════════════════════════════════════╕
    │"n"                                                                   │"r"│"p"                                  │
    ╞══════════════════════════════════════════════════════════════════════╪═══╪═════════════════════════════════════╡
    │{"total_pedestrians_involve":0,"severity":"PDO Major","event_nature":"│{} │{"event_type":"Involving Overtaking"}│
    │Sideswipe Same Dirn","event_type":"Involving Overtaking","event_id":"1│   │                                     │
    │32319","latitude":-31.600145,"total_bike_involved":0,"total_truck_invo│   │                                     │
    │lved":0,"total_heavy_truck_involved":0,"total_motor_cycle_involved":0,│   │                                     │
    │"total_other_vehicles_involved":0,"longitude":115.681653}             │   │                                     │
    ├──────────────────────────────────────────────────────────────────────┼───┼─────────────────────────────────────┤
    │{"total_pedestrians_involve":0,"severity":"PDO Minor","event_nature":"│{} │{"event_type":"Involving Overtaking"}│
    │Sideswipe Same Dirn","event_type":"Involving Overtaking","event_id":"1│   │                                     │
    │32359","latitude":-31.863972,"total_bike_involved":0,"total_truck_invo│   │                                     │
    │lved":0,"total_heavy_truck_involved":0,"total_motor_cycle_involved":0,│   │                                     │
    │"total_other_vehicles_involved":0,"longitude":115.808358}             │   │                                     │
    ├──────────────────────────────────────────────────────────────────────┼───┼─────────────────────────────────────┤

    ```

    Counting number of crash types 
    ```
    MATCH (n:Crash)
    RETURN n.event_type AS event_type, COUNT(*) AS Crashes;
    ```


    Response: 
    ```
    ╒═════════════════════════════╤═════════════╕
    │"event_type"                 │"Crashes"    │
    ╞═════════════════════════════╪═════════════╡
    │"Involving Animal"           │246          │
    ├─────────────────────────────┼─────────────┤
    │"Involving Overtaking"       │741          │
    ├─────────────────────────────┼─────────────┤
    │"Involving Parking"          │410          │
    ├─────────────────────────────┼─────────────┤
    │"Entering / Leaving Driveway"│613          │
    ├─────────────────────────────┼─────────────┤
    │"Involving Pedestrian"       │97           │
    └─────────────────────────────┴─────────────┘
    ```

- **Find all different crash types for each local governments.**

    ```
    MATCH (lga:LGA)<-[:IN]-(c:Crash)-[:WAS]->(t:EventType)
    RETURN lga.local_gov_name as LGA ,  t.event_type as type, count(*) as Crashes 
    ORDER BY Crashes DESC
    ```

    Sample Response: 
    ```
    ╒══════════════════════════╤═════════════════════════════╤═════════╕
    │"LGA"                     │"type"                       │"Crashes"│
    ╞══════════════════════════╪═════════════════════════════╪═════════╡
    │"Stirling (C)"            │"Entering / Leaving Driveway"│75       │
    ├──────────────────────────┼─────────────────────────────┼─────────┤
    │"Stirling (C)"            │"Involving Overtaking"       │72       │
    ├──────────────────────────┼─────────────────────────────┼─────────┤
    │"Perth (C)"               │"Involving Overtaking"       │62       │
    ├──────────────────────────┼─────────────────────────────┼─────────┤
    │"Canning (C)"             │"Involving Overtaking"       │49       │
    ├──────────────────────────┼─────────────────────────────┼─────────┤
    ```


- **Find crashes based on the presence of a cyclist.** 
    ```
    MATCH (n:Crash )
    WHERE n.total_bike_involved > 0
    RETURN n
    ```

    Sample Response: 
    ```
    ╒══════════════════════════════════════════════════════════════════════╕
    │"n"                                                                   │
    ╞══════════════════════════════════════════════════════════════════════╡
    │{"total_pedestrians_involve":0,"severity":"Medical","event_nature":"Si│
    │deswipe Same Dirn","event_type":"Involving Overtaking","event_id":"122│
    │851","latitude":-32.031752,"total_bike_involved":1,"total_truck_involv│
    │ed":0,"total_heavy_truck_involved":0,"total_motor_cycle_involved":0,"t│
    │otal_other_vehicles_involved":1,"longitude":115.747384}               │
    ├──────────────────────────────────────────────────────────────────────┤
    │{"total_pedestrians_involve":0,"severity":"Medical","event_nature":"Ri│
    │ght Angle","event_type":"Entering / Leaving Driveway","event_id":"1229│
    │21","latitude":-32.276304,"total_truck_involved":0,"total_bike_involve│
    │d":1,"total_heavy_truck_involved":0,"total_motor_cycle_involved":0,"to│
    │tal_other_vehicles_involved":1,"longitude":115.72863}                 │
    ├──────────────────────────────────────────────────────────────────────┤
    ```


- **Find crashes posted during a specified period of time (2017-2019).** 
    Number of crashes for each month between 2017 & 2918

    ```
    MATCH (n:Crash)-[r:ON]->(d:Date)
    WHERE 2016< d.year < 2019
    WITH d.year + "-" + d.month as date_value , n
    RETURN distinct(date_value) as Date, count(n) as Crashes ORDER BY Date
    ```
    sample Response: 
    ```
    ╒═════════╤═════════╕
    │"Date"   │"Crashes"│
    ╞═════════╪═════════╡
    │"2017-01"│10       │
    ├─────────┼─────────┤
    │"2017-02"│4        │
    ├─────────┼─────────┤
    │"2017-03"│11       │
    ├─────────┼─────────┤
    │"2017-04"│7        │
    ├─────────┼─────────┤
    │"2017-05"│8        │
    ├─────────┼─────────┤
    ```
   
    - **Count By Month** 
    ```
    MATCH (n:Crash)-[r:ON]->(d:Date)
    RETURN distinct(d.month) as Month, count(n) as Crashes ORDER BY Month
    ```
    Response: 
    ```
    ╒═══════╤═════════╕
    │"Month"│"Crashes"│
    ╞═══════╪═════════╡
    │"01"   │50       │
    ├───────┼─────────┤
    │"02"   │39       │
    ├───────┼─────────┤
    │"03"   │56       │
    ├───────┼─────────┤
    │"04"   │55       │
    ├───────┼─────────┤
    │"05"   │58       │
    ├───────┼─────────┤
    │"06"   │62       │
    ├───────┼─────────┤
    │"07"   │51       │
    ├───────┼─────────┤
    │"08"   │69       │
    ├───────┼─────────┤
    │"09"   │222      │
    ├───────┼─────────┤
    │"10"   │340      │
    ├───────┼─────────┤
    │"11"   │521      │
    ├───────┼─────────┤
    │"12"   │584      │
    └───────┴─────────┘
    ```

### Write Cypher queries for at least two other meaningful queries that you can think of.

- **What local government region has the most Major crashes?** 
    ```
    MATCH (s:Severity {severity: "PDO Major"})<-[:WAS]-(n:Crash)-[:IN]->(p:LGA)
    RETURN p.local_gov_name as LGA, COUNT(*) AS FatalCrashes
    ORDER BY FatalCrashes DESC
    limit 5;
    ```

    Response: 
    ```
    ╒══════════════╤══════════════╕
    │"LGA"         │"FatalCrashes"│
    ╞══════════════╪══════════════╡
    │"Stirling (C)"│105           │
    ├──────────────┼──────────────┤
    │"Perth (C)"   │68            │
    ├──────────────┼──────────────┤
    │"Canning (C)" │54            │
    ├──────────────┼──────────────┤
    │"Cockburn (C)"│53            │
    ├──────────────┼──────────────┤
    │"Melville (C)"│50            │
    └──────────────┴──────────────┘
    ```
- **How many crashes occurred on rainy days?** 
    
    ```
    MATCH (n:Crash)-[r:READING]->(w:WeatherReading)
    WHERE w.precipitation_mm > 0
    RETURN count(n) as number_of_crashes
    ```
    
    Response:
    ```
    ╒═══════════════════╕
    │"number_of_crashes"│
    ╞═══════════════════╡
    │2489               │
    └───────────────────┘
    ```

    Return those days & crashes 
    
    ```
    MATCH (n:Crash)-[r:READING]->(w:WeatherReading)
    WHERE w.precipitation_mm > 0
    RETURN n, r, w
    ```

    Sample Response: 
    ```
    ╒══════════════════════════════════════════════╤═══╤══════════════════════════════════════════════╕
    │"n"                                           │"r"│"w"                                           │
    ╞══════════════════════════════════════════════╪═══╪══════════════════════════════════════════════╡
    │{"total_pedestrians_involve":0,"severity":"PDO│{} │{"temp_ave":13.9,"temp_min":14.8,"weather_id":│
    │ Major","event_nature":"Right Angle","event_id│   │"94610_2021-10-01","wind_direction":313.0,"win│
    │":"131274","latitude":-31.941698,"total_truck_│   │d_speed":18.4,"pressure":1006.3,"temp_max":19.│
    │involved":0,"total_bike_involved":0,"total_hea│   │6,"precipitation_mm":3.0}                     │
    │vy_truck_involved":0,"total_motor_cycle_involv│   │                                              │
    │ed":0,"total_other_vehicles_involved":0,"longi│   │                                              │
    │tude":115.925635}                             │   │                                              │
    ├──────────────────────────────────────────────┼───┼──────────────────────────────────────────────┤
    │{"total_pedestrians_involve":0,"severity":"PDO│{} │{"temp_ave":13.9,"temp_min":14.8,"weather_id":│
    │ Minor","event_nature":"Sideswipe Same Dirn","│   │"94610_2021-10-01","wind_direction":313.0,"win│
    │event_type":"Involving Overtaking","event_id":│   │d_speed":18.4,"pressure":1006.3,"temp_max":19.│
    │"122844","latitude":-31.945757,"total_bike_inv│   │6,"precipitation_mm":3.0}                     │
    │olved":0,"total_truck_involved":0,"total_heavy│   │                                              │
    │_truck_involved":1,"total_motor_cycle_involved│   │                                              │
    │":0,"total_other_vehicles_involved":1,"longitu│   │                                              │
    │de":115.839168}                               │   │                                              │
    ├──────────────────────────────────────────────┼───┼──────────────────────────────────────────────┤
    ```

- **Number of crashes by Severity for each year.**

    ```
     MATCH (s:Severity)<-[:WAS]-(n:Crash)-[r:ON]->(d:Date)
     RETURN distinct(d.year) as Date, s.severity as sev, count(n) as Crashes 
     ORDER BY Date
    ```

    Response: 
    ```
    ╒══════╤═══════════╤═════════╕
    │"Date"│"sev"      │"Crashes"│
    ╞══════╪═══════════╪═════════╡
    │2017  │"PDO Major"│250      │
    ├──────┼───────────┼─────────┤
    │2017  │"PDO Minor"│176      │
    ├──────┼───────────┼─────────┤
    │2017  │"Medical"  │41       │
    ├──────┼───────────┼─────────┤
    │2017  │"Hospital" │21       │
    ├──────┼───────────┼─────────┤
    │2018  │"PDO Major"│321      │
    ├──────┼───────────┼─────────┤
    │2018  │"PDO Minor"│175      │
    ├──────┼───────────┼─────────┤
    │2018  │"Medical"  │84       │
    ├──────┼───────────┼─────────┤
    │2018  │"Hospital" │25       │
    ├──────┼───────────┼─────────┤
    │2019  │"PDO Major"│292      │
    ├──────┼───────────┼─────────┤
    │2019  │"PDO Minor"│204      │
    ├──────┼───────────┼─────────┤
    │2019  │"Medical"  │52       │
    ├──────┼───────────┼─────────┤
    │2019  │"Hospital" │26       │
    ├──────┼───────────┼─────────┤
    │2020  │"PDO Major"│310      │
    ├──────┼───────────┼─────────┤
    │2020  │"PDO Minor"│234      │
    ├──────┼───────────┼─────────┤
    │2020  │"Medical"  │39       │
    ├──────┼───────────┼─────────┤
    │2020  │"Hospital" │24       │
    ├──────┼───────────┼─────────┤
    │2021  │"PDO Major"│4116     │
    ├──────┼───────────┼─────────┤
    │2021  │"PDO Minor"│2028     │
    ├──────┼───────────┼─────────┤
    │2021  │"Medical"  │1153     │
    ├──────┼───────────┼─────────┤
    │2021  │"Hospital" │404      │
    ├──────┼───────────┼─────────┤
    │2021  │"Fatal"    │25       │
    └──────┴───────────┴─────────┘
    ```

- **Number of crashes of an event type (Involving Overtaking) for each severity** 
    ```
    MATCH (s:Severity)<-[:WAS]-(n:Crash)-[:WAS]->(p:EventType {event_type: "Involving Overtaking"})
    RETURN s.severity AS severity, COUNT(*) AS num_crashes;
    ```

    Response: 
    ```
    ╒═══════════╤═════════════╕
    │"severity" │"num_crashes"│
    ╞═══════════╪═════════════╡
    │"PDO Minor"│292          │
    ├───────────┼─────────────┤
    │"PDO Major"│392          │
    ├───────────┼─────────────┤
    │"Medical"  │51           │
    ├───────────┼─────────────┤
    │"Hospital" │14           │
    ├───────────┼─────────────┤
    │"Fatal"    │1            │
    └───────────┴─────────────┘
    ```

- **How many pedestrians were involving in crashes by LGA?**
    ```
     MATCH (n:Crash)-[r:IN]->(l:LGA)
     WHERE n.total_pedestrians_involve > 0
     RETURN l.local_gov_name as local_gov , sum(n.total_pedestrians_involve) as number_of_crashes_involving_pedestrians
     ORDER BY number_of_crashes_involving_pedestrians DESC
     LIMIT 5;
    ```

    Response: 
    ```
    ╒══════════════╤═════════════════════════════════════════╕
    │"local_gov"   │"number_of_crashes_involving_pedestrians"│
    ╞══════════════╪═════════════════════════════════════════╡
    │"Stirling (C)"│13                                       │
    ├──────────────┼─────────────────────────────────────────┤
    │"Perth (C)"   │12                                       │
    ├──────────────┼─────────────────────────────────────────┤
    │"Wanneroo (C)"│8                                        │
    ├──────────────┼─────────────────────────────────────────┤
    │"Swan (C)"    │8                                        │
    ├──────────────┼─────────────────────────────────────────┤
    │"Vincent (C)" │8                                        │
    └──────────────┴─────────────────────────────────────────┘
    ```


- **How many Vehicles, bikes, Trucks, & pedestrians were involved in accidents by 10 local governments with the hights total number of crashes**

    ```
    MATCH (n:Crash)-[:IN]->(lga:LGA)
    RETURN lga.local_gov_name as LGA ,  
           sum(n.total_other_vehicles_involved) as total_other_vehicles , 
           sum(n.total_heavy_truck_involved) as total_heavy_trucks, 
           sum(n.total_bike_involved) as total_bikes, 
           sum(n.total_motor_cycle_involved) as total_motor_cycles,
           sum(n.total_pedestrians_involve) as total_pedestrians,
           sum(n.total_other_vehicles_involved + 
               n.total_heavy_truck_involved + 
               n.total_bike_involved + 
               n.total_motor_cycle_involved + 
               n.total_pedestrians_involve) as Total
    ORDER BY Total DESC
    Limit 5
    ```

    Response: 
    ```
    ╒══════════════╤══════════════════════╤════════════════════╤═════════════╤════════════════════╤═══════════════════╤═══════╕
    │"LGA"         │"total_other_vehicles"│"total_heavy_trucks"│"total_bikes"│"total_motor_cycles"│"total_pedestrians"│"Total"│
    ╞══════════════╪══════════════════════╪════════════════════╪═════════════╪════════════════════╪═══════════════════╪═══════╡
    │"Stirling (C)"│1205                  │6                   │14           │20                  │13                 │1258   │
    ├──────────────┼──────────────────────┼────────────────────┼─────────────┼────────────────────┼───────────────────┼───────┤
    │"Canning (C)" │625                   │3                   │9            │9                   │7                  │653    │
    ├──────────────┼──────────────────────┼────────────────────┼─────────────┼────────────────────┼───────────────────┼───────┤
    │"Swan (C)"    │582                   │9                   │6            │23                  │8                  │628    │
    ├──────────────┼──────────────────────┼────────────────────┼─────────────┼────────────────────┼───────────────────┼───────┤
    │"Cockburn (C)"│591                   │8                   │2            │13                  │5                  │619    │
    ├──────────────┼──────────────────────┼────────────────────┼─────────────┼────────────────────┼───────────────────┼───────┤
    │"Perth (C)"   │553                   │6                   │16           │17                  │12                 │604    │
    └──────────────┴──────────────────────┴────────────────────┴─────────────┴────────────────────┴───────────────────┴───────┘
    ```

- **Find the 10 closest accidents to the Perth Weather Stn that occurred on rainy days** 

    Using optional match as the station is used later in the query however we do not want to limit the nodes on this connection as not all stations have being applied through the API. 

    ```
    MATCH  (r:WeatherReading)<-[:READING]-(c:Crash)  OPTIONAL MATCH (stn:WeatherStn {station_name: "Mandurah"})
    Where r.precipitation_mm > 0
    WITH c ,
      point({latitude:stn.latitude, longitude:stn.longitude}) AS p1,
      point({latitude:c.latitude, longitude:c.longitude}) AS p2
    RETURN c
    ORDER BY  toInteger(point.distance(p1, p2)/1000) 
    Limit 10
    ```

    Sample Response: 
    
    ```
    ╒══════════════════════════════════════════════╕
    │"c"                                           │
    ╞══════════════════════════════════════════════╡
    │{"total_pedestrians_involve":0,"severity":"PDO│
    │ Major","event_nature":"Hit Object","event_id"│
    │:"129746","latitude":-32.523248,"total_truck_i│
    │nvolved":0,"total_bike_involved":0,"total_heav│
    │y_truck_involved":0,"total_motor_cycle_involve│
    │d":0,"total_other_vehicles_involved":0,"longit│
    │ude":115.729455}                              │
    ├──────────────────────────────────────────────┤
    │{"total_pedestrians_involve":0,"severity":"PDO│
    │ Minor","event_nature":"Hit Object","event_id"│
    │:"129938","latitude":-32.521671,"total_truck_i│
    │nvolved":0,"total_bike_involved":0,"total_heav│
    │y_truck_involved":0,"total_motor_cycle_involve│
    │d":0,"total_other_vehicles_involved":0,"longit│
    │ude":115.724282}                              │
    ├──────────────────────────────────────────────┤
    ```

![image](https://user-images.githubusercontent.com/76982323/177722236-5df4e631-7ffc-4b9e-9f5c-f5b190176f94.png)


