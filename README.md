# Practica 2: Algoritmos de busqueda

Universidad Nacional Autónoma de México 
Facultad de Ingeniería 
Inteligencia Artificial 

Práctica 2 

Integrantes:
- Barrera Gomez Támara 
- Juarez valdez Israel 
- Portillo Jaramillo David
- Velasco Pérez Angel David
- Villanueva Pérez Vianey
- Zarco Evandher

## Parte 1: A aplicado a rutas de metro

**Descripción del problema:**
Un turista está visitando una ciudad y quiere encontrar la ruta más rápida entre dos estaciones de metro. Se tiene información sobre las conexiones entre estaciones y una estimación del tiempo de viaje entre ellas.

**Tareas:**
1. Modelar el mapa del metro como un grafo, donde los nodos son estaciones y los arcos representan conexiones con tiempos de viaje.
2. Implementar el algoritmo A para encontrar la mejor ruta entre dos estaciones dadas.
3. Probar el algoritmo con al menos dos pares de estaciones y analizar los resultados.
(probabilidad)

## Prueba del algoritmo

Para la prueba del algoritmo, se tomo todo el sistema de metro y las 4 estaciones objetivo :::::::::::::

el codigo se desarrollo en python y se encuentra dentro de la carpeta Python, tambien se puede acceder a el desde [aqui](./Python/AStar.ipynb).

Es posible modificar la estacion en las que se requiera iniciar al modificar el nombre de la variable estInicialStr dentro de la seccion [Conversion de JsonFile a Datos](./Python/AStar.ipynb#'ConversiondeJsonFileaDatos')

De la misma forma, es posible cambiar las estaciones objetivo 
al modificar la lista de estaciones al crear una instancia de la funcion **SistemaMetro**.

Al utilizar la estacion *pantitlan* como estacion inicial, obtenemos que la estacion mas cercana segun el costo de los caminos y el costo de los nodos es **La villa Basilica** como se puede observar en el codigo y en la siguiente imagen.

![ImagenAStar](./Images/AStarResults.jpeg)


## Parte 3: Algoritmos Genéticos para Optimización de Rutas de Metro
### Descripción del problema
Se busca encontrar la mejor combinación de estaciones para optimizar una ruta turística en metro, maximizando la cantidad de sitios visitados en un tiempo determinado.

### Tareas
1. Representar una ruta en metro como un cromosoma (lista de estaciones en un orden determinado).
2. Definir una función de aptitud basada en el tiempo de viaje y la cantidad de sitios visitados.
3. Implementar un algoritmo genético con selección, cruza y mutación para encontrar la mejor ruta.
4. Probar el algoritmo con un conjunto de 10 estaciones y analizar los resultados.

### Implementación
El algoritmo genético se implementó en Python utilizando el framework de Google Colab. El código se encuentra en la carpeta Python y se llama  el archivo Algoritmos_Geneticos.ipynb tambien se puede acceder a el desde [aqui](./Python/Algoritmos_Geneticos.ipynb).

Se tomo el sistema del metro representado en el siguiente grafo:

![Grafo Metro](./Images/grafo_metro.png)

### Análisis de Resultados
En la siguiente imagen podemos ver los resultados tras 300 iteraciones, tenemos un rendimiento prácticamente excelente, también se puede visualizar un poco de la población inicial y final.

![Resultados](./Images/resultados_text.png)

Nuestro análisis de resultados se basará en la gráfica "Average Fitness vs Iterations", que se muestra a continuación:

![Average Fitness vs Iterations](./Images/resultados_gráfica.png)

**Interpretación de la gráfica:**
La gráfica muestra la evolución del fitness promedio de la población a lo largo de las generaciones. Se observa que el fitness promedio aumenta con el tiempo y tiende a estabilizarse en un valor, lo que indica que el algoritmo está convergiendo hacia una solución óptima.

**Observaciones:**

* **Convergencia:** El algoritmo converge relativamente rápido en las primeras iteraciones, lo que sugiere que la función de fitness y los operadores genéticos son efectivos para encontrar buenas soluciones.
* **Estabilidad:** Después de un cierto número de iteraciones, el fitness promedio tiende hacia ciertos valores, lo que indica que el algoritmo ha encontrado una solución óptima o cercana al óptimo.
* **Fitness del mejor individuo:** 243.57. Este valor es el esperado en comparación con el fitness del modelo objetivo, lo que indica que el algoritmo ha encontrado una buena solución.
* **Tiempo de viaje y sitios visitados:** Se encontró una solución que permite visitar **76 sitios turísticos** en **24 minutos**. Estos valores indican que el mejor individuo **cumple** con los criterios solicitados.

### Conclusiones
* El algoritmo genético es efectiva para la optimización de rutas de metro.
* La función de fitness y los operadores genéticos utilizados son adecuados para el problema.
* El algoritmo converge hacia una solución óptima o cercana al óptimo.
* El mejor individuo encontrado representa una buena solución al problema.
