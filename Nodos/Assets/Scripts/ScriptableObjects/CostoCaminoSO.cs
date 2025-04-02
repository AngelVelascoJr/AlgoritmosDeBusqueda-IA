using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Costo del camino")]
public class CostoCaminoSO : ScriptableObject
{
    public CostoEstacionSO[] EstacionesSO = null;
    public int costo = 0;

    public CostoEstacionSO[] GetEstaciones()
    {
        return EstacionesSO;
    }

}
