using System.IO;
using System.Reflection;
using OfficeOpenXml;

namespace Lab3.Models.ReadersWriters
{
    public class ExcelReaderWriter<T> : IReaderWriter<T> where T : new()
    {
        public List<T> Read(string path)
        {
            var list = new List<T>();
            ExcelPackage.License.SetNonCommercialOrganization("NaU_OA");

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets[0];

                var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance) // отримуємо всі публічні та нестатичні властивості класу
                    .Select((p, index) => new
                    {
                        Property = p,
                        Order = p.GetCustomAttribute<PropertyOrderAttribute>()?.Order ?? int.MaxValue
                    })
                    .OrderBy(p => p.Order)
                    .Select(p => p.Property)
                    .ToArray();

                var start = worksheet.Dimension.Start;
                var end = worksheet.Dimension.End;

                var headerMap = new Dictionary<string, int>();
                for (int col = 1; col <= end.Column; col++)
                {
                    string header = worksheet.Cells[1, col].Text.Trim();

                    if (col <= props.Length)
                    {
                        string expectedHeader = props[col - 1].Name;

                        if (!string.Equals(header, expectedHeader, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new FormatException($"Header mismatch at column {col}: expected '{expectedHeader}', but found '{header}'");
                        }
                    }

                    headerMap[header] = col;
                }

                for (int row = 2; row <= end.Row; row++)
                {
                    T obj = new T();

                    foreach (var prop in props)
                    {
                        if (headerMap.TryGetValue(prop.Name, out int colIndex))
                        {
                            string cellValue = worksheet.Cells[row, colIndex].Text;

                            object? convertedValue = Convert.ChangeType(cellValue, prop.PropertyType);
                            prop.SetValue(obj, convertedValue);
                        }
                    }

                    list.Add(obj);
                }
            }

            return list;
        }

        public void Write(List<T> items, string path)
        {
            ExcelPackage.License.SetNonCommercialOrganization("NaU_OA");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");
                var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance) // отримуємо всі публічні та нестатичні властивості класу
                    .Select((p, index) => new
                    {
                        Property = p,
                        Order = p.GetCustomAttribute<PropertyOrderAttribute>()?.Order ?? int.MaxValue
                    })
                    .OrderBy(p => p.Order)
                    .Select(p => p.Property)
                    .ToArray();

                for (int col = 0; col < props.Length; col++)
                {
                    worksheet.Cells[1, col + 1].Value = props[col].Name;
                }

                for (int row = 0; row < items.Count; row++)
                {
                    for (int col = 0; col < props.Length; col++)
                    {
                        var value = props[col].GetValue(items[row]);

                        if (value is DateTime dateValue)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dateValue;
                            worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "dd.MM.yyyy";
                        }
                        else
                        {
                            worksheet.Cells[row + 2, col + 1].Value = value ?? string.Empty;
                        }
                    }
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                package.SaveAs(new FileInfo(path));
            }
        }

    }
}