using System;
using System.IO;
using NPOI.HSSF.UserModel;

namespace SearchEverywhere.Utility.office;

internal class ExcelReader
{
    public void ReadExcel(string path)
    {
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var xls = new HSSFWorkbook(fs);
        for (var i = 0; i < xls.NumberOfSheets; i++)
        {
            var sheet = xls.GetSheetAt(i);
            for (var j = 0; j < sheet.LastRowNum; j++)
            {
                var eachRow = sheet.GetRow(j);
                Console.WriteLine(sheet.GetRow(j));
            }

            Console.WriteLine(sheet);
        }
    }
}