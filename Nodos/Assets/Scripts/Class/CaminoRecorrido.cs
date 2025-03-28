using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CaminoRecorrido
{
    [Obsolete] public SistemaMetro.EstacionesConTransbordes estacionActual;
    public CostoNodoSO estacionActualSO;
    public int CostoCaminoRecorrido = 0;

    public bool PuedeSeguir = true;

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

    [Obsolete] public SistemaMetro.EstacionesConTransbordes GetEstacionActual()
    {
        return estacionActual;
    }

    [Obsolete]public void SetEstacionActual(SistemaMetro.EstacionesConTransbordes Estacion)
    {
        estacionActual = Estacion;
    }

    public CostoNodoSO GetEstacionActualSO()
    {
        return estacionActualSO;
    }

    public void SetEstacionActual(CostoNodoSO Estacion)
    {
        estacionActualSO = Estacion;
    }

    public void AddToCostoCaminoRecorrido(int Costo)
    {
        CostoCaminoRecorrido += Costo;
    }



}
