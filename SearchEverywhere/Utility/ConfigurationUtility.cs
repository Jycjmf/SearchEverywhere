using Config.Net;
using SearchEverywhere.Model;

namespace SearchEverywhere.Utility;

internal class ConfigurationUtility
{
    public AppSettings appSettings;

    public ConfigurationUtility()
    {
        appSettings = new ConfigurationBuilder<AppSettings>().UseIniFile("setting.ini").Build();
    }
}