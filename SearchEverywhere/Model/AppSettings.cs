using Config.Net;

namespace SearchEverywhere.Model;

public interface AppSettings
{
    [Option(DefaultValue = true)] public bool FirstUse { get; set; }

    [Option(DefaultValue = false)] public bool StartWithWindows { get; set; }
    [Option(DefaultValue = false)] public bool SilentWindows { get; set; }
    [Option(DefaultValue = 14)] public int FontSize { get; set; }
    [Option(DefaultValue = 100)] public int SearchDelay { get; set; }
    [Option(DefaultValue = "#51459E")] public string ThemeColor { get; set; }
    [Option(DefaultValue = "#F9896B")] public string AccentColor { get; set; }
}