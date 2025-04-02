using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;

public class ConvertSOToFile : MonoBehaviour
{
    [Header("DataToSerialize")]
    [SerializeField] private List<CostoCaminoSO> Caminos;
    [SerializeField] private List<CostoEstacionSO> Estaciones;
    [SerializeField] private List<PeliculaSO> Peliculas;

    [Header("Testing")]
    [SerializeField] bool testing;
    [SerializeField] private int testingInt;
    
    [SerializeField] private TestingClassSO testingClassObj;
    [SerializeField] private List<TestingClassSO> testingList;

    void Start()
    {
        //if(testing)
        //{
        //    TestSaveFile();
        //}
    }

    private void SaveSOToFiles()
    {
        int count = 0;
        foreach (var camino in Caminos)
        {
            string filename = $"{camino.name}.txt";
            string path = Application.dataPath + $"/SOToFile/Caminos/{filename}";

            string message = JsonConvert.SerializeObject(camino);

            Debug.Log($"{count}.- " + message);

            CreateFile(path, message);
            count++;
        }
        Debug.Log($"<color=red>Guardados {count} Caminos</color>");
        count = 0;
        foreach (var estacion in Estaciones)
        {
            string filename = $"{estacion.name}.txt";
            string path = Application.dataPath + $"/SOToFile/Estaciones/{filename}";

            string message = JsonConvert.SerializeObject(estacion);

            Debug.Log($"{count}.- " + message);

            CreateFile(path, message);
            count++;
        }
        Debug.Log($"<color=red>Guardados {count} Estaciones</color>");
        count = 0;
        foreach (var pelicula in Peliculas)
        {
            string filename = $"{pelicula.name}.txt";
            string path = Application.dataPath + $"/SOToFile/Peliculas/{filename}";

            string message = JsonConvert.SerializeObject(pelicula);

            Debug.Log($"{count}.- " + message);

            CreateFile(path, message);
            count++;
        }
        Debug.Log($"<color=red>Guardados {count} Peliculas</color>");
        Debug.Log("<color=yellow>Fin del programa</color>");
    }

    private void TestSaveFile()
    {
        string filename = "xd.txt";
        string path = Application.dataPath + $"/SOToFile/testing/{filename}";

        string message = JsonConvert.SerializeObject(testingClassObj);

        Debug.Log(message);
                
        CreateFile(path, message);
    }

    private void TestSaveListFile()
    {
        int count = 0;
        foreach (var testing in testingList)
        {
            string filename = $"xd{count}.txt";
            string path = Application.dataPath + $"/SOToFile/testing/{filename}";

            string message = JsonConvert.SerializeObject(testing);

            Debug.Log($"{count}.- "+message);

            CreateFile(path, message);
            count++;
        }
    }

    private void CreateFile(string path, string content)
    {
        File.WriteAllText(path, content);
        //Console.WriteLine("File created and text written successfully.");
    }

    private void OnGUI()
    {
        if (!testing)
        {
            if (GUI.Button(new Rect(Screen.width - 300, Screen.height - 200, 300, 200), "Save SO To Files"))
            {
                SaveSOToFiles();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width - 300, Screen.height - 200, 300, 200), "Debug create file"))
            {
                TestSaveFile();
            }
            if (GUI.Button(new Rect(Screen.width - 600, Screen.height - 200, 300, 200), "Debug List"))
            {
                TestSaveListFile();
            }
        }
    }

}
