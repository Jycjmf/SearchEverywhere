using System.IO;
using System.Reflection;
using Config.Net;
using SearchEverywhere.Model;

namespace SearchEverywhere.Utility;

internal class ConfigurationUtility
{
    public AppSettings appSettings;

    public ConfigurationUtility()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        path += "\\setting.ini";
        appSettings = new ConfigurationBuilder<AppSettings>().UseIniFile(path).Build();
    }
}