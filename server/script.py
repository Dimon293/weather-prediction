import pandas as pd
import numpy as np
import os
import pickle
# import matplotlib.pyplot as plt
# import math
# from keras.models import Sequential
# from keras.layers import Dense
# from keras.layers import LSTM
# from sklearn.preprocessing import MinMaxScaler
# from sklearn.metrics import mean_squared_error

df = pd.read_csv('./data/tr.csv', delimiter=",")

listLoc = list(df['Location'].unique())

def minMax(column, maximum, minimum):
    df[column] = (df[column]-minimum)/(maximum - minimum)

pd.set_option('display.max_rows', 64)
pd.set_option('display.max_columns', 64)
pd.options.display.float_format = '{:,.3f}'.format

for x in df.columns:
    if x!='Date' and x!='Location':
        minMax(x, df[x].max(), df[x].min())

df['Location'] = pd.Categorical(df['Location'], categories=listLoc)
df = pd.concat([df, pd.get_dummies(df['Location']).astype(np.int8)], axis=1)
# del df['Location']

for x in listLoc:
    temp = df.loc[df['Location']==x]
    del temp['Location']
    filename = './data/cities/' + x +'.pickle'
    if os.path.exists(filename):
        os.remove(filename)
    with open(filename, 'wb') as f:
        pickle.dump(temp, f, protocol=pickle.HIGHEST_PROTOCOL)

