using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class VideoSliderModel : ObservableObject
{
    private double currentValue;
    private double maxValue;

    public VideoSliderModel(double currentValue, double maxValue)
    {
        this.currentValue = currentValue;
        this.maxValue = maxValue;
    }

    public double MaxValue
    {
        get => maxValue;
        set
        {
            SetProperty(ref maxValue, value);
            OnPropertyChanged();
        }
    }

    public double CurrentValue
    {
        get => currentValue;
        set
        {
            SetProperty(ref currentValue, value);
            OnPropertyChanged();
        }
    }
}