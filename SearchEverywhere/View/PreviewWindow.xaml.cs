using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace SearchEverywhere.View;

public partial class PreviewWindow : Window, IView
{
    public static Action<bool> TopMostAction;

    public PreviewWindow()
    {
        InitializeComponent();
        TopMostAction = sw => { Topmost = sw; };
        WeakReferenceMessenger.Default.Register<PreviewWindow, string, string>(this, "CloseWindowToken",
            (r, msg) => CloseWindowUtility());
        WeakReferenceMessenger.Default.Register<PreviewWindow, string, string>(this, "AddSingletonWindowPreview",
            (r, msg) =>
            {
                PreviewView.instance ??= new PreviewView();
                if (PreviewView.instance != null && !r.RootGird.Children.Contains(PreviewView.instance))
                    r.RootGird.Children.Add(PreviewView.instance);
                WeakReferenceMessenger.Default.Send("", "RefreshWidthHeightToken");
            });
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
            CloseWindowUtility();
    }

    private void CloseWindowUtility()
    {
        Hide();
        WeakReferenceMessenger.Default.Send("true", "IsSmallWindowToken");
        if (PreviewView.instance != null)
            RootGird.Children.Remove(PreviewView.instance);
        WeakReferenceMessenger.Default.Send("true", "AddMainWindowPreview");
    }

    private void RootGird_Loaded(object sender, RoutedEventArgs e)
    {
        //PreviewView.instance ??= new PreviewView();
        //RootGird.Children.Add(PreviewView.instance);
    }
}

public interface IView
{
    void ShowWindow();
}