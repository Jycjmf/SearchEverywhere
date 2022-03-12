using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SearchEverywhere.Model;
using SearchEverywhere.Utility;

namespace SearchEverywhere.Everything;

public class Everything
{
    private const int EVERYTHING_REQUEST_FILE_NAME = 0x00000001;
    private const int EVERYTHING_REQUEST_PATH = 0x00000002;
    private const int EVERYTHING_REQUEST_SIZE = 0x00000010;
    private const int EVERYTHING_REQUEST_DATE_MODIFIED = 0x00000040;


    [DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
    public static extern uint Everything_SetSearchW(string lpSearchString);

    [DllImport("Everything64.dll")]
    public static extern void Everything_SetMatchPath(bool bEnable);

    [DllImport("Everything64.dll")]
    public static extern void Everything_SetMatchCase(bool bEnable);

    [DllImport("Everything64.dll")]
    public static extern void Everything_SetMatchWholeWord(bool bEnable);

    [DllImport("Everything64.dll")]
    public static extern void Everything_SetRegex(bool bEnable);

    [DllImport("Everything64.dll")]
    public static extern void Everything_SetMax(uint dwMax);

    [DllImport("Everything64.dll")]
    public static extern void Everything_SetOffset(uint dwOffset);


    [DllImport("Everything64.dll")]
    public static extern bool Everything_QueryW(bool bWait);

    [DllImport("Everything64.dll")]
    public static extern uint Everything_GetNumResults();

    [DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
    public static extern void Everything_GetResultFullPathName(uint nIndex, StringBuilder lpString, uint nMaxCount);

    [DllImport("Everything64.dll", CharSet = CharSet.Unicode)]
    public static extern IntPtr Everything_GetResultFileName(uint nIndex);

    // Everything 1.4

    [DllImport("Everything64.dll")]
    public static extern void Everything_SetRequestFlags(uint dwRequestFlags);

    [DllImport("Everything64.dll")]
    public static extern bool Everything_GetResultSize(uint nIndex, out long lpFileSize);

    [DllImport("Everything64.dll")]
    public static extern bool Everything_GetResultDateModified(uint nIndex, out long lpFileTime);


    public async Task<SuccessModel<List<ListItemModel>>> SearchFileAsync(string keyword, uint limit = 2000)
    {
        var resList = new SuccessModel<List<ListItemModel>>(true, new List<ListItemModel>());
        await Task.Run(() =>
        {
            Everything_SetSearchW(keyword);
            var isSuccess = Everything_QueryW(true);
            if (!isSuccess)
            {
                resList.IsSuccess = false;
                return;
            }

            var totalResCount = Everything_GetNumResults() <= limit ? Everything_GetNumResults() : limit;
            var buffer = new StringBuilder(256);
            for (uint i = 0; i < totalResCount; i++)
            {
                var title = Marshal.PtrToStringUni(Everything_GetResultFileName(i));
                Everything_GetResultDateModified(i, out var date_modified);
                Everything_GetResultSize(i, out var size);
                if (size < 1024)
                    continue;
                Everything_GetResultFullPathName(i, buffer, 256);
                var path = buffer.ToString();
                var modifyTime = DateTime.FromFileTime(date_modified);
                var sizeString = FileUtility.ConvertSize(size);
                var extension = Path.GetExtension(path);
                var svgIcon = ConvertIcon(extension);
                resList.Result.Add(new ListItemModel(null, title, IntPtr.Zero, modifyTime, sizeString, path, extension,
                    svgIcon, 0));
            }
        });

        return resList;
    }

    public void InitSearch(uint limit = 1000, uint offset = 0)
    {
        Everything_SetRequestFlags(EVERYTHING_REQUEST_FILE_NAME | EVERYTHING_REQUEST_PATH |
                                   EVERYTHING_REQUEST_DATE_MODIFIED | EVERYTHING_REQUEST_SIZE);
        Everything_SetOffset(offset);
        //  Everything_SetMax(limit);
    }

    private string ConvertIcon(string extension)
    {
        if (Regex.Matches(extension, @"exe|msi", RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/exe.svg";
        if (Regex.Matches(extension, @"ppt|pptx|pptm|potx|pps", RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/powerpoint.svg";
        if (Regex.Matches(extension, @"doc|docx|rtf", RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/word.svg";
        if (Regex.Matches(extension, @"xlsx|xls|csv|tsv", RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/table.svg";
        if (Regex.Matches(extension, @"png|jpeg|jpg|gif|ico|psd|bmp|img|raw|eps", RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/image.svg";
        if (Regex.Matches(extension, @"ini|dlc|dll|config|conf|prop|proerties|settings|option|props|prefs|cfg|yaml",
                RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/settings.svg";
        if (Regex.Matches(extension, @"mkv|webm|flv|avi|mp4|m4v|mpg|mpeg|mov", RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/video.svg";
        if (Regex.Matches(extension, @"mp3|flac|m4a|wma|wav|ape", RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/audio.svg";
        if (Regex.Matches(extension, @"txt", RegexOptions.IgnoreCase).Count > 0)
            return "img/icons/document.svg";
        return "img/icons/file.svg";
    }
}