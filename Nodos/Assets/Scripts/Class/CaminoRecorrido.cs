using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class CaminoRecorrido
{
    [ReadOnly] public SistemaMetro.EstacionesConTransbordes estacionActualVisual;
    public CostoNodoSO estacionActualSO;
    public int CostoCaminoRecorrido = 0;

    public bool PuedeSeguir = true;

    public CaminoRecorrido(CostoNodoSO nodoSO, int CostoCaminoRecorrido)
    {
        estacionActualSO = nodoSO;
        this.CostoCaminoRecorrido = CostoCaminoRecorrido;    
    }

    public void DetenerRecorrido()
    {
        PuedeSeguir = false;
    }

    public bool GetPuedeSeguir()
    {
        return PuedeSeguir;
    }
    /*
    public void AddEstacionToRecorridas(SistemaMetro.EstacionesConTransbordes estacion)
    {
        estacionesRecorridas.Add(estacion);
    }

    public List<SistemaMetro.EstacionesConTransbordes> GetEstacionesRecorridas()
    {
        return estacionesRecorridas;
    }*/

    public CostoNodoSO GetEstacionActualSO()
    {
        return estacionActualSO;
    }

    public void SetEstacionActual(CostoNodoSO Estacion)
    {
        estacionActualSO = Estacion;
        estacionActualVisual = estacionActualSO.Estacion;
    }

    public void AddToCostoCaminoRecorrido(int Costo)
    {
        CostoCaminoRecorrido += Costo;
    }



}
