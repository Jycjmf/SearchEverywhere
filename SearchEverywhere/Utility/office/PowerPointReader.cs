using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using SearchEverywhere.Model;

namespace SearchEverywhere.Utility.office;

internal class PowerPointReader
{
    public async Task<SuccessModel<string>> ReadPowerPointAsync(string path)
    {
        var res = new SuccessModel<string>(true, null);
        var targetPath = string.Empty;
        await Task.Run(() =>
        {
            try
            {
                targetPath = Directory.GetCurrentDirectory() + "\\temp\\ppt";
                if (Directory.Exists(targetPath))
                    Directory.Delete(targetPath, true);
                var pptApp = new Application();
                var ppt = pptApp.Presentations.Open(path, WithWindow: MsoTriState.msoFalse);
                Directory.CreateDirectory(targetPath);
                foreach (Slide each in ppt.Slides)
                    each.Export($"{targetPath}\\{each.SlideIndex}.png", "png");
                pptApp.Quit();
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                res.Result = e.Message;
            }
        });
        res.Result = targetPath;
        return res;
    }
}