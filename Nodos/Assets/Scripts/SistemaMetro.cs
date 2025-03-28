using System.Collections.Generic;
using System.Linq;
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
	[SerializeField] private List<CostoNodoSO> EstacionesVisitadas;
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
			if(found)
			{
				Debug.Log($"found after {i} iterations");
				break;
			}
		}
    }

	private bool AStar()
	{
		//1.- revisar si estamos en objetivo
		foreach(var EstacionObjetivo in estacionesObjetivo)
		{
			if(EstacionActual.estacionActualSO.Estacion == EstacionObjetivo)//.Contains(EstacionObjetivo))
			{
				Debug.Log("Se ha llegado al objetivo");
				return true;
			}
		}

		//2.- revisar el nodo hijo menos costoso (nodo a moverse + camino total) entre TODOS los nodos
		//2.1.- obtener los nodos hijo (posibles caminos)
		var caminosPosibles = new List<CostoCaminoSO>();
		var EstacionActualEnum = EstacionActual.estacionActualSO.Estacion;
		foreach (var Camino in caminos)
		{
			var estacionesEnCamino = Camino.GetEstaciones().ToList();
			var estacionesVisitadasEnum = new List<EstacionesConTransbordes>();
			foreach (var estacionEnum in EstacionesVisitadas)
			{
				estacionesVisitadasEnum.Add(estacionEnum.Estacion);
			}

            if (estacionesEnCamino.Contains(EstacionActualEnum))
			{
				bool tieneElementos = false;
				foreach (var element in estacionesEnCamino)
				{
					if (estacionesVisitadasEnum.Contains(element))
						tieneElementos = true;
				}
				if(!tieneElementos)
					caminosPosibles.Add(Camino);
			}
		}

		foreach (var Camino in caminosPosibles)
			Debug.Log($"{Camino.GetEstaciones()[0]} y {Camino.GetEstaciones()[1]}");

		//2.2.- revisar el camino menos costoso
		CostoCaminoSO caminoMenosCostoso = null;
		CostoNodoSO PosibleOtraEstacion = null;
		foreach (var Camino in caminosPosibles)
		{
			//revisar cual es el otro camino
			//comparar el costo entre todos los nuevos caminos
			if (caminoMenosCostoso == null)
			{
				caminoMenosCostoso = Camino;
			}
            if (caminoMenosCostoso.EstacionesSO[0].Estacion == EstacionActualEnum)
                PosibleOtraEstacion = caminoMenosCostoso.EstacionesSO[1];
            else
                PosibleOtraEstacion = caminoMenosCostoso.EstacionesSO[0];
			int CostoNodoNuevo = PosibleOtraEstacion.costo;
			Debug.Log($"Hacia {PosibleOtraEstacion.Estacion}: Costo camino: {Camino.costo}, Costo nodo: {CostoNodoNuevo}");

			int costoActual = EstacionActual.CostoCaminoRecorrido + caminoMenosCostoso.costo + CostoNodoNuevo;
			int costoOtroCamino = EstacionActual.CostoCaminoRecorrido + Camino.costo + PosibleOtraEstacion.costo;

            if (costoOtroCamino < costoActual)
			{
				caminoMenosCostoso = Camino;
            }
		}

		CostoNodoSO NuevaEstacion = PosibleOtraEstacion;

		Debug.Log($"Camino a seguir: {caminoMenosCostoso.EstacionesSO[0].Estacion.ToString()} a {caminoMenosCostoso.EstacionesSO[1].Estacion.ToString()}");
		Debug.Log($"Menor costo: {caminoMenosCostoso.costo + NuevaEstacion.costo}");
		Debug.Log($"camino total menor: {EstacionActual.CostoCaminoRecorrido} + {caminoMenosCostoso.costo} + {NuevaEstacion.costo} = {EstacionActual.CostoCaminoRecorrido + caminoMenosCostoso.costo + NuevaEstacion.costo}: Estacion a moverse: {NuevaEstacion.Estacion.ToString()}");

		//3.- moverse a ese
		//3.1- Guardar los otros nodos por revisar
		foreach (var camino in caminosPosibles)
		{
			if (!caminosPreviosNoRevisados.Contains(camino) && camino != caminoMenosCostoso)
				caminosPreviosNoRevisados.Add(camino);
		}


		//3.1.- Cambiarse al nuevo 
		EstacionesVisitadas.Add(EstacionActual.estacionActualSO);
		EstacionActual.SetEstacionActual(NuevaEstacion);
		EstacionActual.AddToCostoCaminoRecorrido(caminoMenosCostoso.costo);

		//4.- repetir
		return false;
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
        List<List<EstacionesConTransbordes>> ListaRevisados = new List<List<EstacionesConTransbordes>>();
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
