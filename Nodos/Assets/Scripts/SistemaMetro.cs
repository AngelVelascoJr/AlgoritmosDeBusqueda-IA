using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SistemaMetro : MonoBehaviour
{
    public enum EstacionesConTransbordes
    {
        Pantitlan,
        El_rosario,
        Instituto_del_petroleo,
        Deportivo_18_de_Marzo,
        La_Raza,
        Martin_Carrera,
        Consulado,
        Tacuba,
        Oceania,
        Guerrero,
        Garibaldi,
        Hidalgo,
        Bellas_artes,
        Morelos,
        San_Lazaro,
        Balderas,
        Salto_del_agua,
        Pino_suarez,
        Candelaria,
        Tacubaya,
        CentroMedico,
        Chabacano,
        Jamaica,
        Santa_Anita,
        Mixcoac,
        Zapata,
        Ermita,
        Atlalilco,
        Universidad,
        La_Villa_Basilica
    }

    [SerializeField] private EstacionesConTransbordes[] estacionesObjetivo;

    [SerializeField] private List<CostoNodoSO> estaciones;
    [SerializeField] private List<CostoCaminoSO> caminos;

    [SerializeField] private CaminoRecorrido EstacionActual;
    [SerializeField] private List<CaminoRecorrido> EstacionesVisitadas;
    [SerializeField] private List<CostoCaminoSO> caminosPreviosNoRevisados;

    [SerializeField] private int MaxIterations;


    private void Start()
    {
        //RevisarDuplicados(estaciones);
        //RevisarCaminosDuplicados(caminos);
        //CaminoRecorrido main = new CaminoRecorrido();
        //main.SetEstacionActual(EstacionesConTransbordes.Pantitlan);//main.AddEstacionToRecorridas(EstacionesConTransbordes.Pantitlan);
        //EstacionActual = main;
        //EstacionesVisitadas.Add(EstacionActual.estacionActualSO);
        for (int i = 0; i < MaxIterations; i++)
        {
            bool found = AStar();
            if (found)
            {
                Debug.Log($"found after {i} iterations");
                break;
            }
        }
    }

    private bool AStar()
    {
        //1.- revisar si estamos en objetivo
        foreach (var EstacionObjetivo in estacionesObjetivo)
        {
            if (EstacionActual.estacionActualSO.Estacion == EstacionObjetivo)//.Contains(EstacionObjetivo))
            {
                Debug.Log("Se ha llegado al objetivo");
                return true;
            }
        }

        //2.- revisar el nodo hijo menos costoso (nodo a moverse + camino total) entre TODOS los nodos
        //2.1.- obtener los nodos hijo (posibles caminos)
        //var caminosPosibles = new List<CostoCaminoSO>();
        var EstacionActualSO = EstacionActual.estacionActualSO;
        ObtenerCaminosPosibles(EstacionActualSO);

        //2.2.- revisar el camino menos costoso
        CostoCaminoSO caminoMenosCostoso = null;
        CostoNodoSO NuevaEstacion = null;
        BuscarElMejorCamino(EstacionActualSO, ref caminoMenosCostoso, ref NuevaEstacion);


        Debug.Log($"Camino a seguir: {caminoMenosCostoso.EstacionesSO[0].Estacion.ToString()} a {caminoMenosCostoso.EstacionesSO[1].Estacion.ToString()}");
        Debug.Log($"Menor costo: {caminoMenosCostoso.costo + NuevaEstacion.costo}");
        Debug.Log($"camino total menor: {EstacionActual.CostoCaminoRecorrido} + {caminoMenosCostoso.costo} + {NuevaEstacion.costo} = {EstacionActual.CostoCaminoRecorrido + caminoMenosCostoso.costo + NuevaEstacion.costo}: Estacion a moverse: {NuevaEstacion.Estacion.ToString()}");

        //3.- moverse a ese
        //3.1- Guardar los otros nodos por revisar
        foreach (var Estacion in EstacionesVisitadas)
        {
            foreach (var camino in Estacion.CaminosRestantesPorRecorrer)
            {
                if (!caminosPreviosNoRevisados.Contains(camino) && camino != caminoMenosCostoso)
                    caminosPreviosNoRevisados.Add(camino);
            }
        }


        //3.1.- Cambiarse al nuevo 
        //EstacionesVisitadas.Add(new CaminoRecorrido(EstacionActual.estacionActualSO, EstacionActual.CostoCaminoRecorrido));
        EstacionActual.SetEstacionActual(NuevaEstacion);
        EstacionActual.AddToCostoCaminoRecorrido(caminoMenosCostoso.costo);

        //4.- repetir
        return false;
    }

    private void BuscarElMejorCamino(CostoNodoSO EstacionActual, ref CostoCaminoSO caminoMenosCostoso, ref CostoNodoSO NuevaEstacion)
    {
        CostoNodoSO PosibleOtraEstacion = null;
        foreach (var EstacionVisitada in EstacionesVisitadas)
        {
            foreach (var Camino in EstacionVisitada.CaminosRestantesPorRecorrer)
            {

                //revisar cual es el otro camino
                if (Camino.EstacionesSO[0].Estacion == EstacionActual.Estacion)
                {
                    PosibleOtraEstacion = Camino.EstacionesSO[1];

                }
                else
                    PosibleOtraEstacion = Camino.EstacionesSO[0];
                //comparar el costo entre todos los nuevos caminos
                // para cuando es el primer ciclo
                if (caminoMenosCostoso == null)
                {
                    caminoMenosCostoso = Camino;
                    NuevaEstacion = PosibleOtraEstacion;
                }


                //Calcular el costo de este nuevo nodo
                Debug.Log($"Hacia {PosibleOtraEstacion.Estacion}: Costo camino: {Camino.costo}, Costo nodo: {PosibleOtraEstacion.costo}");

                int costoActual = this.EstacionActual.CostoCaminoRecorrido + caminoMenosCostoso.costo + EstacionActual.costo;
                int costoOtroCamino = this.EstacionActual.CostoCaminoRecorrido + Camino.costo + PosibleOtraEstacion.costo;

                if (costoOtroCamino < costoActual)
                {
                    caminoMenosCostoso = Camino;
                    NuevaEstacion = PosibleOtraEstacion;
                }
            }
        }

        //

    }

    private void ObtenerCaminosPosibles(CostoNodoSO EstacionActualSO)
    {
        foreach (var Camino in caminos)
        {
            var estacionesQueTieneUnCamino = Camino.GetEstaciones().ToList();
            //si el camino no tiene la estacion en la que estamos, continuar la busqueda
            if (!estacionesQueTieneUnCamino.Contains(EstacionActualSO))
            {
                continue;
            }

            //
            bool containsCamino = false;
            int index = -1;
            for (int i = 0; i < EstacionesVisitadas.Count; i++)
            {
                if (EstacionesVisitadas[i].estacionActualSO == EstacionActualSO)
                {
                    containsCamino = true;
                    index = i;
                }
            }

            if(!containsCamino)
            {
                var camRecorrido = new CaminoRecorrido(EstacionActualSO, EstacionActual.CostoCaminoRecorrido, new List<CostoCaminoSO>());
                camRecorrido.CaminosRestantesPorRecorrer.Add(Camino);
                EstacionesVisitadas.Add(camRecorrido);
            }
            else
            {
                EstacionesVisitadas[index].CaminosRestantesPorRecorrer.Add(Camino);
            }
        }

        foreach (var Estacion in EstacionesVisitadas)
            foreach (var CaminosRestantes in Estacion.CaminosRestantesPorRecorrer)
            {
                Debug.Log($"{CaminosRestantes.GetEstaciones()[0]} y {CaminosRestantes.GetEstaciones()[1]}");
            }
    }

    private void RevisarDuplicados<r1>(List<r1> listaARevisar)
	{
		List<r1> var = new List<r1>();
		for (int i = 0; i < listaARevisar.Count; i++)
		{
			if (var.Contains(listaARevisar[i]))
				Debug.LogWarning($"Se encontro un elemento duplicado: {listaARevisar[i]}");
			else
				var.Add(listaARevisar[i]);
		}
		Debug.Log("Fin de revision de lista");
	}

    private void RevisarCaminosDuplicados(List<CostoCaminoSO> listaARevisar)
    {
        List<List<CostoNodoSO>> ListaRevisados = new List<List<CostoNodoSO>>();
        for (int i = 0; i < listaARevisar.Count; i++)
        {
			var estacionesEnLista = listaARevisar[i].GetEstaciones().ToList();
			//Debug.Log($"{estacionesEnLista[0]}, {estacionesEnLista[1]}");
            for (int j = 0;	j < ListaRevisados.Count; j++)
			{
				int Repeated = 0;
				for (int k = 0; k < estacionesEnLista.Count; k++)
				{
					if (estacionesEnLista[k] == ListaRevisados[j][0] || estacionesEnLista[k] == ListaRevisados[j][1])
						Repeated++;
				}
				if (Repeated > 2)
					Debug.Log($"Repeated values found in {estacionesEnLista[0]} and {estacionesEnLista[1]}");
			}
			ListaRevisados.Add(estacionesEnLista);
        }
        Debug.Log("Fin de revision de lista");
    }

}
