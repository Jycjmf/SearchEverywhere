using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace SearchEverywhere.View;

/// <summary>
///     Interaction logic for PreviewWindow.xaml
/// </summary>
public partial class PreviewWindow : Window, IView
{
    public PreviewWindow()
    {
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<PreviewWindow, string, string>(this, "CloseWindowToken",
            (r, msg) => { Hide(); });
        AddHandler(KeyUpEvent, new KeyEventHandler(ShortcutHandler), true);
    }

    public void ShowWindow()
    {
        ShowDialog();
    }

    private void ShortcutHandler(object sender, KeyEventArgs e)
    {
        if (e == null) return;
        if (e.Key == Key.Escape)
            Hide();
    }
}

public interface IView
{
    void ShowWindow();
}