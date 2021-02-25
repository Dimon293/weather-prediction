from tensorflow.python.keras.models import load_model
from tensorflow.python.keras import backend as K
import tensorflow as tf
import pandas as pd
import numpy as np
import math
import traceback
from Field import Field

import tornado.httpserver
import tornado.ioloop
import tornado.options
import tornado.web
import json
from tornado.options import define, options

define("port", default=5000, help="run on the given port", type=int)


model = None
graph = None

# Конфигурационный файл
f = open("./data/result.txt", "r")
collums = []
for x in f:
    col = Field(x)
    if col.type == "disc":
        col.categories = list(col.map.values())
#        col.map = {str(k): str(v) for k, v in col.map.items()}
    collums.append(col)
f.close()

def accuracy(y_true, y_pred):
    auc = tf.metrics.Accuracy(y_true, y_pred)[1]
    K.get_session().run(tf.local_variables_initializer())
    return auc

# Загрузка модели
def load_my_model():
    global model
    model = load_model('lstm.h5')
    # global graph
    # graph = tf.get_default_graph()

# Подготовкка данных
def prepare_data(dic):
    if len(dic) != 21:
        raise Exception("Not enough columns")

    for col in collums:
        if col.type == "disc":
            dic[col.name] = col.map[dic[col.name]]
            # print(col.name)
            # print(dic[col.name])
        else:
            print(dic[col.name])
            print(col.name)
            value = float(dic[col.name])
            if value > col.max:
                value = col.max
            elif value < col.min:
                value = col.min
            dic[col.name] = str((value - col.min) / (col.max - col.min))
    df = pd.DataFrame(data=dic, index=[0])
    x='Location'
    # print(x)
    for col in collums:
        if col.name == x:
            df[x] = pd.Categorical(df[x], categories=col.categories)
            # print(df.info())
            df = pd.concat([df, pd.get_dummies(df[x]).astype(np.int8)], axis=1)
            del df[x]
    return df.values

class MainHandler(tornado.web.RequestHandler):
    def get(self):
        self.write("Hello, world")


# POST
class PredictionHandler(tornado.web.RequestHandler):
    def post(self):
        jsstr = json.loads(self.request.body)
        response = {'success': False}
        print(jsstr)
        # если строка пустая, то отправляем ошибку
        if not jsstr:
            response = {
                'reason': 'Empty data'
            }
            self.write(response)
            return
        jsstr = json.loads(jsstr)
        data = jsstr["days"]

        listDays = []

        for i in range(0, len(data)):
            listDays.append(prepare_data(data[i]))
        listDaysNormalize = np.vstack(listDays)
        # предсказываем риск выпадения дождя и максимальную температуру воздуха
        try:
            #with graph.as_default():
            X_train = []

            X_train.append(listDaysNormalize)

            listDaysNormalize = np.stack(X_train, axis=0)
            result = model.predict(listDaysNormalize)
            response["RainToday"] = "{0:.3f}%".format(result[0, 3, 0] * 100.0)
            response["MinTemp"] = "{0:.3f}".format(result[0, 3, 1] * (31.4 + 7.06) - 7.06)
            response["MaxTemp"] = "{0:.3f}".format(result[0, 3, 1] * (44.46 - 2.17) + 2.17)
            response["success"] = True

        except Exception:
            traceback.print_exc()
            response["reason"] = "Wrong data"
        finally:
            print(response)
            self.write(response)


def main():
    load_my_model()
    tornado.options.parse_command_line()
    application = tornado.web.Application([
        (r"/", MainHandler),
        (r"/predict", PredictionHandler),
    ])
    http_server = tornado.httpserver.HTTPServer(application)
    http_server.listen(options.port)
    tornado.ioloop.IOLoop.instance().start()


if __name__ == "__main__":
    main()
