using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace SearchEverywhere.Model;

public class AnimationModel : ObservableObject
{
    private bool excelAnimation;
    private bool loadingAnimation;
    private bool pptAnimation;

    private bool wordAnimation;

    public bool LoadingAnimation
    {
        get => loadingAnimation;
        set
        {
            SetProperty(ref loadingAnimation, value);
            OnPropertyChanged();
        }
    }

    public bool PptAnimation
    {
        get => pptAnimation;
        set
        {
            SetProperty(ref pptAnimation, value);
            OnPropertyChanged();
        }
    }

    public bool ExcelAnimation
    {
        get => excelAnimation;
        set
        {
            SetProperty(ref excelAnimation, value);
            OnPropertyChanged();
        }
    }

    public bool WordAnimation
    {
        get => wordAnimation;
        set
        {
            SetProperty(ref wordAnimation, value);
            OnPropertyChanged();
        }
    }
}