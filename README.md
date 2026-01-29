# Autonomous-Virtual-Vehicle-2D
Perceptrón multicapa aplicado en Unity para el entrenamiento y ejecución de un vehículo virtual a partir de un Dataset.

## REQUERIMIENTOS

Los requerimientos del sistema son los siguientes:
* Necesitas instalar .NET Framework en su ultima version.
* El S.O debe de ser Windows 10 o superior o cualquier sistema compatible con la ultima version de .NET Framework.
* Necesitas tener DirectX 11 o superior instalado.

## INSTALACION

La instalacion no es complicada. Solamente descomprima el .zip incluido en la version de releases, y, ademas, ejecute el archivo **TRAIN NEURAL NET CARS.exe**. La version release actual solamente es para obtencion de datos de conduccion humana. Estos datos son capturados directamente por el juego y, ademas, se guardan en un archivo dentro de capeta Dataset, dentro del directorio donde esta el juego.

## CONTROLES

Los controles del juego son los siguientes:
* W = Acelerar
* S = Desacelerar o frenar
* A = Girar a la izquierda
* D = Girar a la derecha
* Esc = Salir del juego.

## Que datos captura??

Los datos capturados son de los sensores del autos (Son unas X que se pintan en frente del auto virtual), ademas de la posicion en el mundo (X,Y), la velocidad y la potencia de giro y de motor. Proximamente se creara un diccionario de datos mas especifico que describa los datos que reune.

## Artículo científico

El artículo ha sido publicado en la IEEE mediante la revista CONIITI en noviembre de 2020. Se puede encontrar facilmente aquí: [Autonomous virtual vehicles with FNN-GA and
Q-learning in a video game environment.](https://ieeexplore.ieee.org/document/9240296)
