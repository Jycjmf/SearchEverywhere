using System;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace SearchEverywhere.View;

/// <summary>
///     Interaction logic for SearchView.xaml
/// </summary>
public partial class SearchView : UserControl
{
    public static Func<Grid> GetPreviewBorderFunc;
    public static Func<SearchBar> GetSearchbarFunc;

    public SearchView()
    {
        InitializeComponent();
        GetPreviewBorderFunc = () => PreviewBorder;
        GetSearchbarFunc = () => SearchBar;
        WeakReferenceMessenger.Default.Register<SearchView, string, string>(this, "AddMainWindowPreview", (r, msg) =>
        {
            PreviewView.instance ??= new PreviewView();
            if (PreviewView.instance != null && !r.PreviewBorder.Children.Contains(PreviewView.instance))
                r.PreviewBorder.Children.Add(PreviewView.instance);
            WeakReferenceMessenger.Default.Send("", "RefreshWidthHeightToken");
        });
    }


    private void SearchView_OnLoaded(object sender, RoutedEventArgs e)
    {
        PreviewView.instance ??= new PreviewView();
        if (PreviewView.instance != null && !PreviewBorder.Children.Contains(PreviewView.instance))
            PreviewBorder.Children.Add(PreviewView.instance);
        WeakReferenceMessenger.Default.Send("", "RefreshWidthHeightToken");
    }

    private void SearchView_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
    }
}