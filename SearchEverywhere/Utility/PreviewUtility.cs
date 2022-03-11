using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Toolkit.Mvvm.Messaging;
using SearchEverywhere.Model;

namespace SearchEverywhere.Utility;

public class PreviewUtility
{
    public void TryToPreview(string path)
    {
        if (path == null)
            return;
        var extension = Path.GetExtension(path);
        if (Regex.Matches(extension, @"png|jpeg|jpg|gif|ico|bmp", RegexOptions.IgnoreCase).Count > 0)
        {
            WeakReferenceMessenger.Default.Send(
                new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Image, true,
                    path), "StartPreview");
            return;
        }

        if (Regex.Matches(extension, @"mkv|webm|flv|avi|mp4|m4v|mpg|mpeg|mov", RegexOptions.IgnoreCase).Count > 0)
        {
            WeakReferenceMessenger.Default.Send(
                new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Video, true,
                    path), "StartPreview");
            return;
        }

        if (Regex.Matches(extension, @"mp3|flac|m4a|wma|wav|ape", RegexOptions.IgnoreCase).Count > 0)
        {
            WeakReferenceMessenger.Default.Send(
                new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Audio, true,
                    path), "StartPreview");
            return;
        }

        if (Regex.Matches(extension, @"xlsx|xls", RegexOptions.IgnoreCase).Count > 0)
        {
            WeakReferenceMessenger.Default.Send(
                new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Excel, true,
                    path), "StartPreview");
            return;
        }

        if (Regex.Matches(extension, @"ppt|pptx", RegexOptions.IgnoreCase).Count > 0)
        {
            WeakReferenceMessenger.Default.Send(
                new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Ppt, true,
                    path), "StartPreview");
            return;
        }

        if (Regex.Matches(extension, @"docx", RegexOptions.IgnoreCase).Count > 0)
        {
            WeakReferenceMessenger.Default.Send(
                new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Word, true,
                    path), "StartPreview");
            return;
        }

        if (Regex.Matches(extension, @"txt|csv|tsv|xml", RegexOptions.IgnoreCase).Count > 0)
        {
            WeakReferenceMessenger.Default.Send(
                new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Text, true,
                    path), "StartPreview");
            return;
        }

        if (Regex.Matches(extension, @"config|inf|ini", RegexOptions.IgnoreCase).Count > 0)
        {
            WeakReferenceMessenger.Default.Send(
                new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Config, true,
                    path), "StartPreview");
            return;
        }

        WeakReferenceMessenger.Default.Send(
            new PreviewUiElementModel(PreviewUiElementModel.PreviewUiElement.Unknown, true,
                path), "StartPreview");
    }
}