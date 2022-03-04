using System;
using System.Windows;

namespace SearchEverywhere
{
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
            Console.WriteLine(FirCol.ActualWidth);
            SearchBar.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(FirCol.ActualWidth);
            Console.WriteLine("load");
            //  Visibility = Visibility.Hidden;
            //var hotkey = new HotKeyUtility();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Console.WriteLine(FirCol.ActualWidth);
        }
    }
}