using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Windows;

namespace SearchEverywhere.Utility;

internal class CheckPermissionUtility
{
    public void ElevatePermission()
    {
        var exeName = Assembly.GetExecutingAssembly().Location;
        var startInfo = new ProcessStartInfo(exeName)
        {
            Verb = "runas",
            Arguments = "restart"
        };
        Process.Start(startInfo);
        Application.Current.Shutdown();
    }

    public bool IsAdministrator()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}