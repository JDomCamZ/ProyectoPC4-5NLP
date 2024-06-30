# Proyecto de Generación de Texto en Entorno VR

## Descripción General

Este proyecto tiene como objetivo integrar un modelo de generación de texto en un entorno de realidad virtual (VR) usando Unity. A continuación, se describen los pasos principales realizados durante el desarrollo del proyecto.

## 1. Selección del Dataset

### Dataset: AG News

Para este proyecto, se eligió el dataset AG News, el cual es utilizado para entrenar el modelo de generación de texto. Este dataset contiene noticias clasificadas en varias categorías, proporcionando una rica fuente de datos para la generación de texto.

## 2. Preparación del Dataset

### Script: `prepare.py`

Se desarrolló un script llamado `prepare.py` enfocado en la preparación del dataset AG News. Este script se encarga de descargar, procesar y preparar los datos para su uso en el entrenamiento del modelo.

## 3. Entrenamiento del Modelo

### Script: `train_ag_news.py`

Para entrenar el modelo de generación de texto, se creó el archivo `train_ag_news.py`. Este script utiliza el dataset preparado para entrenar un modelo de lenguaje, generando el output en la carpeta `out-ag-news`.


## 4. Generación de Muestras

### Script: `sample.py`

Una vez entrenado el modelo, se utiliza el script `sample.py` para generar muestras de texto. Este script apunta a la carpeta con el modelo creado (out-ag-news) y genera ejemplos de texto a partir del modelo entrenado.

## 5. Integración con Unity

### Script en C#: `TextModel.cs`

El output del modelo se integra al entorno de VR usando un script en C# llamado TextModel.cs. En este script, se implementa la funcionalidad de un botón que muestra el texto generado por el modelo en la pantalla del entorno VR.

## 6. Consideraciones y Limitaciones

### Hipótesis y Finetuning

La segunda parte del proyecto se enfoca en probar diferentes hiperparámetros y utilizar finetuning para mejorar el modelo. Sin embargo, debido a limitaciones de hardware, no fue posible cambiar los hiperparámetros en este proyecto. Para realizar esta tarea se podría desarrollar un script que pruebe varios hiperparámetros de manera secuencial.

En cuanto al finetuning, se requiere que los parámetros sean idénticos a los del GPT-2 instaurado por Hugging Face, lo cual presenta incompatibilidades al ejecutar el script `testing_model.py`. Por lo tanto, el proyecto queda estancado en este punto.
