using Config.Net;
using SearchEverywhere.Model;

namespace SearchEverywhere.Utility;

internal class ConfigurationUtility
{
    private readonly AppSettings appSettings;

    public ConfigurationUtility()
    {
        appSettings = new ConfigurationBuilder<AppSettings>().UseIniFile("setting.ini").Build();
    }

    public bool IsFirstUse()
    {
        return appSettings.FirstUse;
    }

    public bool StartWithWindows()
    {
        return appSettings.StartWithWindows;
    }

    public void SetFirstUsage(bool usage)
    {
        appSettings.FirstUse = false;
    }

    public void SetAutoStart(bool autoStart)
    {
        appSettings.StartWithWindows = autoStart;
    }
}