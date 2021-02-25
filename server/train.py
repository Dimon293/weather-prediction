import pandas as pd
import numpy as np
import os
import pickle
from tensorflow.python.keras.models import Sequential
from tensorflow.python.keras.layers import Dense, LSTM, Activation, Dropout, TimeDistributed
import tensorflow as tf
from split import splitData


def build_model(target_features):
    model = Sequential()
    model.add(LSTM(input_shape=(timeStep, n_features), return_sequences=True, units=150))
    model.add(Dropout(0.5))
    model.add(LSTM(250, return_sequences=True))
    model.add(Dropout(0.5))
    model.add(TimeDistributed(Dense(len(target_features))))
    model.add(Activation("relu"))
    model.compile(loss="mse", optimizer="adam", metrics=['accuracy'])
    return model

listCit = os.listdir('./data/cities')
d={}
for x in listCit:
    with open('./data/cities/' + x, 'rb') as f:
        df = pickle.load(f)
        del df["Date"]
        d[x] = df

timeStep = 4
n_features = 69
target_features = ["RainToday", "Temp9am", "Temp3pm"]

model = build_model(target_features)

mean_acc = 0
counter = 0
for city in listCit:
    X_train, Y_train, X_test, Y_test = splitData(d[city], target_features, timeStep)
    model.fit(X_train, Y_train, epochs=10, batch_size=1000, verbose=2, shuffle=False)
    los, acc = model.evaluate(X_test, Y_test)
    print("Accuracy at city" + city + ": " + str(acc * 100) + "%")
    mean_acc += acc
    counter += 1
    print("Complete ["+str(counter)+"/" + str(len(listCit))+"]")
    pred = model.predict(X_test)
    if counter % 10 == 0:
        print(1)

mean_acc = mean_acc/len(listCit)
print("Total accuracy: " + str(mean_acc * 100) + "%")

model.save('lstm.h5')
