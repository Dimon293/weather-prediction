import pandas as pd
import numpy as np
import pickle
import os

pd.set_option('display.max_rows', 64)
pd.set_option('display.max_columns', 64)

# загружаем данные и объединяем
df = pd.read_csv('./data/tr.csv', delimiter=",")
del df['Date']
result = ""
# формируем конфиг для всех параметров
for c in df.columns:
    print(c)
    if c=='RainTomorrow' or c=='Date':
        continue
    # дискретные параметры one hot
    if c == 'Location':
        unique = df[c].unique().tolist()
        unique = {k: v for v, k in enumerate(unique)}
        result += "{0};{1};top:{2};{3}\n".format("disc", c, df[c].mode()[0], str(unique))
    # флаговые параметры
    elif c == 'RainToday':
        unique = df[c].unique().tolist()
        zero = 0.0 in unique
        result += "{0};{1};max:{2};min:{3}\n".format("bol", c, str(df[c].max()), str(df[c].min()))
    # непрерывные параметры
    else:
        temp = df[c].astype(np.float32)
        result += "{0};{1};max:{2};min:{3}\n".format("cont", c, str(temp.max()), str(temp.min()))
        del temp


# print(result)
# записываем конфиг в файл
filename = './data/result.txt'
if os.path.exists(filename):
    os.remove(filename)
file = open(filename, "w")
file.write(result)
file.close()

# for c in small.columns:
#     print("{0} nan's: {1}".format(c, small[c].isnull().sum().sum()))

# for col in small.columns:
#     # print(col)
#     if col in droppable_features:
#         print("del " + col)
#         del small[col]
#
# print("=============DESCRIBE=============")
# data_describe = small.describe(include='all')
# for c in small.columns:
#     if c in categories:
#         print(c)
#         small[c] = small[c].fillna(data_describe[c]['top'])
#
# print("=============continuous=============")
# for c in small.columns:
#     if c in continuous:
#         print(c)
#         small[c] = small[c].fillna(small.median(axis=0), axis=0)
#

# print("======================")
# print(small.count(axis=0))
# print(small.head())

# pd.set_option('display.max_rows', 500)

# Корреляция между параметрами
# cols = ['Census_OSBuildNumber', 'OsBuild', 'Census_InternalPrimaryDisplayResolutionHorizontal',
#         'Census_InternalPrimaryDisplayResolutionVertical', 'Census_ProcessorModelIdentifier',
#         'Census_ProcessorManufacturerIdentifier']
#
#
# plt.figure(figsize=(7, 7))
# co_cols = cols
    # co_cols.append('HasDetections')
# sns.heatmap(small[co_cols].corr(), cmap='RdBu_r', annot=True, center=0.0)
# plt.title('Корреляция между параметрами')
# plt.show()


# процентное соотношение пропущенных значений
# print((small.isnull().sum()/small.shape[0]).sort_values(ascending=False))


# pd.options.display.float_format = '{:,.3f}'.format
# sk = pd.DataFrame([{'колонка': c, 'уникальных': small[c].nunique(), 'процент': small[c].value_counts(normalize=True).values[0] * 100} for c in small.columns])
# sk = sk.sort_values('процент', ascending=False)
# print(sk)

# sns.countplot(x='OsVer', hue='HasDetections', data=small)
# plt.show()

#   sns.countplot(x=col, hue='HasDetections',
#                   order=small.Census_PrimaryDiskTotalCapacity.value_counts().iloc[:7].index, data=small)

# df = small.groupby(['OsVer', 'HasDetections'])['OsVer'].count()
# df_1 = df.groupby(level=0).apply(lambda x: 100 * x / float(x.sum())).unstack().fillna(0)
# print(df_1)
# df_1.plot.line()
# df_1.plot.bar()
# plt.show()


# cols = ['LocaleEnglishNameIdentifier', 'Platform', 'Processor', 'OsVer', 'OsBuild', 'OsSuite', 'OsPlatformSubRelease', 'OsBuildLab', 'SkuEdition', 'PuaMode', 'IeVerIdentifier', 'SmartScreen', 'UacLuaenable', 'Census_MDC2FormFactor', 'Census_DeviceFamily', 'Census_OEMNameIdentifier', 'Census_OEMModelIdentifier', 'Census_ProcessorManufacturerIdentifier', 'Census_ProcessorModelIdentifier', 'Census_ProcessorClass', 'Census_PrimaryDiskTypeName', 'Census_ChassisTypeName', 'Census_PowerPlatformRoleName', 'Census_InternalBatteryType', 'Census_OSVersion', 'Census_OSArchitecture', 'Census_OSBranch', 'Census_OSBuildNumber', 'Census_OSBuildRevision', 'Census_OSEdition', 'Census_OSSkuName', 'Census_OSInstallTypeName', 'Census_OSInstallLanguageIdentifier', 'Census_OSUILocaleIdentifier', 'Census_OSWUAutoUpdateOptionsName', 'Census_GenuineStateName', 'Census_ActivationChannel', 'Census_FlightRing', 'Census_FirmwareManufacturerIdentifier', 'Census_FirmwareVersionIdentifier', 'Census_IsWIMBootEnabled', 'Wdft_RegionIdentifier']
# for col in cols:
#     sns.countplot(x=col, hue='HasDetections', data=small)
#     plt.show()

# отображаем уникальные значения и их количество
# for c in small.columns:
#     print("\n---- %s ---" % c)
#     print(small[c].value_counts())
