using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace SearchEverywhere.ViewModel;

public class ViewModelLocator
{
    public MainViewModel mainViewModel => Ioc.Default.GetService<MainViewModel>();
    public PreviewViewModel previewViewModel => Ioc.Default.GetService<PreviewViewModel>();
    public WizardViewModel wizardViewModel => Ioc.Default.GetService<WizardViewModel>();
    public SettingViewModel settingViewModel => Ioc.Default.GetService<SettingViewModel>();
}