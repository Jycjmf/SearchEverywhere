using System;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class CurrentTimeModel : ObservableObject
{
    private TimeSpan currentTime;

    private TimeSpan totalTime;

    public CurrentTimeModel(TimeSpan currentTime, TimeSpan totalTime)
    {
        CurrentTime = currentTime;
        TotalTime = totalTime;
    }

    public TimeSpan CurrentTime
    {
        get => currentTime;
        set
        {
            SetProperty(ref currentTime, value);
            OnPropertyChanged();
        }
    }

    public TimeSpan TotalTime
    {
        get => totalTime;
        set
        {
            SetProperty(ref totalTime, value);
            OnPropertyChanged();
        }
    }
}