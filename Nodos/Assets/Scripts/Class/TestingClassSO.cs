using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Testing/TestClassSO")]
public class TestingClassSO : ScriptableObject
{
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum TestingEnum
    {
        largo,
        chico,
        xd
    }
    [SerializeField] public TestingEnum testingEnum;
    [SerializeField] public float testingfloat;
    [SerializeField] public MyDateTimeClass testingDateTimeClass;
    [SerializeField] public MyTimeSpanClass testingTimeSpanClass;

    public TestingClassSO()
    {

    }
}
