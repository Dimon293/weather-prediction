import pandas as pd
import numpy as np
import os
import pickle

def splitData(data, target_features, timeStep):
    x = data.drop('RainTomorrow', axis=1)
    y = data[target_features]

    test_len = round(x.shape[0] / 10)
    train_len = round(x.shape[0] - test_len)
    x_train = x.head(train_len)
    x_test = x.tail(test_len)
    del x
    y_train = y.head(train_len)
    y_test = y.tail(test_len)
    del y

    X_train = []
    Y_train = []
    X_test = []
    Y_test = []

    for x in range(timeStep, len(x_train)):
        X_train.append(x_train.iloc[x - timeStep:x, :])
        Y_train.append(y_train.iloc[x - timeStep + 1:x + 1, :])
    X_train = np.stack(X_train, axis=0)
    Y_train = np.stack(Y_train, axis=0)

    for x in range(timeStep, len(x_test)):
        X_test.append(x_test.iloc[x - timeStep:x, :])
        Y_test.append(y_test.iloc[x - timeStep + 1:x + 1, :])
    X_test = np.stack(X_test, axis=0)
    Y_test = np.stack(Y_test, axis=0)

    return X_train, Y_train, X_test, Y_test
