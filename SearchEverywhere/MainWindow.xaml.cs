using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using SearchEverywhere.Utility;
using SearchEverywhere.View;
using SearchEverywhere.ViewModel;

namespace SearchEverywhere;

public partial class MainWindow
{
    public static Action<bool> ChangeWindowStateAction;
    private readonly ConfigurationUtility config = Ioc.Default.GetService<ConfigurationUtility>();
    private readonly MainViewModel vm;

    public MainWindow()
    {
        vm = Ioc.Default.GetService<MainViewModel>();
        ChangeWindowStateAction = x =>
        {
            Dispatcher.Invoke(() =>
            {
                if (x)
                {
                    Visibility = Visibility.Visible;
                    Activate();
                    PreviewControl.Focus();
                }

                else
                {
                    Visibility = Visibility.Hidden;
                }
            });
        };
        AddHandler(PreviewKeyDownEvent, new KeyEventHandler(ShortcutHandler));
        InitializeComponent();
    }

    private void ShortcutHandler(object sender, KeyEventArgs e)
    {
        if (e == null)
            return;

        if (e.KeyboardDevice.Modifiers == ModifierKeys.Alt)
            switch (e.SystemKey)
            {
                case Key.Enter:
                    if (PreviewView.instance != null)
                        SearchView.GetPreviewBorderFunc()?.Children.Remove(PreviewView.instance);
                    WeakReferenceMessenger.Default.Send("true", "AddSingletonWindowPreview");
                    vm.FullscreenCommand.Execute(null);
                    return;
            }

        switch (e.Key)
        {
            case Key.Enter when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                vm.PreviewCommand.Execute(null);
                break;
            case Key.S when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                SearchView.GetSearchbarFunc().Focus();
                break;
            case Key.Up:
                vm.UpCommand.Execute(null);
                return;
            case Key.Down:
                vm.DownCommand.Execute(null);
                return;
            case Key.Enter:
                vm.EnterCommand.Execute(null);
                ChangeWindowStateAction(false);
                return;
            case Key.Tab:
                vm.InputTabCommand.Execute(null);
                SearchView.GetSearchbarFunc().Focus();
                return;
            case Key.Escape:
                ChangeWindowStateAction(false);
                return;
        }
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (config.appSettings.SilentWindows) Visibility = Visibility.Hidden;
        var hotkey = new HotKeyUtility();
    }

    private void MainWindow_OnClosing(object sender, CancelEventArgs e)
    {
        Environment.Exit(0);
    }
}