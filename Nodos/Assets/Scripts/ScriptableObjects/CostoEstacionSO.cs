using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Costo de la estacion")]
public class CostoEstacionSO : ScriptableObject
{
    public SistemaMetro.EstacionesConTransbordes Estacion;
    public int costo;
}
