using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Data;

namespace ClinicManagement.Model.Helper
{
    public class TemplateExcelHelper
    {
        public static void FillReport(string filename, string templatefilename, DataSet data)
        {
            FillReport(filename, templatefilename, data, ["%", "%"]);
        }

        public static byte[] FillReport(string filename, string templatefilename, DataSet data, string[] deliminator)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using FileStream file = new(filename, FileMode.CreateNew);
            using FileStream temp = new(templatefilename, FileMode.Open);
            using ExcelPackage xls = new(file, temp);
            foreach (ExcelNamedRange n in xls.Workbook.Names)
            {
                FillWorksheetData(data, n.Worksheet, n, deliminator);
            }
            foreach (ExcelWorksheet ws in xls.Workbook.Worksheets)
            {
                foreach (ExcelNamedRange n in ws.Names)
                {
                    FillWorksheetData(data, ws, n, deliminator);
                }
            }
            foreach (ExcelWorksheet ws in xls.Workbook.Worksheets)
            {
                foreach (ExcelRangeBase c in ws.Cells)
                {
                    var s = string.Concat(string.Empty, c.Value);
                    if (s.StartsWith(deliminator[0]) == false && s.EndsWith(deliminator[1]) == false)
                    {
                        continue;
                    }
                    s = s.Replace(deliminator[0], string.Empty).Replace(deliminator[1], string.Empty);
                    string[] ss = s.Split(ConstantHelper.Dot);
                    try
                    {
                        c.Value = data.Tables[ss[0]]?.Rows[0][ss[1]];
                    }
                    catch { }
                }
            }
            return xls.GetAsByteArray();
        }

        private static void FillWorksheetData(DataSet data, ExcelWorksheet ws, ExcelNamedRange n, string[] deliminator)
        {
            if (data.Tables.Contains(n.Name) == false)
            {
                return;
            }
            DataTable? dt = data.Tables[n.Name];
            var row = n.Start.Row;
            string[] cn = new string[n.Columns];
            int[] st = new int[n.Columns];
            for (int i = 0; i < n.Columns; i++)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                cn[i] = (n.Value as object[,])[0, i].ToString().Replace(deliminator[0], string.Empty).Replace(deliminator[1], string.Empty);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                if (cn[i].Contains(ConstantHelper.Dot))
                {
                    cn[i] = cn[i].Split(ConstantHelper.Dot)[1];
                }
                st[i] = ws.Cells[row, n.Start.Column + i].StyleID;
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            foreach (DataRow r in dt.Rows)
            {
                for (int col = 0; col < n.Columns; col++)
                {
                    if (dt.Columns.Contains(cn[col]))
                    {
                        ws.Cells[row, n.Start.Column + col].Value = r[cn[col]];
                    }
                    ws.Cells[row, n.Start.Column + col].StyleID = st[col];
                }
                row++;
            }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // extend table formatting range to all rows
            foreach (ExcelTable t in ws.Tables)
            {
                ExcelAddressBase a = t.Address;
                if (n.Start.Row.Between(a.Start.Row, a.End.Row) &&
                    n.Start.Column.Between(a.Start.Column, a.End.Column))
                {
                    ExtendRows(t, dt.Rows.Count - 1);
                }
            }
        }

        public static void ExtendRows(ExcelTable excelTable, int count)
        {
            _ = new ExcelAddress(excelTable.Address.Start.Row,
                                      excelTable.Address.Start.Column,
                                      excelTable.Address.End.Row + count,
                                      excelTable.Address.End.Column);
            //Address = ad;
        }
    }

    public static class IntBetween
    {
        public static bool Between(this int v, int a, int b)
        {
            return v >= a && v <= b;
        }
    }
}
