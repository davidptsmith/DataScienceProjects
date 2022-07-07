# Text Classification with Pytorch: MLP, CNN, RNN, GRU, LSTM 

**Note:**

Only a subset of the data was used to train the various models due to the short timeframe. For better predictions this should be increased to the full dataset. 


## Data: 

The data provided is from the the job searching website Seek.com where many companies post looking for future employees. 

The csv for Australian job listings from Seek job board has 30,000 observations and 12 variables. 

- category	
- city	
- company_name	
- geo	
- job_board	
- job_description	
- job_title	
- job_type	
- post_date	
- salary_offered	
- state	
- url

Many of these have issues in the data such as character artifacts from text encoding and missing data. 

Job description field is the variable of interest, however, giving the various ways in which people write job descriptions there are many issues in this column. 
Luckily, where the job description lacks the information, the structured fields can work to combine the information and make it usable. 




## File Structure 

Data: 
- Holds the raw data, embeddings, & exported csv files for the different cases 

Model Storage 
- Contains a folder for each model. This folder contains a subfolder for each data type exported containing the vectors, embeddings and final the model.


## TF-IDF Implemenetaiton 

Document Frequency (dft) is defined as the number of documents in the collection that contain a term.

Inverse Document Frequency (IDF) gives a bonus to words that appear frequently in a document but not across the full document collection. 
Essentially, a higher document frequency works against a word's rating.

The TF-IDF allows as to remove stop words and make similarity comparisons between documents. This can be used in ranking the users outputs or in classifying a piece of text across different categories. 

### Remove Stop Words & Place in Column

I am using the TF-IDF process to remove stop words and words that provide little meaning to text differentiation. The remaining words are then stored in a column within the dataframe. 


# Exploratory Data Analysis 




## Class balance 

There is quite a significant class imbalance in the job categories. For accurate modeling, this issue must be taken into account. 

It would probably be best practice to group some of these jobs into aggregate categories to ensure they are represented in the training data. Though this will only work well if they are similar and use similar language otherwise it will lead to increased variation in the classes and lead to poor modeling. 


![output](https://user-images.githubusercontent.com/76982323/177726452-1ed0dbde-3fae-4704-95ec-c675af2b2bfb.png)

![image](https://user-images.githubusercontent.com/76982323/177726511-cc1da719-b11f-4b24-9b85-8013ce75b1eb.png)


# Binary Classification 

## Select Model & Data Type

Use the values below to select what model, data type and word embeddings you wish to use for the modeling phase. 

Note: Lists indexes are 0 based.

For Example: 

Selecting:
- model 1, data_index 1, & embedding index 2 

Would return: 
- 'GRU' for the top ten 'filtered words' using the domain trained 'Word2Vector' embeddings 


## MLP 

----------------------------------------------------------------------------------------------------
                                 Model Outcomes: MLP filterd_words
---------------------------------------------------------------------------------------------------- 

MLP  Accuracy Score:  55.48%
MLP  Precision Score:  61.94%
MLP  Recall Score:  55.48%
MLP  F1 Score:  57.01%
----------------------------------------------------------------------------------------------------
                                       Classification Report
---------------------------------------------------------------------------------------------------- 

              precision    recall  f1-score   support

           0       0.37      0.58      0.45       313
           1       0.73      0.54      0.63       673

    accuracy                           0.55       986
   macro avg       0.55      0.56      0.54       986
weighted avg       0.62      0.55      0.57       986

----------------------------------------------------------------------------------------------------
                                          Confusion Matrix
---------------------------------------------------------------------------------------------------- 

![image](https://user-images.githubusercontent.com/76982323/177726965-cf67c640-9e58-4a95-abfc-3e44232d41a0.png)

## CNN


----------------------------------------------------------------------------------------------------
                                 Model Outcomes: CNN filterd_words
---------------------------------------------------------------------------------------------------- 

CNN  Accuracy Score:  59.33%
CNN  Precision Score:  63.00%
CNN  Recall Score:  59.33%
CNN  F1 Score:  60.53%
----------------------------------------------------------------------------------------------------
                                       Classification Report
---------------------------------------------------------------------------------------------------- 

              precision    recall  f1-score   support

           0       0.39      0.53      0.45       313
           1       0.74      0.62      0.68       673

    accuracy                           0.59       986
   macro avg       0.57      0.58      0.56       986
weighted avg       0.63      0.59      0.61       986

----------------------------------------------------------------------------------------------------
                                          Confusion Matrix
---------------------------------------------------------------------------------------------------- 
![image](https://user-images.githubusercontent.com/76982323/177727305-094379a6-a2e4-4861-ace6-bc45393b6569.png)


## Most Influential Words
----------------------------------------------------------------------------------------------------
                    CNN Full Description Influential Words in Positive Reviews:
---------------------------------------------------------------------------------------------------- 

so
support
established
looking
must
related
based
role
preparation
market
----------------------------------------------------------------------------------------------------
                    CNN Full Description Influential Words in Negative Reviews:
---------------------------------------------------------------------------------------------------- 

so
support
established
looking
must
related
based
role
preparation
market


# Multi Class Classification 

##  RNN: Top 10 Filtered TF-IDF Classification  

----------------------------------------------------------------------------------------------------
                                 Model Outcomes: RNN filterd_words
---------------------------------------------------------------------------------------------------- 

RNN  Accuracy Score:  5.27%
RNN  Precision Score:  6.92%
RNN  Recall Score:  5.27%
RNN  F1 Score:  5.33%
----------------------------------------------------------------------------------------------------
                                       Classification Report
---------------------------------------------------------------------------------------------------- 

              precision    recall  f1-score   support

           0       0.06      0.03      0.04        32
           1       0.43      0.31      0.36        61
           2       0.12      0.03      0.05       106
           3       0.10      0.06      0.08        81
           4       0.06      0.05      0.05        44
           5       0.00      0.00      0.00        20
           6       0.00      0.00      0.00        90
           7       0.00      0.00      0.00        89
           8       0.00      0.00      0.00        27
           9       0.00      0.00      0.00        32
          10       0.01      0.33      0.03         6
          11       0.00      0.00      0.00        29
          12       0.02      0.03      0.03        29
          13       0.05      0.02      0.02        66
          14       0.00      0.00      0.00        22
          15       0.00      0.00      0.00        19
          16       0.12      0.14      0.13        42
          17       0.00      0.00      0.00        59
          18       0.02      0.04      0.03        26
          19       0.03      0.10      0.05        21
          20       0.00      0.00      0.00         6
          21       0.00      0.00      0.00        13
          22       0.12      0.11      0.12        44
          23       0.00      0.00      0.00         1
          24       0.04      0.25      0.07         4
          25       0.00      0.00      0.00         4
          26       0.02      0.17      0.03         6
          27       0.14      1.00      0.25         2
          28       0.00      0.00      0.00         1
          29       0.00      0.00      0.00         4

    accuracy                           0.05       986
   macro avg       0.05      0.09      0.04       986
weighted avg       0.07      0.05      0.05       986


----------------------------------------------------------------------------------------------------
                                          Confusion Matrix
---------------------------------------------------------------------------------------------------- 

![output](https://user-images.githubusercontent.com/76982323/177727941-0665008e-0512-4d08-87a9-b9ed9ac5f7c9.png)


## GRU: Top 10 Filtered TF-IDF Classification 

----------------------------------------------------------------------------------------------------
                                 Model Outcomes: GRU filterd_words
---------------------------------------------------------------------------------------------------- 

GRU  Accuracy Score:  6.59%
GRU  Precision Score:  11.69%
GRU  Recall Score:  6.59%
GRU  F1 Score:  6.92%
----------------------------------------------------------------------------------------------------
                                       Classification Report
---------------------------------------------------------------------------------------------------- 

              precision    recall  f1-score   support

           0       0.17      0.03      0.05        32
           1       0.88      0.23      0.36        61
           2       0.15      0.10      0.12       106
           3       0.17      0.14      0.15        81
           4       0.00      0.00      0.00        44
           5       0.04      0.25      0.07        20
           6       0.00      0.00      0.00        90
           7       0.14      0.02      0.04        89
           8       0.00      0.00      0.00        27
           9       0.02      0.03      0.02        32
          10       0.01      0.17      0.01         6
          11       0.00      0.00      0.00        29
          12       0.06      0.14      0.09        29
          13       0.04      0.02      0.02        66
          14       0.00      0.00      0.00        22
          15       0.00      0.00      0.00        19
          16       0.10      0.24      0.14        42
          17       0.00      0.00      0.00        59
          18       0.09      0.04      0.05        26
          19       0.03      0.10      0.05        21
          20       0.00      0.00      0.00         6
          21       0.00      0.00      0.00        13
          22       0.03      0.02      0.03        44
          23       0.00      0.00      0.00         1
          24       0.00      0.00      0.00         4
          25       0.00      0.00      0.00         4
          26       0.00      0.00      0.00         6
          27       0.00      0.00      0.00         2
          28       0.00      0.00      0.00         1
          29       0.00      0.00      0.00         4

    accuracy                           0.07       986
   macro avg       0.06      0.05      0.04       986
weighted avg       0.12      0.07      0.07       986

----------------------------------------------------------------------------------------------------
                                          Confusion Matrix
---------------------------------------------------------------------------------------------------- 
![output](https://user-images.githubusercontent.com/76982323/177728087-11d926ea-66f8-4e4a-a127-773cb7e09818.png)

## LSTM: Top 10 Filtered TF-IDF Classification 

----------------------------------------------------------------------------------------------------
                                 Model Outcomes: LSTM filterd_words
---------------------------------------------------------------------------------------------------- 

LSTM  Accuracy Score:  7.10%
LSTM  Precision Score:  9.27%
LSTM  Recall Score:  7.10%
LSTM  F1 Score:  7.46%
----------------------------------------------------------------------------------------------------
                                       Classification Report
---------------------------------------------------------------------------------------------------- 

              precision    recall  f1-score   support

           0       0.17      0.03      0.05        32
           1       0.49      0.41      0.45        61
           2       0.11      0.06      0.07       106
           3       0.14      0.12      0.13        81
           4       0.00      0.00      0.00        44
           5       0.11      0.15      0.12        20
           6       0.00      0.00      0.00        90
           7       0.12      0.02      0.04        89
           8       0.00      0.00      0.00        27
           9       0.05      0.09      0.07        32
          10       0.00      0.00      0.00         6
          11       0.03      0.03      0.03        29
          12       0.10      0.14      0.12        29
          13       0.00      0.00      0.00        66
          14       0.00      0.00      0.00        22
          15       0.00      0.00      0.00        19
          16       0.07      0.07      0.07        42
          17       0.05      0.02      0.03        59
          18       0.00      0.00      0.00        26
          19       0.01      0.05      0.02        21
          20       0.00      0.00      0.00         6
          21       0.03      0.08      0.04        13
          22       0.19      0.18      0.18        44
          23       0.00      0.00      0.00         1
          24       0.02      0.25      0.04         4
          25       0.00      0.00      0.00         4
          26       0.00      0.00      0.00         6
          27       0.00      0.00      0.00         2
          28       0.00      0.00      0.00         1
          29       0.00      0.00      0.00         4

    accuracy                           0.07       986
   macro avg       0.06      0.06      0.05       986
weighted avg       0.09      0.07      0.07       986

----------------------------------------------------------------------------------------------------
                                          Confusion Matrix
---------------------------------------------------------------------------------------------------- 

![output](https://user-images.githubusercontent.com/76982323/177728202-bad67fda-305a-47d0-95fb-688b328913cd.png)


