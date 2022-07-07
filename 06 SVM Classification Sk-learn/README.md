#  Classification of Remote Sensing Data in Japanese Forests

## Introduction 

> Multi-temporal remote sensing data of a forested area in Japan. The goal is to map different forest types using spectral data.
> Data Set Information:
>
> This data set contains training and testing data from a remote sensing study which mapped different forest types based on their spectral characteristics at visible-to-near infrared wavelengths, using ASTER satellite imagery. The output (forest type map) can be used to identify and/or quantify the ecosystem services (e.g. carbon storage, erosion protection) provided by the forest.
>
> **Attribute Information:**
> - `Class`: 's' ('Sugi' forest), 'h' ('Hinoki' forest), 'd' ('Mixed deciduous' forest), 'o' ('Other' non-forest land)
> - `b1 - b9`: ASTER image bands containing spectral information in the green, red, and near infrared wavelengths for three dates (Sept. 26, 2010; March 19, 2011; May 08, 2011.
> - 1pred_minus_obs_S_b1` - pred_minus_obs_S_b9: Predicted spectral values (based on spatial interpolation) minus actual spectral values for the 's' class (b1-b9).
> - `pred_minus_obs_H_b1` - pred_minus_obs_H_b9: Predicted spectral values (based on spatial interpolation) minus actual spectral values for the 'h' class (b1-b9). <sup><a href="#1" >1</a></sup>

### Review training and testing data structure 

As the data set has been already split into testing and training, it is important to review the proportion of each class to ensure there is a sufficient number of observations in each set. 

Here the testing set is quite large at approximately 38% of the total number of observations. It may have been better to reduce this proportion to give the training algorithms more data to work with, however for this assignment the training - test split will remain as is. 

### Review the Class Balance 

In the plot below we see that the classes are not balanced which may cause issues in classification tasks. As such, stratified sampling may need to be used to ensure that samples are representative. With class 's' ('Sugi' forest) representing 42% of all data and class 'd'('Mixed deciduous' forest) representing 32%, and the 'o' ('Other' non-forest land) and 'h' ('Hinoki' forest) at 14% and 12% respectively, it might be useful to combine classes when modeling. This will depend however on what the goals of process are. However, given the data is mapping the spectral characteristics at visible-to-near infrared wavelengths, using ASTER satellite imagery to identify and/or quantify the ecosystem services provided by each forrest type, this may not be appropriate. 



## Support Vector Machine Classifier implemented in the sklearn.svm.SVC class to perform multiclass classification using the one-versus-one strategy. 

> *You should look at the Scikit-learn API for this class and experiment with two hyperparameters. You should use grid search and 3-fold cross validation to find the optimal values for these two hyperparameters that maximise the classification accuracy.*
>
> *For other hyperparameters, you can manually set them to some reasonable values. Apart from the Python code, you should explain what you carried out in markdown cells, e.g., which two hyperparameters you have tried? what combination of the hyperparameter values gave the highest classification accuracy?*


#### What are Support Vector Machines 

Support Vector Machine (SVM) is a machine learning model that is capable of both linear and non-linear classification, regression, and outlier detection. 

It is a model that works best on complex small or medium sized datasets. 


>The **advantages** of support vector machines are:
>- Effective in high dimensional spaces.  
>- Still effective in cases where number of dimensions is greater than the number of samples.  
>- Uses a subset of training points in the decision function (called support vectors), so it is >also memory efficient.  
>- Versatile: different Kernel functions can be specified for the decision function. Common kernels are provided, but it is also possible to specify custom kernels.
>
>The **disadvantages** of support vector machines include:  
>- If the number of features is much greater than the number of samples, avoid over-fitting in choosing Kernel functions and regularization term is crucial.  
>- SVMs do not directly provide probability estimates, these are calculated using an expensive five-fold cross-validation (see Scores and probabilities, below). <sup><a href="#2" >2</a></sup>

#### Implementation 

For classification task there are two ways of running the algorithm - one vs one, and one vs rest. For this question the former will be implemented. There are two ways of implementing this in Sklearn as seen below. In this case the second implementation will be used as its seems to be cleaner to read. 
```
# Set process In Using Function 
from sklearn.multiclass import OneVsRestClassifier, OneVsOne

ovo_clf = OneVsRestClassifier(SVC(...))
ovr_clf = OneVsRestClassifier(SVC(...))

## OR...

# Set process In SVC Constructor
ovo_clf = SVC( decision_function_shape='ovo'  , ...)
ovr_clf = SVC( decision_function_shape='ovr'  , ...)

```

#### Hyperparameters Used & Considered 

There are many hyperparameters that can be tuned when fitting a support vector machine. The variables following have been fit using a cross validation grid search, the explanation of each has been taken from the documentation. 


>Cfloat, default=1.0
>- Regularization parameter. The strength of the regularization is inversely proportional to C. Must be strictly positive. The penalty is a squared l2 penalty.
>
>kernel{‘linear’, ‘poly’, ‘rbf’, ‘sigmoid’, ‘precomputed’} or callable, default=’rbf’
>- Specifies the kernel type to be used in the algorithm. If none is given, ‘rbf’ will be used. If a callable is given it is used to pre-compute the kernel matrix from data matrices; that matrix should be an array of shape (n_samples, n_samples).
>
>degreeint, default=3
>- Degree of the polynomial kernel function (‘poly’). Ignored by all other kernels.
>
>gamma{‘scale’, ‘auto’} or float, default=’scale’
>Kernel coefficient for ‘rbf’, ‘poly’ and ‘sigmoid’.
>- if gamma='scale' (default) is passed then it uses 1 / (n_features * X.var()) as value of gamma,
>- if ‘auto’, uses 1 / n_features.
>
>decision_function_shape{‘ovo’, ‘ovr’}, default=’ovr’  
>- Whether to return a one-vs-rest (‘ovr’) decision function of shape (n_samples, n_classes) as all other classifiers, or the original one-vs-one (‘ovo’) decision function of libsvm which has shape (n_samples, n_classes * (n_classes - 1) / 2). However, one-vs-one (‘ovo’) is always used as multi-class strategy. The parameter is ignored for binary classification. <sup><a href="#3" >3</a></sup>

#### Grid Search 
A grid search is an exhaustive search over a set of specified parameter values for estimators. 
This has been implemented using the cross fold validation that allows for the data to be partitioned into testing and training chunks to evaluate the performance of each parameter pairing by a given scoring function. 

A grid search has been asked for by the question, however a Stratified K-fold grid search might be more appropriate here due to the imbalance in the class labels. This would ensure that each fold is representative. 


#### Process & Outcomes 

Best Hyperparameter Combinations: 
- 'C': 1
- 'gamma': 0.1
- 'kernel': 'rbf'

The best model has an accuracy of 91% when applied to the training set. The full classifications report is shown below. This looks good however the polynomial model does have a tendency to over fit to the training data. This will be further explored in the model evaluation set. Model evaluation on the testing set is not run here as it could bias the way other models are explored.  

Training Classification Report: 
```
               precision    recall  f1-score   support

           d       0.92      0.92      0.92       105
           h       0.86      0.79      0.82        38
           o       0.97      0.83      0.89        46
           s       0.89      0.96      0.92       136

    accuracy                           0.91       325
   macro avg       0.91      0.87      0.89       325
weighted avg       0.91      0.91      0.91       325

```




## Stochastic Gradient Descent Classifier from the SGDClassifier class.
> *As the one-versus-one computation is not implemented in SGDClassifier, you can use the default oneversus-all strategy for this classifier.*

<br/>

Stochastic Gradient Descent is simply an optimization technique that does not correspond to a family of machine learning models. It is a simple approach to efficiently fit linear classifiers and regressors under convex loss functions such as Support Vector Machines and Logistic Regression. 

The gradient of the loss function estimate for is computed over a series of samples with the goal of decreasing the loss at each step by the learning rate and updating the model parameters. By running in 'mini batches' the SGD method enables online/ out of core learning by partially fitting the data. This makes this method work well with very large datasets that cannot be loaded into memory. 


>
>The **advantages** of Stochastic Gradient Descent are:
>- Efficiency.
>- Ease of implementation (lots of opportunities for code tuning).
>
>The **disadvantages** of Stochastic Gradient Descent include:
>- SGD requires a number of hyperparameters such as the regularization parameter and the number of iterations.
>- SGD is sensitive to feature scaling.<sup><a href="#4" >4</a></sup>


#### Hyperparameters Used & Considered

There are many hyperparameters that can be tuned for the Stochastic Gradient Descent classifier. The following variable have been fit using a cross validation grid search, the explanation of each has been taken from the documentation. 


>l1_ratiofloat, default=0.15
>- The Elastic Net mixing parameter, with 0 <= l1_ratio <= 1. l1_ratio=0 corresponds to L2 penalty, l1_ratio=1 to L1. Only used if penalty is ‘elasticnet’.
>
>alphafloat, default=0.0001
>- Constant that multiplies the regularization term. The higher the value, the stronger the regularization. Also used to compute the learning rate when set to learning_rate is set to ‘optimal’.
>
>learning_ratestr, default=’optimal’
>- The learning rate schedule:
>- ‘constant’: eta = eta0
>-‘optimal’: eta = 1.0 / (alpha * (t + t0)) where t0 is chosen by a heuristic proposed by Leon Bottou.
>-‘invscaling’: eta = eta0 / pow(t, power_t)
>-‘adaptive’: eta = eta0, as long as the training keeps decreasing. Each time n_iter_no_change consecutive epochs fail to decrease the training loss by tol or fail to increase validation score by tol if early_stopping is True, the current learning rate is divided by 5.
>
>averagebool or int, default=False
>- When set to True, computes the averaged SGD weights across all updates and stores the result in the coef_ attribute. If set to an int greater than 1, averaging will begin once the total number of samples seen reaches average. So average=10 will begin averaging after seeing 10 samples.<sup><a href="#5" >5</a></sup>

### Process & Outcomes 

Best Hyperparameter Combinations: 
 - 'alpha': 0.01
 - 'average': True 
 - 'l1_ratio': 0.0

The best model has an accuracy of 86% when applied to the training set. The full classifications report is shown below. The classification looks to be doing quite well given the high precision, recall, f1 and accuracy scores. 

Training Classification Report: 
```
               precision    recall  f1-score   support

           d       0.86      0.87      0.86       105
           h       0.83      0.76      0.79        38
           o       0.97      0.70      0.81        46
           s       0.85      0.94      0.89       136

    accuracy                           0.86       325
   macro avg       0.88      0.82      0.84       325
weighted avg       0.87      0.86      0.86       325


```

## Compare the performances of the two classifiers and give a brief discussion about your experimental results.
> *You should show the confusion matrices and accuracies of the two classifiers for the testing set.*

The confusion matrix give insights into the areas where the model is having difficult between distinguishing between classes. Both models seem to be having difficulty classifying 'H' by classifying it as 'O'. This could be due to a lack of training data to properly define 'O' class as this only had 38 observations. 

The two models perform quite closely with regards to accuracy on the training data. The overall accuracy between the two models is relatively similar with the SVM at 84% and the SGD at 83%. 

The SVM however has higher precision, recall and F1 scores of 87%, 84%, 83% respectively, as opposed ot the SGD which had 86%, 82%, and 81%. However, the SVM had a large fall off in accuracy from 91% to 84% accuracy from the training to the testing dataset. This is evidence that the data has been overfit to the training set which may mean it wont generalize as well to new data. 

Given the higher metrics on the testing set across the categories, the best model is the SVM.

<br/>

---

<br/>

Note: the calculation of the individual scores for the model are based on the `weighted` method due to the imbalance of the classes within the dataset. This is described in the documentation as: 
> ``'weighted'``:
        Calculate metrics for each label, and find their average weighted
        by support (the number of true instances for each label). This
        alters 'macro' to account for label imbalance; it can result in an
        F-score that is not between precision and recall.

