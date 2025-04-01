using System.Globalization;
using System;
using UnityEngine;

[Serializable]
public class MyDateTimeClass
{
    public int year, month, day;

    public MyDateTimeClass(int year, int month, int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
    }

}
