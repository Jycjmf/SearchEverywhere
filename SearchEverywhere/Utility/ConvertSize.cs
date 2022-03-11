using System;

namespace SearchEverywhere.Utility;

public class FileUtility
{
    public static string ConvertSize(long size)
    {
        if (size < 1024)
            return $"{size} B";
        if (size < 1024 * 1024)
            return $"{Math.Round(size / 1024f, 1)} KB";
        if (size < Math.Pow(1024, 3))
            return $"{Math.Round(size / Math.Pow(1024, 2), 1)} MB";
        return $"{Math.Round(size / Math.Pow(1024, 3), 1)} GB";
    }
}