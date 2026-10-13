using ClosedXML.Excel;
using Serilog;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ClinicManagement.Model.Helper
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("GeneratedRegex", "SYSLIB1045:Convert to 'GeneratedRegexAttribute'.", Justification = "<Pending>")]
    public class DataTableHelper
    {
        public static bool IsContainColumn(string columnName, DataTable dataTable)
        {
            DataColumnCollection columns = dataTable.Columns;
            if (columns.Contains(columnName))
            {
                return true;
            }
            return false;
        }

        public static DataTable RemoveColumnsInDataTable(string[] listColumnRemove, DataTable dataTable)
        {
            for (int i = dataTable.Columns.Count - 1; i >= 0; i--)
            {
                DataColumn dataColumn = dataTable.Columns[i];
                if (listColumnRemove.Contains(dataColumn.ColumnName))
                {
                    dataTable.Columns.Remove(dataColumn);
                }
            }
            return dataTable;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = [];
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            var valueFromExcel = dr[column.ColumnName]?.ToString();
                            if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
                            {
                                if (string.IsNullOrEmpty(valueFromExcel))
                                {
                                    pro.SetValue(obj, null, null);
                                }
                                else
                                {
                                    pro.SetValue(obj, DateTime.Parse(valueFromExcel), null);
                                }
                            }
                            if (pro.PropertyType == typeof(long) || pro.PropertyType == typeof(long?))
                            {
                                if (string.IsNullOrEmpty(valueFromExcel))
                                {
                                    pro.SetValue(obj, null, null);
                                }
                                else
                                {
                                    pro.SetValue(obj, long.Parse(valueFromExcel), null);
                                }
                            }
                            if (pro.PropertyType == typeof(bool) || pro.PropertyType == typeof(bool?) || pro.PropertyType.Name == "Boolean")
                            {
                                if (string.IsNullOrEmpty(valueFromExcel))
                                {
                                    pro.SetValue(obj, null, null);
                                }
                                else
                                {
                                    if (valueFromExcel == ConstantHelper.One)
                                    {
                                        pro.SetValue(obj, true, null);
                                    }
                                    else
                                    {
                                        pro.SetValue(obj, false, null);
                                    }
                                }
                            }
                            if (pro.PropertyType == typeof(string))
                            {
                                if (string.IsNullOrEmpty(valueFromExcel))
                                {
                                    pro.SetValue(obj, null, null);
                                }
                                else
                                {
                                    pro.SetValue(obj, valueFromExcel, null);
                                }
                            }
                            if (pro.PropertyType == typeof(decimal) || pro.PropertyType == typeof(decimal?))
                            {
                                if (string.IsNullOrEmpty(valueFromExcel))
                                {
                                    pro.SetValue(obj, null, null);
                                }
                                else
                                {
                                    pro.SetValue(obj, decimal.Parse(valueFromExcel, NumberStyles.AllowThousands), null);
                                }
                            }
                            if (pro.PropertyType == typeof(Guid) || pro.PropertyType == typeof(Guid?))
                            {
                                if (string.IsNullOrEmpty(valueFromExcel))
                                {
                                    pro.SetValue(obj, null, null);
                                }
                                else
                                {
                                    pro.SetValue(obj, Guid.Parse(valueFromExcel), null);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Log.Error(string.Concat("Error : Name - ", pro.Name, " DataType:", pro.PropertyType.Name, " -value- ", dr[column.ColumnName]));
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return obj;
        }

        public static List<dynamic> DataTableToList(DataTable dt)
        {
            List<dynamic> listaData = [];
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                listaData.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dictionary = (IDictionary<string, object>)dyn;
                    if (row[column] is DBNull)
                    {
                        dictionary[column.ColumnName] = new object();
                    }
                    else
                    {
                        dictionary[column.ColumnName] = row[column];
                    }
                }
            }
            return listaData;
        }

        public static DataTable ListObjectToDataTable<T>(IEnumerable<T> data, string tableName)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            table.TableName = tableName;
            return table;
        }

        public static DataTable RemoveColumnFieldAndValuesOfColumn(DataTable dataTable, string tableFileNameRemove)
        {
            dataTable.Columns.Remove(tableFileNameRemove);
            return dataTable;
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            using StreamReader sr = new(strFilePath);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string[] headers = sr.ReadLine().Split(ConstantHelper.Comma);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            DataTable dt = new();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
#pragma warning restore CS8604 // Possible null reference argument.
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable ConvertExeltoDataTable(string path)
        {
            DataTable dt = new();
            using XLWorkbook workbook = new(path);
            IXLWorksheet worksheet = workbook.Worksheet(1);
            bool firstRow = true;
            //Range for reading the cells based on the last cell used.  
            var readRange = "1:1";
            foreach (IXLRow row in worksheet.RowsUsed())
            {
                //If Reading the First Row (used) then add them as column name  
                if (firstRow)
                {
                    //Checking the Last cellused for column generation in datatable  
                    readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                    foreach (IXLCell cell in row.Cells(readRange))
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                    firstRow = false;
                }
                else
                {
                    //Adding a Row in datatable  
                    dt.Rows.Add();
                    int cellIndex = 0;
                    //Updating the values of datatable  
                    foreach (IXLCell cell in row.Cells(readRange))
                    {
                        dt.Rows[^1][cellIndex] = cell.Value.ToString();
                        cellIndex++;
                    }
                }
            }
            return dt;
        }
    }
}
