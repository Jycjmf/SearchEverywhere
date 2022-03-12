using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using SearchEverywhere.ViewModel;

namespace SearchEverywhere;

public partial class MainWindow
{
    public static Action<bool> ChangeWindowStateAction;
    private readonly MainViewModel vm;

    public MainWindow()
    {
        ChangeWindowStateAction = x =>
        {
            Dispatcher.Invoke(() =>
            {
                if (x)
                    Visibility = Visibility.Visible;
                else
                    Visibility = Visibility.Hidden;
            });
        };
        InitializeComponent();
        SearchBar.Focus();
        AddHandler(KeyUpEvent, new KeyEventHandler(ShortcutHandler));
        vm = Ioc.Default.GetService<MainViewModel>();
    }

    private void ShortcutHandler(object sender, KeyEventArgs e)
    {
        if (e == null)
            return;
        switch (e.Key)
        {
            case Key.Enter when Keyboard.Modifiers == ModifierKeys.Control:
                vm.PreviewCommand.Execute(null);
                break;
            case Key.Enter when Keyboard.Modifiers == ModifierKeys.Alt:
                vm.FullscreenCommand.Execute(null);
                break;
            case Key.Up:
                vm.UpCommand.Execute(null);
                return;
            case Key.Down:
                vm.DownCommand.Execute(null);
                return;
            case Key.Enter:
                vm.EnterCommand.Execute(null);
                return;
            case Key.Tab:
                vm.InputTabCommand.Execute(null);
                return;
        }
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //  Visibility = Visibility.Hidden;
        //var hotkey = new HotKeyUtility();
    }
}