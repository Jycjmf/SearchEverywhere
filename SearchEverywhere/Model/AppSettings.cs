using Config.Net;

namespace SearchEverywhere.Model;

public interface AppSettings
{
    [Option(DefaultValue = true)] public bool FirstUse { get; set; }

    [Option(DefaultValue = false)] public bool StartWithWindows { get; set; }
}