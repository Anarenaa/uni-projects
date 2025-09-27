using System.IO;
using System.Reflection;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;

namespace Lab3.Models.ReadersWriters
{
    public static class PdfReportGenerator
    {
        public static void GenerateReport<T>(List<T> data, string title, string filePath)
        {
            var document = new Document();
            var section = document.AddSection();

            var style = document.Styles["Normal"];
            style.Font.Name = "Arial Unicode MS";

            PdfReportGenerator.GenerateTable<T>(section, data, title);

            var summary = section.AddParagraph($"Всього: {data.Count}");
            summary.Format.SpaceBefore = "0.5cm";
            summary.Format.Font.Size = 10;
            summary.Format.Alignment = ParagraphAlignment.Left;

            // 🖨️ Рендер
            var pdfRenderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(filePath);
        }

        private static void GenerateTable<T>(Section section, List<T> data, string title)
        {
            var heading = section.AddParagraph(title);
            heading.Format.Font.Size = 12;
            heading.Format.Font.Bold = true;
            heading.Format.SpaceAfter = "0.5cm";

            var table = section.AddTable();
            table.Borders.Width = 0.75;

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite) // тільки ті, що мають set
                .Select(p => new
                {
                    Property = p,
                    Order = p.GetCustomAttribute<PropertyOrderAttribute>()?.Order ?? int.MaxValue
                })
                .OrderBy(p => p.Order)
                .Select(p => p.Property)
                .ToList();

            var columnWidths = new Dictionary<string, double>
            {
                { "Id", 1.5 },
                { "PhotoPath", 2 },
                { "Name", 3 },
                { "Patronymic", 4 },
                { "Surname", 3 },
                { "DateOfBirth", 3 },
                { "Info", 5 },
                { "YearsOfExpirience", 3 },

                { "DateOfLesson", 3 },
                { "Teacher", 5 },
                { "Duration", 2.5 },
            };
            var headerNames = new Dictionary<string, string>
            {
                    { "PhotoPath", "Фото" },
                    { "Name", "Ім'я" },
                    { "Patronymic", "По батькові" },
                    { "Surname", "Прізвище" },
                    { "DateOfBirth", "Дата нар." },
                    { "Info", "Інфо" },
                    { "YearsOfExpirience", "Роки досвіду" },

                    { "DateOfLesson", "Дата і час" },
                    { "Group", "Група" },
                    { "Student", "Учень" },
                    { "Teacher", "Викладач" },
                    { "Duration", "Тривалість" },
            };

            foreach (var prop in props)
            {
                var width = columnWidths.TryGetValue(prop.Name, out var w) ? w : 3;
                table.AddColumn(Unit.FromCentimeter(width));
            }

            // Заголовки
            var headerRow = table.AddRow();
            headerRow.Format.Font.Bold = true;
            for (int i = 0; i < props.Count; i++)
            {
                headerRow.Cells[i].AddParagraph(headerNames.TryGetValue(props[i].Name, out var name) ? name : props[i].Name);
            }

            // Дані
            foreach (var item in data)
            {
                var row = table.AddRow();
                for (int i = 0; i < props.Count; i++)
                {
                    var prop = props[i];
                    var value = prop.GetValue(item);

                    if (prop.Name == "DateOfBirth" && value is DateTime dt)
                    {
                        row.Cells[i].AddParagraph(dt.ToString("dd.MM.yyyy"));
                    }
                    else if (prop.Name == "PhotoPath" && value is string imagePath && File.Exists(imagePath))
                    {
                        try
                        {
                            var image = row.Cells[i].AddImage(imagePath);
                            image.LockAspectRatio = true;

                            image.Height = Unit.FromCentimeter(1);
                        }
                        catch
                        {
                            row.Cells[i].AddParagraph("[невірне зображення]");
                        }
                    }
                    else
                    {
                        row.Cells[i].AddParagraph(value?.ToString() ?? "");
                    }

                    row.TopPadding = Unit.FromCentimeter(0.1);
                    row.BottomPadding = Unit.FromCentimeter(0.1);
                }
            }
        }

        public static void GenerateGroupReport(DanceSchool danceSchool, string filePath)
        {
            var document = new Document();
            var section = document.AddSection();

            var style = document.Styles["Normal"];
            style.Font.Name = "Arial Unicode MS";

            var groups = danceSchool.GetGroups();

            foreach (var group in groups)
            {
                var heading = section.AddParagraph($"\"{group.Name}\"");
                heading.Format.Font.Size = 16;
                heading.Format.Font.Bold = true;
                heading.Format.SpaceBefore = "2cm";
                heading.Format.SpaceAfter = "1cm";

                var teacherLabel = section.AddParagraph($"Викладач: {group.Teacher}");
                teacherLabel.Format.Font.Size = 12;
                teacherLabel.Format.Font.Bold = true;
                teacherLabel.Format.SpaceAfter = "0.5cm";

                if(group.Students.Count > 0)
                    PdfReportGenerator.GenerateTable<Student>(section, group.Students.ToList(), "Учні:");
                else
                    section.AddParagraph($"Група поки учнів не має.");
            }
           
            var summary = section.AddParagraph($"Всього груп: {danceSchool.GetGroups().Count}");
            summary.Format.SpaceBefore = "2cm";
            summary.Format.Font.Size = 10;
            summary.Format.Alignment = ParagraphAlignment.Left;

            // 🖨️ Рендер
            var pdfRenderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            pdfRenderer.RenderDocument();
            pdfRenderer.PdfDocument.Save(filePath);
        }
    }
 
}
