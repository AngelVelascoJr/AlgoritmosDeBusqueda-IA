using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    [SerializeField] private List<CostoEstacionSO> estaciones;
    [SerializeField] private List<CostoCaminoSO> caminos;

    [SerializeField] private CaminoRecorrido EstacionActual;
    [SerializeField] private List<CaminoRecorrido> EstacionesVisitadas;
    [SerializeField] private List<CostoCaminoSO> caminosPreviosYaRevisados;

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
                Debug.Log($"<color=lime>found after {i} iterations</color>");
                break;
            }
        }
    }


    /* Pasos A*
     * 
     * 1.- Obtener nuevos nodos hijo, agregarlos a un arreglo de todos los caminos posibles
     * 2.- Revisar de dicho arreglo, cual es el nodo con menor costo
     * 3.- Moverse al nodo con menor costo, almacenar el costo del camino hasta ese momento y el camino seguido
     * 4.- Verificar si ese nodo es el objetivo, si si terminoar, si no, repetir
     *  
     */


    private bool AStar()
    {
        //1.- Obtener nodos hijo, agregarlos a un arreglo de todos los caminos posibles
        BuscarCaminosHijosNuevos();

        //2.- Revisar de dicho arreglo, cual es el nodo con menor costo
        CostoEstacionSO PosibleOtraEstacion = null;
        CostoCaminoSO caminoMenosCostoso = null;
        CostoEstacionSO NuevaEstacion = null;
        ObtenerCaminoDeMenorCosto(ref PosibleOtraEstacion, ref caminoMenosCostoso, ref NuevaEstacion);

        //3.- Moverse al nodo con menor costo, almacenar el costo del camino hasta ese momento y el camino seguido 
        ModerseAOtroNodo(caminoMenosCostoso, NuevaEstacion);

        //4.- Verificar si ese nodo es el objetivo, si si terminoar, si no, repetir
        foreach (var estacionObjetivo in estacionesObjetivo)
        {
            if(estacionObjetivo == EstacionActual.estacionActualSO.Estacion)
                return true;
        }
        return false;
    }
    private void BuscarCaminosHijosNuevos()
    {
        foreach (var Camino in caminos)
        {
            var estacionesQueTieneUnCamino = Camino.GetEstaciones().ToList();
            //si el camino no tiene la estacion en la que estamos, continuar la busqueda
            if (!estacionesQueTieneUnCamino.Contains(EstacionActual.estacionActualSO))
            {
                continue;
            }

            //si el camino ya fue revisado, continuar
            if(caminosPreviosYaRevisados.Contains(Camino))
            {
                continue;
            }

            //revisa si ya tenemos el camino agregado dentro de el arreglo
            bool containsCamino = false;
            int index = -1;
            for (int i = 0; i < EstacionesVisitadas.Count; i++)
            {
                if (EstacionesVisitadas[i].estacionActualSO == EstacionActual.estacionActualSO)
                {
                    containsCamino = true;
                    index = i;
                }
            }
            if (!containsCamino)
            {
                var camRecorrido = new CaminoRecorrido(EstacionActual.estacionActualSO, EstacionActual.CostoCaminoRecorrido, new List<CostoCaminoSO>());
                camRecorrido.CaminosRestantesPorRecorrer.Add(Camino);
                EstacionesVisitadas.Add(camRecorrido);
            }
            else
            {
                //si el camino ya esta dentro de caminos restantes por recorrer o dentro de caminos ya recorridos, ignorar
                if (!EstacionesVisitadas[index].CaminosRestantesPorRecorrer.Contains(Camino) && !EstacionesVisitadas[index].CaminosYaRecorridos.Contains(Camino))
                    EstacionesVisitadas[index].CaminosRestantesPorRecorrer.Add(Camino);
            }
        }

        foreach (var Estacion in EstacionesVisitadas)
            foreach (var CaminosRestantes in Estacion.CaminosRestantesPorRecorrer)
            {
                Debug.Log($"{CaminosRestantes.GetEstaciones()[0]} y {CaminosRestantes.GetEstaciones()[1]}");
            }
    }

    private void ObtenerCaminoDeMenorCosto(ref CostoEstacionSO PosibleOtraEstacion, ref CostoCaminoSO caminoMenosCostoso, ref CostoEstacionSO NuevaEstacion)
    {
        foreach (var EstacionVisitada in EstacionesVisitadas)
        {
            foreach (var Camino in EstacionVisitada.CaminosRestantesPorRecorrer)
            {
                //revisar cual es el otro camino
                if (Camino.EstacionesSO[0] == EstacionActual.estacionActualSO)
                {
                    PosibleOtraEstacion = Camino.EstacionesSO[1];
                }
                else
                    PosibleOtraEstacion = Camino.EstacionesSO[0];

                Debug.Log($"Posible otra estacion: {PosibleOtraEstacion} en camino: {EstacionVisitada.estacionActualSO}");
                //comparar el costo entre todos los nuevos caminos
                // para cuando es el primer ciclo
                if (caminoMenosCostoso == null)
                {
                    caminoMenosCostoso = Camino;
                    NuevaEstacion = PosibleOtraEstacion;
                }

                //Calcular el costo de este nuevo nodo
                Debug.Log($"Hacia {PosibleOtraEstacion.Estacion}: Costo camino: {Camino.costo}, Costo nodo: {PosibleOtraEstacion.costo}");

                int costoActual = this.EstacionActual.CostoCaminoRecorrido + caminoMenosCostoso.costo + EstacionActual.estacionActualSO.costo;
                int costoOtroCamino = this.EstacionActual.CostoCaminoRecorrido + Camino.costo + PosibleOtraEstacion.costo;

                if (costoOtroCamino < costoActual)
                {
                    caminoMenosCostoso = Camino;
                    NuevaEstacion = PosibleOtraEstacion;
                }
            }
        }
        
        Debug.Log($"Camino a seguir: {caminoMenosCostoso.EstacionesSO[0].Estacion.ToString()} a {caminoMenosCostoso.EstacionesSO[1].Estacion.ToString()}");
        Debug.Log($"Menor costo: {caminoMenosCostoso.costo + NuevaEstacion.costo}");
        Debug.Log($"camino total menor: {EstacionActual.CostoCaminoRecorrido} + {caminoMenosCostoso.costo} + {NuevaEstacion.costo} = {EstacionActual.CostoCaminoRecorrido + caminoMenosCostoso.costo + NuevaEstacion.costo}: Estacion a moverse: {NuevaEstacion.Estacion.ToString()}");
    }
    private void ModerseAOtroNodo(CostoCaminoSO caminoMenosCostoso, CostoEstacionSO NuevaEstacion)
    {
        //EstacionesVisitadas.Add(new CaminoRecorrido(EstacionActual.estacionActualSO, EstacionActual.CostoCaminoRecorrido, EstacionActual.CaminosRestantesPorRecorrer));

        //EstacionesVisitadas[0].CaminosYaRecorridos.Add(EstacionesVisitadas[0].CaminosRestantesPorRecorrer[0]);
        //EstacionesVisitadas[0].CaminosRestantesPorRecorrer.RemoveAt(0);
        for (int i = 0; i < EstacionesVisitadas.Count; i++)
        {
            CaminoRecorrido estacion = EstacionesVisitadas[i];
            if (estacion.estacionActualSO == EstacionActual.estacionActualSO)
            {
                int index = 0;
                for (int j = 0; j < estacion.CaminosRestantesPorRecorrer.Count; j++)
                {
                    if (estacion.CaminosRestantesPorRecorrer[j] == caminoMenosCostoso)
                    {
                        index = j;
                        break;
                    }
                }
                estacion.CaminosYaRecorridos.Add(estacion.CaminosRestantesPorRecorrer[index]);
                caminosPreviosYaRevisados.Add(estacion.CaminosRestantesPorRecorrer[index]);
                estacion.CaminosRestantesPorRecorrer.RemoveAt(index);
                break;
            }
        }

        EstacionActual.SetEstacionActual(NuevaEstacion);
        EstacionActual.AddToCostoCaminoRecorrido(caminoMenosCostoso.costo);
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
        List<List<CostoEstacionSO>> ListaRevisados = new List<List<CostoEstacionSO>>();
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
