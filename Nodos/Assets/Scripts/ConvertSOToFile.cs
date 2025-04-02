using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ConvertSOToFile : MonoBehaviour
{
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
        // Create the file and write to it
        File.WriteAllText(path, content);
        Console.WriteLine("File created and text written successfully.");
    }

    private void OnGUI()
    {
        if (!testing)
            return;
        if (GUI.Button(new Rect(Screen.width-300, Screen.height-200, 300, 200), "Debug create file"))
        {
            TestSaveFile();
        }
        if (GUI.Button(new Rect(Screen.width - 600, Screen.height - 200, 300, 200), "Debug List"))
        {
            TestSaveListFile();
        }
    }

}
