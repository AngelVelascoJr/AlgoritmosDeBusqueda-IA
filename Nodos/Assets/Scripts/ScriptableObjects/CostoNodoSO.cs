using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Costo del nodo")]
public class CostoNodoSO : ScriptableObject
{
    public SistemaMetro.EstacionesConTransbordes Estacion;
    public int costo;
}
