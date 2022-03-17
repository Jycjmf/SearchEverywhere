using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HandyControl.Controls;
using SearchEverywhere.Model;
using SearchEverywhere.Utility;

namespace SearchEverywhere.Everything;

public class Everything
{
    public const string EverythingVersion32 = @"Everything32.dll";
    public const string EverythingVersion64 = @"Everything64.dll";
    private const int EVERYTHING_REQUEST_FILE_NAME = 0x00000001;
    private const int EVERYTHING_REQUEST_PATH = 0x00000002;
    private const int EVERYTHING_REQUEST_SIZE = 0x00000010;
    private const int EVERYTHING_REQUEST_DATE_MODIFIED = 0x00000040;

    /// <summary>
    ///     32Bit
    /// </summary>
    [DllImport(EverythingVersion32, CharSet = CharSet.Unicode, EntryPoint = "Everything_SetSearchW")]
    public static extern uint SetSearch32(string lpSearchString);


    [DllImport(EverythingVersion32, EntryPoint = "Everything_GetMinorVersion")]
    public static extern uint GetVersion32();


    [DllImport(EverythingVersion32, EntryPoint = "Everything_SetOffset")]
    public static extern void SetOffset32(uint dwOffset);


    [DllImport(EverythingVersion32, EntryPoint = "Everything_QueryW")]
    public static extern bool QuerySearchResult32(bool bWait);

    [DllImport(EverythingVersion32, EntryPoint = "Everything_GetNumResults")]
    public static extern uint GetNumberResult32();

    [DllImport(EverythingVersion32, CharSet = CharSet.Unicode, EntryPoint = "Everything_GetResultFullPathName")]
    public static extern void GetResultFullPathName32(uint nIndex, StringBuilder lpString, uint nMaxCount);

    [DllImport(EverythingVersion32, CharSet = CharSet.Unicode, EntryPoint = "Everything_GetResultFileName")]
    public static extern IntPtr GetResultFileName32(uint nIndex);


    [DllImport(EverythingVersion32, EntryPoint = "Everything_SetRequestFlags")]
    public static extern void SetRequestFlags32(uint dwRequestFlags);

    [DllImport(EverythingVersion32, EntryPoint = "Everything_GetResultSize")]
    public static extern bool GetResultSize32(uint nIndex, out long lpFileSize);

    [DllImport(EverythingVersion32, EntryPoint = "Everything_GetResultDateModified")]
    public static extern bool GetResultDateModified32(uint nIndex, out long lpFileTime);

    /// <summary>
    ///     64Bit
    /// </summary>
    [DllImport(EverythingVersion64, CharSet = CharSet.Unicode, EntryPoint = "Everything_SetSearchW")]
    public static extern uint SetSearch64(string lpSearchString);


    [DllImport(EverythingVersion64, EntryPoint = "Everything_GetMinorVersion")]
    public static extern uint GetVersion64();


    [DllImport(EverythingVersion64, EntryPoint = "Everything_SetOffset")]
    public static extern void SetOffset64(uint dwOffset);


    [DllImport(EverythingVersion64, EntryPoint = "Everything_QueryW")]
    public static extern bool QuerySearchResult64(bool bWait);

    [DllImport(EverythingVersion64, EntryPoint = "Everything_GetNumResults")]
    public static extern uint GetNumberResult64();

    [DllImport(EverythingVersion64, CharSet = CharSet.Unicode, EntryPoint = "Everything_GetResultFullPathName")]
    public static extern void GetResultFullPathName64(uint nIndex, StringBuilder lpString, uint nMaxCount);

    [DllImport(EverythingVersion64, CharSet = CharSet.Unicode, EntryPoint = "Everything_GetResultFileName")]
    public static extern IntPtr GetResultFileName64(uint nIndex);


    [DllImport(EverythingVersion64, EntryPoint = "Everything_SetRequestFlags")]
    public static extern void SetRequestFlags64(uint dwRequestFlags);

    [DllImport(EverythingVersion64, EntryPoint = "Everything_GetResultSize")]
    public static extern bool GetResultSize64(uint nIndex, out long lpFileSize);

    [DllImport(EverythingVersion64, EntryPoint = "Everything_GetResultDateModified")]
    public static extern bool GetResultDateModified64(uint nIndex, out long lpFileTime);

    //General Method
    private uint SetSearch(string lpSearchString)
    {
        return Environment.Is64BitProcess ? SetSearch64(lpSearchString) : SetSearch32(lpSearchString);
    }


    private uint GetVersion()
    {
        return Environment.Is64BitProcess ? GetVersion64() : GetVersion32();
    }

    private void SetOffset(uint dwOffset)
    {
        if (Environment.Is64BitProcess)
            SetOffset64(dwOffset);
        else
            SetOffset32(dwOffset);
    }

    private bool QuerySearchResult(bool bWait)
    {
        return Environment.Is64BitProcess ? QuerySearchResult64(bWait) : QuerySearchResult32(bWait);
    }

    private uint GetNumberResult()
    {
        return Environment.Is64BitProcess ? GetNumberResult64() : GetNumberResult32();
    }

    private void GetResultFullPathName(uint nIndex, StringBuilder lpString, uint nMaxCount)
    {
        if (Environment.Is64BitProcess)
            GetResultFullPathName64(nIndex, lpString, nMaxCount);
        else
            GetResultFullPathName32(nIndex, lpString, nMaxCount);
    }

    private void SetRequestFlags(uint dwRequestFlags)
    {
        if (Environment.Is64BitProcess)
            SetRequestFlags64(dwRequestFlags);
        else
            SetRequestFlags32(dwRequestFlags);
    }

    private IntPtr GetResultFileName(uint nIndex)
    {
        return Environment.Is64BitProcess ? GetResultFileName64(nIndex) : GetResultFileName32(nIndex);
    }

    private bool GetResultSize(uint nIndex, out long lpFileSize)
    {
        return Environment.Is64BitProcess
            ? GetResultSize64(nIndex, out lpFileSize)
            : GetResultSize32(nIndex, out lpFileSize);
    }

    private bool GetResultDateModified(uint nIndex, out long lpFileTime)
    {
        return Environment.Is64BitProcess
            ? GetResultDateModified64(nIndex, out lpFileTime)
            : GetResultDateModified32(nIndex, out lpFileTime);
    }

    public async Task<SuccessModel<List<ListItemModel>>> SearchFileAsync(string keyword, uint limit = 2000)
    {
        var resList = new SuccessModel<List<ListItemModel>>(true, new List<ListItemModel>());
        await Task.Run(() =>
        {
            try
            {
                SetSearch(keyword);
                var isSuccess = QuerySearchResult(true);
                if (!isSuccess)
                {
                    resList.IsSuccess = false;
                    return;
                }

                var totalResCount = GetNumberResult() <= limit ? GetNumberResult() : limit;
                var buffer = new StringBuilder(256);
                for (uint i = 0; i < totalResCount; i++)
                {
                    var title = Marshal.PtrToStringUni(GetResultFileName(i));
                    GetResultDateModified(i, out var date_modified);
                    GetResultSize(i, out var size);
                    if (size < 1024)
                        continue;
                    GetResultFullPathName(i, buffer, 256);
                    var path = buffer.ToString();
                    var modifyTime = DateTime.FromFileTime(date_modified);
                    var sizeString = FileUtility.ConvertSize(size);
                    var extension = Path.GetExtension(path);
                    var svgIcon = ConvertIcon(extension);
                    resList.Result.Add(new ListItemModel(null, title, IntPtr.Zero, modifyTime, sizeString, path,
                        extension,
                        new Uri(svgIcon, UriKind.Relative), 0));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Exception Catch");
            }
        });

        return resList;
    }

    public void InitSearch(uint limit = 2000, uint offset = 0)
    {
        SetRequestFlags(EVERYTHING_REQUEST_FILE_NAME | EVERYTHING_REQUEST_PATH |
                        EVERYTHING_REQUEST_DATE_MODIFIED | EVERYTHING_REQUEST_SIZE);
        SetOffset(offset);
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