﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SearchEverywhere.Model;
using SearchEverywhere.Utility;

namespace SearchEverywhere.ViewModel;

public class MainViewModel : ObservableRecipient
{
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private int selectIndex;

    public int SelectIndex
    {
        get => selectIndex;
        set
        {
            SetProperty(ref selectIndex, value);
            Console.WriteLine(searchResultList.Count);
            if (searchResultList.Count != 0 && value != -1)
                CurrentApp = searchResultList[value];
            else
                CurrentApp = null;
            OnPropertyChanged();
        }
    }

    private ListItemModel currentApp;

    public ListItemModel CurrentApp
    {
        get => currentApp;
        set
        {
            SetProperty(ref currentApp, value);
            OnPropertyChanged();
        }
    }

    private ObservableCollection<ListItemModel> runningAppsList = new();

    private Dictionary<int, bool> selectedItem = new() {{0, true}, {1, false}, {2, false}, {3, false}};


    public MainViewModel()
    {
        Task.Run(GetTaskBarApps);
        ChangeItemCommand = new RelayCommand<object>(x => { Console.WriteLine(x); });
        InputCommand = new RelayCommand(() =>
        {
            var index = SelectedItem.First(i => i.Value).Key;
            if (index == 3)
            {
                SelectedItem[3] = false;
                SelectedItem[0] = true;
            }
            else
            {
                SelectedItem[index++] = false;
                SelectedItem[index] = true;
            }

            OnPropertyChanged("SelectedItem");
        });
        EnterCommand = new RelayCommand(() =>
        {
            if (CurrentApp == null)
                return;
            ShowWindow(currentApp.Hwnd, 0);
            ShowWindow(currentApp.Hwnd, 5);
        });
        UpCommand = new RelayCommand(() =>
        {
            if (SelectIndex == 0)
                SelectIndex = SearchResultList.Count - 1;
            else
                SelectIndex--;
        });
        DownCommand = new RelayCommand(() =>
        {
            if (SelectIndex == SearchResultList.Count - 1)
                SelectIndex = 0;
            else
                SelectIndex++;
        });
    }

    private ObservableCollection<ListItemModel> searchResultList = new();

    public ObservableCollection<ListItemModel> SearchResultList
    {
        get => searchResultList;
        set
        {
            SetProperty(ref searchResultList, value);
            OnPropertyChanged();
        }
    }

    private string keyword;

    public string Keyword
    {
        get => keyword;
        set
        {
            SetProperty(ref keyword, value);
            StartSearch(value);
            OnPropertyChanged();
        }
    }

    private void StartSearch(string keyword)
    {
        keyword = keyword.Replace(@"\", @"\\");
        var raw = runningAppsList
            .Where(x => Regex.Matches(x.Title, keyword, RegexOptions.IgnoreCase).Count > 0)
            .ToList();
        var temp = new List<ListItemModel>();
        foreach (var each in raw)
            temp.Add(new ListItemModel(each.Icon,
                Regex.Replace(each.Title, keyword, $"|~S~|{keyword}|~E~|", RegexOptions.IgnoreCase), each.Hwnd,
                each.CreateTime, each.Size));
        SearchResultList = new ObservableCollection<ListItemModel>(temp);
        SelectIndex = 0;
    }

    public ObservableCollection<ListItemModel> RunningAppsList
    {
        get => runningAppsList;
        set
        {
            SetProperty(ref runningAppsList, value);
            CurrentApp = runningAppsList.First();
            OnPropertyChanged();
        }
    }

    public Dictionary<int, bool> SelectedItem
    {
        get => selectedItem;
        set
        {
            SetProperty(ref selectedItem, value);
            OnPropertyChanged();
        }
    }

    public ICommand ChangeItemCommand { get; }
    public ICommand InputCommand { get; }
    public ICommand EnterCommand { get; }
    public ICommand UpCommand { get; }
    public ICommand DownCommand { get; }

    private void GetTaskBarApps()
    {
        var processes = Process.GetProcesses();
        foreach (var item in processes)
            if (item.MainWindowTitle.Length > 0)
                Application.Current.Dispatcher.InvokeAsync(async () =>
                {
                    var icon = IconUtility.GetIcon(item.MainModule.FileName);
                    var tempItem = new ListItemModel(icon, item.MainWindowTitle, item.MainWindowHandle,
                        item.StartTime, GetRamUsage(item));
                    RunningAppsList.Add(tempItem);
                    SearchResultList.Add(tempItem);
                    SelectIndex = 0;
                });
    }

    private string GetRamUsage(Process process)
    {
        var memsize = 0; // memsize in KB
        //var PC = new PerformanceCounter();
        //PC.CategoryName = "Process";
        //PC.CounterName = "Working Set - Private";
        //PC.InstanceName = process.ProcessName;
        //memsize = Convert.ToInt32(PC.NextValue()) / (1024 * 1024);
        //PC.Close();
        //PC.Dispose();
        return $"{memsize} MB";
    }

    private void RegisterHotkey()
    {
    }
}