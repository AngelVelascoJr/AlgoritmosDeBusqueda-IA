using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SistemaMetro : MonoBehaviour
{
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
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
    [Header("Datos")]
    [SerializeField] private EstacionesConTransbordes[] estacionesObjetivo;

    [SerializeField] private List<CostoEstacionSO> estaciones;
    [SerializeField] private List<CostoCaminoSO> caminos;
    
    [Header("Variables")]
    [SerializeField] private CaminoRecorrido EstacionARevisarEnElSiguienteCiclo;
    [SerializeField] private List<CaminoRecorrido> EstacionesVisitadas;
    [SerializeField] private List<CostoCaminoSO> caminosPreviosYaRevisados;

    [Header("Variables de ciclo")]
    [SerializeField] private int MaxIterations;

    [SerializeField] private bool StepByStep;
    [SerializeField] private bool Step = false;
    [SerializeField] private int SSIterationsVisual = 0;


    private void Start()
    {
        //RevisarDuplicados(estaciones);
        //RevisarCaminosDuplicados(caminos);
        if(!StepByStep)
        {
            for (int i = 0; i < MaxIterations; i++)
            {
                bool breakVar = AStarCycle(i);
                if (breakVar)
                    break;
            }
        }        
    }

    private void Update()
    {
        if (StepByStep)
        {
            if (Step)
            {
                Step = false;
                SSIterationsVisual++;
                bool breakVar = AStarCycle(SSIterationsVisual);
                if(breakVar)
                    StepByStep = false;
            }
        }
    }

    private bool AStarCycle(int iterations)
    {
        bool EncontroSolucion = AStar();
        if (EncontroSolucion)
        {
            string log = $"<color=lime>found after {iterations} iterations</color>\nCamino: <color=cyan>-";
            for(int i = 0; i < EstacionARevisarEnElSiguienteCiclo.ListaEstacionesRecorridasEnEsteCiclo.Count; i++)
            {
                log += $"{EstacionARevisarEnElSiguienteCiclo.ListaEstacionesRecorridasEnEsteCiclo[i].Estacion}-";
            }
            log += $"</color>\nCosto del camino total: <color=yellow>{EstacionARevisarEnElSiguienteCiclo.CostoCaminoRecorrido}</color>";
            Debug.Log(log);
            return true;
        }
        return false;
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

        Debug.Log("<color=red>-------------------------</color>");

        //1.- Obtener nodos hijo, agregarlos a un arreglo de todos los caminos posibles
        BuscarCaminosEnHijosNuevos();

        //2.- Revisar de dicho arreglo, cual es el nodo con menor costo
        CostoEstacionSO PosibleOtraEstacion = null;
        CostoCaminoSO caminoMenosCostoso = null;
        CostoEstacionSO NuevaEstacion = null;
        CaminoRecorrido caminoMejor = null;
        ObtenerCaminoDeMenorCosto(ref PosibleOtraEstacion, ref caminoMenosCostoso, ref NuevaEstacion, ref caminoMejor);

        //Debug.Log($"Camino menos costoso: {caminoMenosCostoso}\nNueva estacion: {NuevaEstacion}\nCamino mejor: [{caminoMejor}]\nEstacion actual SO: {EstacionARevisarEnElSiguienteCiclo.EstacionInicialSO}");


        //3.- Moverse al nodo con menor costo, almacenar el costo del camino hasta ese momento y el camino seguido 
        MoverseAOtroNodo(caminoMenosCostoso, NuevaEstacion, caminoMejor);

        //4.- Verificar si ese nodo es el objetivo, si si terminoar, si no, repetir
        for (int i = 0; i < estacionesObjetivo.Length; i++)
        {
            EstacionesConTransbordes estacionObjetivo = estacionesObjetivo[i];
            if (estacionObjetivo == EstacionARevisarEnElSiguienteCiclo.EstacionInicialSO.Estacion)
                return true;
        }
        return false;
    }

    private void BuscarCaminosEnHijosNuevos()
    {
        int index = -1;
        bool containsCamino = false;            
        foreach (var Camino in caminos)
        {
            var estacionesQueTieneUnCamino = Camino.GetEstaciones().ToList();
            //si el camino no tiene la estacion en la que estamos, continuar la busqueda
            if (!estacionesQueTieneUnCamino.Contains(EstacionARevisarEnElSiguienteCiclo.EstacionInicialSO))
            {
                continue;
            }

            //si el camino ya fue revisado, continuar
            if(caminosPreviosYaRevisados.Contains(Camino))
            {
                continue;
            }

            //revisa si ya agregamos el camino, se ejecuta 1 vez por ciclo
            if (!containsCamino)
            {
                List<CostoEstacionSO> CopyListaEstacionesRecorridasEnEsteCiclo = new List<CostoEstacionSO>();
                foreach (var estacionRecorrida in EstacionARevisarEnElSiguienteCiclo.ListaEstacionesRecorridasEnEsteCiclo)
                {
                    CopyListaEstacionesRecorridasEnEsteCiclo.Add(estacionRecorrida);
                }
                CopyListaEstacionesRecorridasEnEsteCiclo.Add(EstacionARevisarEnElSiguienteCiclo.EstacionInicialSO);
                var camRecorrido = new CaminoRecorrido(EstacionARevisarEnElSiguienteCiclo.EstacionInicialSO, EstacionARevisarEnElSiguienteCiclo.CostoCaminoRecorrido, new List<CostoCaminoSO>(), CopyListaEstacionesRecorridasEnEsteCiclo);
                camRecorrido.CaminosRestantesPorRecorrer.Add(Camino);
                EstacionesVisitadas.Add(camRecorrido);
                index = EstacionesVisitadas.Count - 1;
                containsCamino = true;
            }
            else
            {
                //si el camino ya esta dentro de caminos restantes por recorrer o dentro de caminos ya recorridos, ignorar
                if (!EstacionesVisitadas[index].CaminosRestantesPorRecorrer.Contains(Camino) && !EstacionesVisitadas[index].CaminosYaRecorridos.Contains(Camino))
                    EstacionesVisitadas[index].CaminosRestantesPorRecorrer.Add(Camino);
            }
        }

        string log = "";
        foreach (var Estacion in EstacionesVisitadas)
            foreach (var CaminosRestantes in Estacion.CaminosRestantesPorRecorrer)
            {
                log += $"{CaminosRestantes.GetEstaciones()[0]} y {CaminosRestantes.GetEstaciones()[1]}\n";
            }
        Debug.Log(log);
    }

    private void ObtenerCaminoDeMenorCosto(ref CostoEstacionSO PosibleOtraEstacion, ref CostoCaminoSO caminoMenosCostoso, ref CostoEstacionSO NuevaEstacion, ref CaminoRecorrido caminoMejor)
    {
        string log = "";
        foreach (var EstacionVisitada in EstacionesVisitadas)
        {
            foreach (var Camino in EstacionVisitada.CaminosRestantesPorRecorrer)
            {
                //revisar cual es el otro camino
                if (Camino.EstacionesSO[0] == EstacionVisitada.EstacionInicialSO)
                {
                    PosibleOtraEstacion = Camino.EstacionesSO[1];
                }
                else
                    PosibleOtraEstacion = Camino.EstacionesSO[0];

                log += $"Posible otra estacion: {PosibleOtraEstacion} en camino: {EstacionVisitada.EstacionInicialSO}\n\t";
                //comparar el costo entre todos los nuevos caminos
                // para cuando es el primer ciclo
                if (caminoMenosCostoso == null)
                {
                    caminoMenosCostoso = Camino;
                    NuevaEstacion = PosibleOtraEstacion;
                    caminoMejor = EstacionVisitada;
                    log += $"Primera eleccion de <color=orange>{NuevaEstacion}</color>\n";
                }

                //Calcular el costo de este nuevo nodo
                log += $"Hacia {PosibleOtraEstacion.Estacion}:Costo acumulado: {EstacionVisitada.CostoCaminoRecorrido}, Costo camino: {Camino.costo}, Costo nodo: {PosibleOtraEstacion.costo},\t total: <color=yellow>{EstacionVisitada.CostoCaminoRecorrido + Camino.costo + PosibleOtraEstacion.costo}</color>\n";

                int costoActual = caminoMejor.CostoCaminoRecorrido + caminoMenosCostoso.costo + NuevaEstacion.costo;
                int costoOtroCamino = EstacionVisitada.CostoCaminoRecorrido + Camino.costo + PosibleOtraEstacion.costo;

                if (costoOtroCamino < costoActual)
                {
                    log += $"Cambio de camino de <color=orange>{NuevaEstacion}</color> a <color=orange>{PosibleOtraEstacion}</color>\n";
                    caminoMenosCostoso = Camino;
                    NuevaEstacion = PosibleOtraEstacion;
                    caminoMejor = EstacionVisitada;
                }
            }
        }

        Debug.Log(log);

        string log2 = "";

        log2 += $"<color=magenta>Camino a seguir: {caminoMenosCostoso.EstacionesSO[0].Estacion.ToString()} a {caminoMenosCostoso.EstacionesSO[1].Estacion.ToString()}</color>\n";
        log2 += $"\tMenor costo: {caminoMenosCostoso.costo + NuevaEstacion.costo}";
        log2 += $"\tcamino total menor: {caminoMejor.CostoCaminoRecorrido} + {caminoMenosCostoso.costo} + {NuevaEstacion.costo} = {caminoMejor.CostoCaminoRecorrido + caminoMenosCostoso.costo + NuevaEstacion.costo}: Estacion a moverse: <color=cyan>{NuevaEstacion.Estacion.ToString()}</color>\n";

        Debug.Log(log2);

    }
    
    private void MoverseAOtroNodo(CostoCaminoSO caminoMenosCostoso, CostoEstacionSO NuevaEstacion, CaminoRecorrido caminoMejor)
    {
        //agregar caminoMenosCostoso a lista de exclusion
        caminosPreviosYaRevisados.Add(caminoMenosCostoso);

        //agregar caminoMenosCostoso a CaminosPreviosYaRevisados
        caminoMejor.CaminosYaRecorridos.Add(caminoMenosCostoso);
        //eliminar camino de CaminosRestantesPorRecorrer[caminoMenosCostoso]
        caminoMejor.CaminosRestantesPorRecorrer.Remove(caminoMenosCostoso);

        //Cambiar estacionARevisarEnElSiguienteCiclo, agregas el costo total como la suma de el camino mejor y el menos costoso
        EstacionARevisarEnElSiguienteCiclo = new CaminoRecorrido(NuevaEstacion, caminoMenosCostoso.costo + caminoMejor.CostoCaminoRecorrido, new List<CostoCaminoSO>(), caminoMejor.ListaEstacionesRecorridasEnEsteCiclo);

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

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0,0,300,200), "Step"))
        {
            Step = true;
        }
    }

}
