using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace SearchEverywhere.Utility.office;

internal class ExcelReader
{
    public async Task<ObservableCollection<ObservableCollection<string>>> ReadExcel(string path)
    {
        var targetMatrix = new ObservableCollection<ObservableCollection<string>>();
        await Task.Run(() =>
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IWorkbook xls = null;
            if (Path.GetExtension(path) == ".xlsx")
                xls = new XSSFWorkbook(fs);
            else
                xls = new HSSFWorkbook(fs);
            for (var sheetIndex = 0; sheetIndex < xls.NumberOfSheets; sheetIndex++)
            {
                var sheet = xls.GetSheetAt(sheetIndex);
                for (var rowIndex = 0; rowIndex < sheet.LastRowNum; rowIndex++)
                {
                    var eachRow = sheet.GetRow(rowIndex);
                    var strRow = new ObservableCollection<string>();
                    eachRow.Cells.ForEach(x => strRow.Add(x.ToString()));
                    targetMatrix.Add(strRow);
                }

                break;
            }
        });
        return targetMatrix;
    }
}