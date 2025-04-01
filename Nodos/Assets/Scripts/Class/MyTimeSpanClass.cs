using System.Globalization;
using System;
using UnityEngine;

[Serializable]
public class MyTimeSpanClass
{
    public const long millisPerHour = 3600000;
    public const long millisPerMinute = 60000;
    public const long millisPerSecond = 1000;
    public const int secondsPerMinute = 60;
    public const int minutesPerHour = 60;

    public int hours, minutes, seconds;

    public MyTimeSpanClass(int hours, int minutes, int seconds)
    {
        this.hours = hours;
        this.minutes = minutes;
        this.seconds = seconds;
    }

    public double TotalMilliseconds
    {
        get
        {
            return hours * millisPerHour + minutes * millisPerMinute + seconds * millisPerSecond;
        }
    }
    public double TotalMinutes
    {
        get
        {
            return hours * minutesPerHour + minutes + seconds / (float)secondsPerMinute;
        }
    }
    public double TotalSeconds
    {
        get
        {
            return hours * minutesPerHour * secondsPerMinute + minutes * secondsPerMinute;
        }
    }

}
