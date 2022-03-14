using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Threading.Tasks;
using HandyControl.Controls;
using Microsoft.Toolkit.Mvvm.Messaging;
using SearchEverywhere.Model;
using SearchEverywhere.Model.Message;

namespace SearchEverywhere.Utility;

internal class ProcessUtility
{
    private List<ListItemModel> processList = new();

    public async Task TrackNewProcess()
    {
        processList = await GetInitAppsAsync();
        WeakReferenceMessenger.Default.Send(processList, "InitAppListToken");
        StartTrackNewProcess();
    }

    private void StartTrackNewProcess()
    {
        var startWatch = new ManagementEventWatcher(
            new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
        startWatch.EventArrived += NewProcessEvent;
        startWatch.Start();
        var stopWatch = new ManagementEventWatcher(
            new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
        stopWatch.EventArrived += RemoveProcessEvent;
        stopWatch.Start();
    }

    private void RemoveProcessEvent(object sender, EventArrivedEventArgs e)
    {
        try
        {
            var ProcessId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
            if (!processList.Exists(x => x.ProcessId == ProcessId))
                return;
            var tempList = processList.Where(x => x.ProcessId != ProcessId).ToList();
            processList.Clear();
            tempList.ForEach(x => processList.Add(x));
            WeakReferenceMessenger.Default.Send(
                new RefreshProcessModel(false,
                    new ListItemModel(null, null, IntPtr.Zero, DateTime.Now, null, null, null, null, ProcessId)),
                "RefreshApplistToken");
        }
        catch (Exception exception)
        {
        }
    }

    private async void NewProcessEvent(object sender, EventArrivedEventArgs e)
    {
        try
        {
            var ProcessId = Convert.ToInt32(e.NewEvent.Properties["ProcessID"].Value);
            if (processList.Exists(x => x.ProcessId == ProcessId))
                return;
            var tempProcess = Process.GetProcessById(ProcessId);
            if (tempProcess.MainWindowTitle.Length > 0)
            {
                var processInfo = await GetProcessInfo(tempProcess);
                processList.Add(processInfo);
                WeakReferenceMessenger.Default.Send(new RefreshProcessModel(true, processInfo), "RefreshApplistToken");
            }
        }
        catch (Exception exception)
        {
        }
    }

    private async Task<List<ListItemModel>> GetInitAppsAsync()
    {
        try
        {
            var tempList = new List<ListItemModel>();
            await Task.Run(async () =>
            {
                var processes = Process.GetProcesses().Where(x => x.MainWindowTitle.Length > 0);
                foreach (var eachProcess in processes)
                {
                    if (eachProcess.MainModule == null)
                        continue;
                    var tempItem = await GetProcessInfo(eachProcess);
                    tempList.Add(tempItem);
                }

                Console.WriteLine("res");
            });
            return tempList;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            MessageBox.Show(e.ToString(), "Exception Catch");
            return new List<ListItemModel>();
        }
    }

    private async Task<ListItemModel> GetProcessInfo(Process eachProcess)
    {
        var icon = IconUtility.GetIcon(eachProcess.MainModule.FileName);
        var tempItem = new ListItemModel(icon, eachProcess.MainWindowTitle, eachProcess.MainWindowHandle,
            eachProcess.StartTime, await GetRamUsage(eachProcess), null, null, null, eachProcess.Id);
        return tempItem;
    }


    private async Task<string> GetRamUsage(Process process)
    {
        try
        {
            var memsize = 0; // memsize in KB
            var PC = new PerformanceCounter();
            var memoryString = string.Empty;
            await Task.Run(() =>
            {
                PC.CategoryName = "Process";
                PC.CounterName = "Working Set - Private";
                PC.InstanceName = process.ProcessName;
                memsize = Convert.ToInt32(PC.NextValue());
                memoryString = FileUtility.ConvertSize(memsize);
                PC.Close();
                PC.Dispose();
            });

            return memoryString;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Unknown KB";
        }
    }
}