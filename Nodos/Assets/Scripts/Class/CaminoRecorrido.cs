using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class CaminoRecorrido
{
    public SistemaMetro.EstacionesConTransbordes estacionActualVisual;
    public CostoEstacionSO estacionActualSO;
    public List<CostoCaminoSO> CaminosRestantesPorRecorrer;
    public List<CostoCaminoSO> CaminosYaRecorridos;
    public int CostoCaminoRecorrido = 0;

    public bool PuedeSeguir = true;

    public CaminoRecorrido(CostoEstacionSO nodoSO, int CostoCaminoRecorrido, List<CostoCaminoSO> caminosPorRecorrer)
    {
        estacionActualSO = nodoSO;
        this.CostoCaminoRecorrido = CostoCaminoRecorrido;    
        CaminosRestantesPorRecorrer = caminosPorRecorrer;
        CaminosYaRecorridos = new();
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

    public CostoEstacionSO GetEstacionActualSO()
    {
        return estacionActualSO;
    }

    public void SetEstacionActual(CostoEstacionSO Estacion)
    {
        estacionActualSO = Estacion;
        estacionActualVisual = estacionActualSO.Estacion;
    }

    public void AddToCostoCaminoRecorrido(int Costo)
    {
        CostoCaminoRecorrido += Costo;
    }

}
