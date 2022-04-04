using System.Diagnostics;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;

namespace SearchEverywhere.ViewModel;

public class AboutViewModel
{
    public AboutViewModel()
    {
        GoToGithubCommand = new RelayCommand(() => { Process.Start("https://github.com/Jycjmf/SearchEverywhere"); });
    }

    public ICommand GoToGithubCommand { get; }
}