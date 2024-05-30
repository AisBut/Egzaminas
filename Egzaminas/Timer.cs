using System;
using System.Diagnostics;

public class Timer
{
    private readonly Stopwatch stopwatch;

    public Timer()
    {
        stopwatch = new Stopwatch();
    }

    public void Start()
    {
        stopwatch.Start();
    }

    public void Stop()
    {
        stopwatch.Stop();
    }

    public double GetElapsedTime()
    {
        return stopwatch.Elapsed.TotalSeconds;
    }
}

