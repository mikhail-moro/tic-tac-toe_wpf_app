""""import os
import sys

import numpy as np
from keras.models import load_model


def write_int(i: int):
    stdout.write(i.to_bytes(4, byteorder='little'))


stdin = sys.stdin.buffer
stdout = sys.stdout.buffer

stdin.seek(0, os.SEEK_END)
n = stdin.tell() // 4
observation = [0] * n

for i in range(n):
    observation[i] = int.from_bytes(stdin.read(4), byteorder='little')

input_observation = np.eye(3)[observation].reshape(-1).reshape(1, 1, 27)

model = load_model("tic-tac-toe_model.h5")
raw_predict = model.predict(input_observation)[0]
predict = np.array([raw_predict[i] if observation[i] == 0 else 0 for i in np.arange(9)])

write_int(8)
"""

import os
import sys

import numpy as np
from keras.models import load_model

stdin = sys.stdin.buffer
stdout = sys.stdout.buffer


def get_int_list():
    stdin.seek(0, os.SEEK_END)
    n = stdin.tell() // 4
    arr = [0] * n

    for i in range(n):
        arr[i] = int.from_bytes(stdin.read(4), byteorder='little')

    return arr


def write_int(i: int):
    stdout.write(i.to_bytes(4, byteorder='little'))


observation = get_int_list()
input_observation = np.eye(3)[observation].reshape(-1).reshape(1, 1, 27)
model = load_model("tic-tac-toe_model.h5")

raw_predict = model.predict(input_observation, verbose=None)[0]
predicted = np.array([raw_predict[i] if observation[i] == 0 else 0 for i in np.arange(9)])

write_int(int(predicted.argmax()))
