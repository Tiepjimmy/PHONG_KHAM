using OfficeOpenXml;
using OfficeOpenXml.Style;
using ClinicManagement.Model.Model;
using System.Drawing;

namespace ClinicManagement.Model.Helper
{
    public static class ExportToExcelHelper
    {
        public static byte[] DrawTemplate(ExcelPackage excelPackage, ExcelWorksheet worksheet, List<ExportToExcelModel> listExportToExcelModel, int row)
        {
            var first = listExportToExcelModel.First().ColunmName;
            var last = listExportToExcelModel.Last().ColunmName;

            //Draw header
            foreach (var item in listExportToExcelModel)
            {
                worksheet.Cells[string.Concat(item.ColunmName, ConstantHelper.One)].Value = item.ColunmTitle;
            };

            //Set Bold
            worksheet.Cells[string.Concat(first, ConstantHelper.One, ConstantHelper.Colon, last, ConstantHelper.One)].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[string.Concat(first, ConstantHelper.One, ConstantHelper.Colon, last, ConstantHelper.One)].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(ConstantHelper.ExcelStyle.BackgroundColor));
            worksheet.Cells[string.Concat(first, ConstantHelper.One, ConstantHelper.Colon, last, ConstantHelper.One)].Style.Font.Color.SetColor(ColorTranslator.FromHtml(ConstantHelper.ExcelStyle.FontColor));

            //Set Format
            worksheet.Cells.Style.Numberformat.Format = "@";

            //Set font family
            worksheet.Cells[string.Concat(first, ConstantHelper.One, ConstantHelper.Colon, last, row)].Style.Font.Name = ConstantHelper.ExcelStyle.FontName;

            //Set AutoFitColumns
            worksheet.Cells.AutoFitColumns();

            using MemoryStream stream = new();
            excelPackage.SaveAs(stream);
            byte[] result = stream.ToArray();
            return result;
        }
    }
}
