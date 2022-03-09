using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using NPOI.XWPF.UserModel;
using SearchEverywhere.Model;

namespace SearchEverywhere.Utility.office;

internal class WordReader
{
    public async Task<SuccessModel<List<WordContentModel>>> ReadWordAsync(string path)
    {
        var res = new SuccessModel<List<WordContentModel>>(true, new List<WordContentModel>());
        var targetPath = string.Empty;
        await Task.Run(() =>
        {
            try
            {
                using var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var doc = new XWPFDocument(file);

                foreach (var each in doc.BodyElements)
                    if (each.GetType() == typeof(XWPFParagraph))
                    {
                        var p = (XWPFParagraph) each;
                        res.Result.Add(new WordContentModel(p.ParagraphText, null,
                            ConvertHorizontalAlign(p.Alignment)));
                        res.Result.AddRange(from eachRun in p.Runs
                            from img in eachRun.GetEmbeddedPictures()
                            select new WordContentModel(null, ToImage(img.GetPictureData().Data),
                                HorizontalAlignment.Left));
                    }

                Console.WriteLine(res.Result.Count);
            }
            catch (Exception e)
            {
                res.IsSuccess = false;
                Console.WriteLine(e);
            }
        });
        return res;
    }

    public BitmapImage ToImage(byte[] array)
    {
        using var ms = new MemoryStream(array);
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad; // here
        image.StreamSource = ms;
        image.EndInit();
        image.Freeze();
        return image;
    }

    private HorizontalAlignment ConvertHorizontalAlign(ParagraphAlignment align)
    {
        return align switch
        {
            ParagraphAlignment.LEFT => HorizontalAlignment.Left,
            ParagraphAlignment.RIGHT => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Center
        };
    }
}