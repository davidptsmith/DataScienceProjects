# Ensemble Learning with SK-Learn 

## Concrete Strength Test


Data Set Information:

The data set includes 103 data points. There are 7 input variables, and 3 output variables in the data set.
The initial data set included 78 data. After several years, we got 25 new data points.


Attribute Information:

Input variables (7)(component kg in one M^3 concrete):
- Cement
- Slag
- Fly ash
- Water
- SP
- Coarse Aggr.
- Fine Aggr.

Output variables (3):
- SLUMP (cm)
- FLOW (cm)
- 28-day Compressive Strength (Mpa)

The focus of this analysis will be predicting the 28-day Compressive Strength (Mpa) of concrete by the input variables.



## Preliminary Data Exploration 

![image](https://user-images.githubusercontent.com/76982323/177712010-f67983c7-99ee-4d0e-8133-fc33a1d78bcc.png)
![image](https://user-images.githubusercontent.com/76982323/177712115-91ad80f9-f994-477b-a956-3380ad891dfb.png)
![image](https://user-images.githubusercontent.com/76982323/177712170-7cf4aa04-7439-416c-b6d8-2d77144a2e88.png)


## Modeling

Three different models were fit to a voting regression model, a LinearSVR, LinearRegression, & SGDRegressor. Key hyperparameters for each were explored using a grid search.  

Each individual model was then testing and compared based on loss functions. 

![image](https://user-images.githubusercontent.com/76982323/177712968-855e2339-333e-491f-8c25-290fcb6b2452.png)

![image](https://user-images.githubusercontent.com/76982323/177713035-0230cda7-5b52-4802-aee6-7f3745b173c1.png)


# Abalone Data

Predict the age of abalone from physical measurements

### Attribute Information:

Given is the attribute name, attribute type, the measurement unit and a brief description. The number of rings is the value to predict: either as a continuous value or as a classification problem.

Name / Data Type / Measurement Unit / Description  
Sex / nominal / -- / M, F, and I (infant)  
Length / continuous / mm / Longest shell measurement  
Diameter / continuous / mm / perpendicular to length  
Height / continuous / mm / with meat in shell  
Whole weight / continuous / grams / whole abalone  
Shucked weight / continuous / grams / weight of meat  
Viscera weight / continuous / grams / gut weight (after bleeding)  
Shell weight / continuous / grams / after being dried  
Rings / integer / -- / +1.5 gives the age in years  

### Data Set Information:

Predicting the age of abalone from physical measurements. The age of abalone is determined by cutting the shell through the cone, staining it, and counting the number of rings through a microscope -- a boring and time-consuming task. Other measurements, which are easier to obtain, are used to predict the age. Further information, such as weather patterns and location (hence food availability) may be required to solve the problem.

From the original data examples with missing values were removed (the majority having the predicted value missing), and the ranges of the continuous values have been scaled for use with an ANN (by dividing by 200).

# Modeling 

## Random Forest 

Implement a Random Forest regressor with 500 estimators. You can manually experiment with various hyperparameters such as min samples leaf, max features, max samples, bootstrap, etc. Train your Random Forest regressor on the training set and test it on the test set. Report its RMSE for the predictions on the test set. Note: as the ring values must be integers, the predicted results from your Random Forest regressor must be firstly rounded to the nearest integer before the RMSE computation.

## Repeated Modeling on Reduced Variables based on feature importance

Repeat the training and prediction processes above on the reduced-dimensional data. We expect to see a slight increase of RMSE for the reduced-dimensional data. In many real applications, the feature dimension of the data may be reduced drastically with only a slight increase in the prediction error. We won’t see this in this small dataset as the feature dimension is already quite small.


![image](https://user-images.githubusercontent.com/76982323/177713530-17d08486-f6aa-4c0c-9887-8f448889014b.png)


## Bagged Regressor 
Models with high variance include decision trees. It implies that a small modification to the training set of data will result in a completely different model. Typically, decision trees overfit. Using the ensemble approach known as bagging, this problem may be solved. Bagging means Bootstrap Aggregation.
Bagging refers to creating different models using a sample of the data, then aggregating the predictions from each model to lower variance.

## Comparison  
![image](https://user-images.githubusercontent.com/76982323/177714024-b04fb6a8-b417-4269-9bba-980697f807f7.png)

![image](https://user-images.githubusercontent.com/76982323/177713677-4dd31a8a-4c2c-4b0c-b08c-feb412b34b36.png)
![image](https://user-images.githubusercontent.com/76982323/177713732-efbf8049-2e22-4374-88c0-cbe0662f3ebc.png)





