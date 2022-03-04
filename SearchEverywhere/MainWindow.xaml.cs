using System;
using System.Windows;

namespace SearchEverywhere;

public partial class MainWindow
{
    public static Action<bool> ChangeWindowStateAction;

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
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //  Visibility = Visibility.Hidden;
        //var hotkey = new HotKeyUtility();
    }
}