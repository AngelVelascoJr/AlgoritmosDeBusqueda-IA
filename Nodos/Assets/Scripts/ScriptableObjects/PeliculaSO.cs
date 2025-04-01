using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pelicula")]
public class PeliculaSO : ScriptableObject
{
    public string PeliName;
    public float CalifIMDB;
    public MyTimeSpanClass Duracion;
    public MyDateTimeClass FechaDeLanzamiento;

}
