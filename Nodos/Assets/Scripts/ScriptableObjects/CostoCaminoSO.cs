using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Costo del camino")]
public class CostoCaminoSO : ScriptableObject
{
    [Obsolete]public SistemaMetro.EstacionesConTransbordes[] Estaciones = null;
    public CostoEstacionSO[] EstacionesSO = null;
    public int costo = 0;

    public CostoEstacionSO[] GetEstaciones()
    {
        return EstacionesSO;
    }

}
