# Image CLassification & Exploration of Feed Forward Neural Networks


This was a university project that was used to explore Multi-layer Perceptron (MLP) and Convolutional Neural Networks (CNN) for multi class classificaiton. 
To do this, Scikit learn and Keras was used. Both are very intuitive libraries that offer many useful functions. 

Throughout the assignment I set up a series of functions for model setup, evaluation, and utility functions for saving and loading the models. These functions are fairly robust and can be used in a variety of projects however, like anything, this can always be further improved. 

**Note:**   
The task was simplified and the models were not fully explored to reduce the time complexity of the assignment. Further hyperparameters and layer make ups would have been explored further to improve on the solutions. 



# Dataset 

**The CIFAR-10 dataset**


>The CIFAR-10 dataset consists of 60000 32x32 colour images in 10 classes, with 6000 images per class. There are 50000 training images and 10000 test images.
>
>The dataset is divided into five training batches and one test batch, each with 10000 images. The test batch contains exactly 1000 randomly-selected images from each class. The training batches contain the remaining images in random order, but some training batches may contain more images from one class than another. Between them, the training batches contain exactly 5000 images from each class.

**Classes**

>The classes are completely mutually exclusive. There is no overlap between automobiles and trucks. "Automobile" includes sedans, SUVs, things of that sort. "Truck" includes only big trucks. Neither includes pickup trucks.

- airplane
- automobile
- bird
- cat
- dee
- dog
- frog
- horse
- ship
- truck


# Models  

## MLP  

### Loss Function

**Sparse Categorical Cross-Entropy**

In multi-class classification tasks, categorical crossentropy is a loss function where the outcome can only fit into one of many possible categories, and the model must choose one.

### Optimizer

**SGD**

The iterative approach of stochastic gradient descent is used to optimise an objective function with sufficient smoothness criteria. Because it replaces the actual gradient with an estimate, it can be considered a stochastic approximation of gradient descent optimisation.

Stochastic gradient descent with learning rate and momentum as trainable hyperparameters. 

### Hyperparameter 

**NOTE:** 

For runtime speed, the hyperparameters are tuned using a random grid search through the supplied variables. However, the function allows for a full, exhaustive grid search to be performed if required by passing the argument True to the function. 

**Dropout rate:**

Dropout rate is a popular regularization technique used for neural networks. It works were every neurone (including input neurones but excluding output neurones) has a probability p of being momentarily "dropped out" during each training step, meaning it will be completely ignored during current training phase but may be active during the next.

**Learning rate & momentum:**

The learning rate determines how much the weight is updated at the end of each batch, while the momentum determines how much the prior update influences the current update.

**Kernal Initializer:**

Initializers define the way to set the initial random weights of Keras layers.

Here uniform and normal distributions are used to initialize the values.




### Layers 

Flatten: 
- Input shape [32,32,3]

Dropout 

Dense: 
- Shape: 256
- Activation function: Relu

Dropout 

Dense: 
- Shape: 64
- Activation function: Relu

Dropout 

Dense: 
- Shape: 10
- Activation function: Softmax



**Best Parameters from MLP model training**

- momentum: 0.4  
- learn_rate: 0.01  
- init_mode: uniform  
- dropout_rate: 0.0  

### MLP Outcomes 

**Early stopping**  

Early stopping has been added to the full training of the best parameters.
This is not implemented in the cross validation phase of hyperparameter tuning however as this could cause close parameters to stop before their optimal value and issues were comparing between parameters that have not been trained through comparable processes. Cross validation is implicitly base on "all other things being equal" and as such it is best not to use early stopping to keep things as comparable as possible. 


**Cross Validation**

Cross validation has been implemented to ensure that hyperparameter training is done with multiple validations. The CV process has been implemented with a random grid however this can be changed to an exhaustive search by updating the boolean value passed to the function. 

**Validation in Training**

Validation set has been incorporated into the grid search to try to reduce the overfitting of the models. 


![image](https://user-images.githubusercontent.com/76982323/177681264-25a3fbe1-a727-4af2-bfe6-3191df698e96.png)
![image](https://user-images.githubusercontent.com/76982323/177681314-4def3f18-ca6b-419f-bd8a-3d7930726728.png)

![image](https://user-images.githubusercontent.com/76982323/177681346-eb069d53-6ef0-45ec-80d1-79f331c55460.png)



## CNN

### Loss Function

**Sparse Categorical Cross-Entropy**


### Optimizer

**SGD**

Stochastic gradient descent with learning rate and momentum set from MLP training 

### Hyperparameter 

**NOTE:** 

For runtime speed, the hyperparameters are tuned using a random grid search through the supplied variables. However, the function allows for a full, exhaustive grid search to be performed if required by passing the argument True to the function. 


**Number of Kernels**

Number of filter layers. 


**Kernel Size:**
This layer creates a convolution kernel that is convolved with the layer input to produce a tensor of outputs.
Must be an odd number. 

**Activation Function:**
Relu & Sigmoid are tested using the grid search. 


**Dropout rate:**

Set to 0 from MLP model. However this has been left in as the CNN was over-fitting the data and therefore should be reintroduced and trained. 

**Learning rate & momentum:**

The learning rate determines how much the weight is updated at the end of each batch, while the momentum determines how much the prior update influences the current update.

**Kernal Initializer:**

Initializers define the way to set the initial random weights of Keras layers.

Here uniform and normal distributions are used to initialize the values.




### Layers 

Conv2D
- num_kernels: from grid search 
- kernel_size: from grid search 
- activation: from grid search 
- padding="same"
- input_shape=[32, 32, 3]


MaxPooling2D
- pool_size: 2

Conv2D
- num_kernels: from grid search 
- kernel_size: from grid search 
- activation: from grid search 
- padding="same"

MaxPooling2D
- pool_size: 2

Flatten
 
BatchNormalization
 

Dense
- Shape 50, 
- activation='relu'

BatchNormalization

Dense
- Shape 10, 
- activation='softmax'


### Loss Function

**Sparse Categorical Cross-Entropy**


### Optimizer

**SGD**

Stochastic gradient descent with learning rate and momentum set from MLP training 

### Hyperparameter 

**NOTE:** 

For runtime speed, the hyperparameters are tuned using a random grid search through the supplied variables. However, the function allows for a full, exhaustive grid search to be performed if required by passing the argument True to the function. 


**Number of Kernels**

Number of filter layers. 


**Kernel Size:**
This layer creates a convolution kernel that is convolved with the layer input to produce a tensor of outputs.
Must be an odd number. 

**Activation Function:**
Relu & Sigmoid are tested using the grid search. 


**Dropout rate:**

Set to 0 from MLP model. However this has been left in as the CNN was over-fitting the data and therefore should be reintroduced and trained. 

**Learning rate & momentum:**

The learning rate determines how much the weight is updated at the end of each batch, while the momentum determines how much the prior update influences the current update.

**Kernal Initializer:**

Initializers define the way to set the initial random weights of Keras layers.

Here uniform and normal distributions are used to initialize the values.




### Layers 

Conv2D
- num_kernels: from grid search 
- kernel_size: from grid search 
- activation: from grid search 
- padding="same"
- input_shape=[32, 32, 3]


MaxPooling2D
- pool_size: 2

Conv2D
- num_kernels: from grid search 
- kernel_size: from grid search 
- activation: from grid search 
- padding="same"

MaxPooling2D
- pool_size: 2

Flatten
 
BatchNormalization
 

Dense
- Shape 50, 
- activation='relu'

BatchNormalization

Dense
- Shape 10, 
- activation='softmax'



# Model Evaluation and Comparison 

# Evaluate & Compare models 

Compare and comment on your MLP and CNN models on the test set, in terms of: classification
performance (accuracy, F1 score, precision per class), model complexity (e.g., number of trainable
parameters), and computation time4.
For each model, display also a few correctly classified images and a few failure cases for the test
set.

![image](https://user-images.githubusercontent.com/76982323/177681424-ff21b8db-ac63-4272-bed0-e3c5155ca737.png)
![image](https://user-images.githubusercontent.com/76982323/177681442-a1d8a222-95e3-434b-96a3-a773925e2d0f.png)
![image](https://user-images.githubusercontent.com/76982323/177681463-ba8c0bf4-d358-49d8-8a73-7ebbb61476c0.png)









## Overall Model Analysis & Comparison 

| Name | Trainable of Parameters | Time (mins)| Accuracy | Precision |  Recall | F1 |
|:---: |:---: |:---: |:---: |:---: |:---: |:---: |
| MLP | 803,786 | 10 | 55% |55% | 55% | 55% |
| CNN |  1,166,740 | 36 |73% |73% | 73% | 73% |


The CNN model did a better job of fitting to the data than the MLP model, however, the CNN overfit significantly to the training and validation sets with 99.24% and 99.68% accuracy respectively which fell to 72% on the testing set. 

This is not good and further regularisation measures should be introduced such as dropout rate. However the task stated to use the optimal value from the first training so this has been done.

The CNN model trained significantly slower than the MLP model due to an increase in its training and non training parameters. However this allowed it to fit to the more complex function of the data. 


## Class Prediction 

Both models had quite balanced precision and recall scores for the different types of errors. Only class 6, frog, seemed to have a much higher recall than precision in both models. This is due to the model predicting the class of frog for many dogs, cats and deers. This could be due to the heavy green colours in the images. 

### MLP

The MLP model did a okay job of predicting most classes as seen in the confusion matrix. The main issues in classes were the dog & cat, bird & deer, automobile & truck, ship & airplane. 
These are reasonable things that would be difficult for the model to predict.

### CNN

The CNN model did a pretty good job at distinguishing between classes were the MLP model struggled. However some of the still tricky classes caused trouble such as dog & cat, automobile & truck, ship & airplane, and bird & airplane. 




