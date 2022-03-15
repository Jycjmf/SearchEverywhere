using System;
using System.Linq;
using Microsoft.Win32;
using SearchEverywhere.Model;

namespace SearchEverywhere.Utility;

internal class AutoRunUtility
{
    public SuccessModel<string> SetAutoStart()
    {
        var res = new SuccessModel<string>(true, "Success");
        try
        {
            var reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            var path = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName;
            reg.SetValue("SearchEverywhere", path);
            return res;
        }
        catch (Exception e)
        {
            return new SuccessModel<string>(false, e.Message);
        }
    }

    public SuccessModel<string> DeleteAutoStart()
    {
        var res = new SuccessModel<string>(true, "Success");
        try
        {
            var reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (reg.GetValueNames().Contains("SearchEverywhere")) reg.DeleteValue("SearchEverywhere");
            return res;
        }
        catch (Exception e)
        {
            return new SuccessModel<string>(false, e.Message);
        }
    }
}