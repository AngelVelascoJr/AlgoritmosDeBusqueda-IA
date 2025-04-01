using System;
using System.Collections.Generic;
using UnityEngine;

public class SistemaPeliculas : MonoBehaviour
{
    public enum PeliculasCandidatas
    {
        Resplandor,
        ElConjuro,
        CisneNegro,
        LaHuerfana,
        ElAro,
        ElTelefonoNegro,
        Ouija,
        Hablame,
        ElExorcista,
        Halloween
    }
    [Header("Datos")]
    [SerializeField] private List<PeliculaSO> ListaPeliculasCandidatas;

    [Serializable] delegate float FuncionDeEvaluacion(PeliculaSO pelicula);
    [Header("Variables")]
    [SerializeField] FuncionDeEvaluacion funcionDeEvaluacion;
    [SerializeField] PeliculaSO MejorPelicula;
    [SerializeField] float ValorMejorPelicula;

    [Header("Variables de ciclo")]
    [SerializeField] private bool StepByStep;
    [SerializeField] private bool Step = false;
    [SerializeField] private int SSIterationsVisual = 0;

    private void Start()
    {
        //RevisarDuplicados(estaciones);
        //RevisarCaminosDuplicados(caminos);
        ValorMejorPelicula = float.MinValue;
        funcionDeEvaluacion += ValorSegunElReporte;
        //funcionDeEvaluacion += ValorSegunCalificacionPorTiempo;

        if (!StepByStep)
        {
            MejorPelicula = null;
            for (int i = 0; i < ListaPeliculasCandidatas.Count; i++)
            {
                MejorPelicula = AscensoEnColinas(i);
                if(MejorPelicula == null)
                {
                    gameObject.SetActive(false);
                    Debug.Log("No MejorPelicula found, deactivating");
                    return;
                }
            }
            if (MejorPelicula == null)
                return;
            Debug.Log($"segun la funcion {funcionDeEvaluacion.Method.Name}, la mejor pelicula para ver es {MejorPelicula}");
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
                MejorPelicula = AscensoEnColinas(SSIterationsVisual);
                if (SSIterationsVisual == ListaPeliculasCandidatas.Count)
                {
                    StepByStep = false;
                    Debug.Log($"segun la funcion {funcionDeEvaluacion.Method.Name}, la mejor pelicula para ver es {MejorPelicula}");
                }
                else if (MejorPelicula == null)
                {
                    StepByStep = false;
                    return;
                }
            }
        }
    }

    private PeliculaSO AscensoEnColinas(int index)
    {
        //obtener valor de la funcion de evaluacion para cada elemento
        //aplicar la funcion de evaluacion para cada elemento, mantener el que mejor queden (por que son peliculas de terror aleatorias)
        if(funcionDeEvaluacion == null)
        {
            Debug.LogWarning("No se especifico una funcion de evaluacion, saliendo");
            return null;
        }
        float ValorDePeliculaRevisada = funcionDeEvaluacion.Invoke(ListaPeliculasCandidatas[index]);
        if(ValorDePeliculaRevisada > ValorMejorPelicula)
        {
            MejorPelicula = ListaPeliculasCandidatas[index];
            ValorMejorPelicula = ValorDePeliculaRevisada;
        }

        return MejorPelicula;
    }

    private float ValorSegunElReporte(PeliculaSO pelicula)
    {
        return pelicula.CalifIMDB - Math.Abs((float)pelicula.Duracion.TotalMinutes - 120);
    }

    private float ValorSegunCalificacionPorTiempo(PeliculaSO pelicula)
    {
        return pelicula.CalifIMDB/(float)(pelicula.Duracion.TotalSeconds);
    }   

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 300, 200), "Step"))
        {
            Step = true;
        }
    }
}
