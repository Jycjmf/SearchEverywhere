using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using SearchEverywhere.ViewModel;

namespace SearchEverywhere;

/// <summary>
///     App.xaml 的交互逻辑
/// </summary>
public partial class App : Application
{
    public App()
    {
        Ioc.Default.ConfigureServices(
            new ServiceCollection()
                .AddSingleton<MainViewModel>()
                .AddSingleton<PreviewViewModel>()
                .BuildServiceProvider());
    }
}