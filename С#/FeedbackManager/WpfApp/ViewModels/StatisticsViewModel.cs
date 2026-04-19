using System.Diagnostics;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Services;

namespace WpfApp.ViewModels
{
    public partial class StatisticsViewModel : ObservableObject
    {
        private readonly FeedbackService _feedbackService;
        private readonly CategoryService _categoryService;

        [ObservableProperty]
        private int _feedbacksTotalCount;

        [ObservableProperty] 
        private PlotModel _ratingModel;
        [ObservableProperty] 
        private PlotModel _categoryAvgModel;
        [ObservableProperty] 
        private PlotModel _categoryPieModel;
        [ObservableProperty] 
        private PlotModel _timelineModel;

        [ObservableProperty]
        private string _selectedPeriod;

        public StatisticsViewModel(FeedbackService feedbackService, CategoryService categoryService)
        {
            _feedbackService = feedbackService;
            _categoryService = categoryService;

            SelectedPeriod = "7";
            RefreshAllStatistics();
        }
        partial void OnSelectedPeriodChanged(string value)
        {
            RefreshAllStatistics();
        }
        public void RefreshAllStatistics()
        {
            // 1. Визначаємо початкову дату на основі обраного періоду
            int days = int.TryParse(SelectedPeriod, out var d) ? d : 7;
            DateTime startDate = days > 0 ? DateTime.Now.Date.AddDays(-days) : DateTime.MinValue;

            // 2. Отримуємо дані та фільтруємо їх за часом
            var allAnalyzed = _feedbackService.GetFilteredFeedbacks(isAnalyzed: true);
            if (allAnalyzed == null) return;

            var analyzed = allAnalyzed
                .Where(f => f.SubmittedAt >= startDate)
                .ToList();

            if (!analyzed.Any())
            {
                // Очищуємо графіки, якщо даних немає
                RatingModel = CategoryAvgModel = CategoryPieModel = TimelineModel = null;
                return;
            }

            // Отримуємо аналітику категорій
            var catStats = _categoryService.GetCategoryAnalytics();
            FeedbacksTotalCount = analyzed.Count;

            // --- 1. Розподіл оцінок ---
            var ratingPlot = new PlotModel { Title = $"Розподіл оцінок (за {days} дн.)", Background = OxyColors.White };
            var ratingSeries = new LinearBarSeries { FillColor = OxyColors.CornflowerBlue };
            var ratings = new int[11];
            foreach (var f in analyzed) if (f.Rate >= 0 && f.Rate <= 10) ratings[f.Rate]++;
            for (int i = 0; i <= 10; i++) ratingSeries.Points.Add(new DataPoint(i, ratings[i]));
            ratingPlot.Series.Add(ratingSeries);
            ratingPlot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -0.5, Maximum = 10.5, MajorStep = 1 });
            RatingModel = ratingPlot;

            // --- 2. Середній бал в категорії ---
            var avgPlot = new PlotModel { Title = "Середній бал категорій", Background = OxyColors.White };
            var avgSeries = new BarSeries { FillColor = OxyColors.MediumPurple, LabelFormatString = "{0:0.0}" };
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };

            // беремо тільки ті категорії, де є відгуки за обраний період
            var activeStats = catStats.Where(s => s.TotalCount > 0)
                                      .OrderBy(s => s.AverageRating) // сортування від гірших до кращих
                                      .ToList();

            if (activeStats.Any())
            {
                foreach (var s in activeStats)
                {
                    avgSeries.Items.Add(new BarItem { Value = s.AverageRating });
                    categoryAxis.Labels.Add(s.Name);
                }
                avgPlot.Series.Add(avgSeries);
                avgPlot.Axes.Add(categoryAxis);
                avgPlot.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = 0, Maximum = 10, Title = "Рейтинг" });
                CategoryAvgModel = avgPlot;
            }
            else
            {
                CategoryAvgModel = null;
            }

            // --- 3. Пиріг категорій ---
            var piePlot = new PlotModel { Title = "Частка відгуків", Background = OxyColors.White };
            var pieSeries = new PieSeries { InnerDiameter = 0.4 };
            foreach (var s in catStats.Where(x => x.TotalCount > 0))
            {
                pieSeries.Slices.Add(new PieSlice(s.Name, s.TotalCount));
            }
            piePlot.Series.Add(pieSeries);
            CategoryPieModel = piePlot;

            // --- 4. Динаміка (адаптується під кількість днів) ---
            var timelinePlot = new PlotModel { Title = "Активність за період", Background = OxyColors.White };
            var lineSeries = new LineSeries { Color = OxyColors.Orange, MarkerType = MarkerType.Circle };

            DateTime actualStart = days > 0 ? startDate : analyzed.Min(f => f.SubmittedAt).Date;
            int totalDays = (DateTime.Now.Date - actualStart).Days;

            // Крок сітки: якщо днів багато (наприклад, рік), показуємо по місяцях, якщо мало — по днях
            for (int i = 0; i <= totalDays; i++)
            {
                var day = actualStart.AddDays(i);
                double count = analyzed.Count(f => f.SubmittedAt.Date == day);
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(day), count));
            }
            timelinePlot.Series.Add(lineSeries);
            timelinePlot.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = totalDays > 30 ? "MM.yyyy" : "dd.MM" });
            TimelineModel = timelinePlot;
        }

        //Генерація PDF звіту з використанням QuestPDF
        private byte[] exportPlotToByte(PlotModel model)
        {
            if (model == null) return Array.Empty<byte>();

            using (var stream = new MemoryStream())
            {
                // SkiaSharpPngExporter перетворює PlotModel на набір байтів картинки PNG [cite: 2026-02-04]
                var exporter = new OxyPlot.SkiaSharp.PngExporter { Width = 800, Height = 600 };
                exporter.Export(model, stream);
                return stream.ToArray();
            }
        }
        [RelayCommand]
        private void GenerateTimedReport()
        {
            // 1. Налаштування ліцензії (обов'язково для QuestPDF)
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = $"Звіт_відгуки_{DateTime.Now:yyyyMMdd}",
                DefaultExt = ".pdf"
            };

            if (saveFileDialog.ShowDialog() != true) return;
            string filePath = saveFileDialog.FileName;

            // 2. Визначаємо період (SelectedPeriod приходить з ComboBox Tag) [cite: 2026-01-10]
            int days = int.TryParse(SelectedPeriod, out var d) ? d : 7;
            DateTime startDate = days > 0 ? DateTime.Now.AddDays(-days) : DateTime.MinValue;

            // 3. Фільтруємо дані [cite: 2026-01-25]
            var allFeedbacks = _feedbackService.GetAllFeedbacks();
            var recentFeedbacks = allFeedbacks
                .Where(f => f.SubmittedAt >= startDate)
                .OrderByDescending(f => f.SubmittedAt)
                .ToList();

            var criticalFeedbacks = recentFeedbacks.Where(f => f.IsImportant).ToList();

            // 4. Генерація QuestPDF [cite: 2026-02-05]
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Header().Column(col =>
                    {
                        col.Item().Text("Аналітичний звіт по відгуках").FontSize(24).SemiBold().FontColor("#6200EE");
                        string periodText = days > 0 ? $"{startDate:dd.MM.yyyy} — {DateTime.Now:dd.MM.yyyy}" : "За весь час";
                        col.Item().Text($"Період: {periodText}").FontSize(12).Italic();
                    });

                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        col.Spacing(20);
                        col.Item().Text("1. Розподіл відгуків за категоріями").FontSize(16).Bold();
                        col.Item().Border(1).BorderColor("#EEE").Image(exportPlotToByte(CategoryPieModel));

                        col.Item().Text("2. Середній бал по категоріях").FontSize(16).Bold();
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();   // Назва категорії
                                columns.ConstantColumn(100); // Середній бал
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Назва категорії");
                                header.Cell().Element(CellStyle).Text("Сер. бал");

                                static IContainer CellStyle(IContainer container) =>
                                    container.DefaultTextStyle(x => x.Bold()).PaddingVertical(5).BorderBottom(1);
                            });

                            // Отримуємо аналітику та фільтруємо категорії, де 0 відгуків
                            var catStats = _categoryService.GetCategoryAnalytics()
                                                           .Where(s => s.TotalCount > 0);

                            foreach (var s in catStats)
                            {
                                table.Cell().PaddingVertical(5).Text(s.Name);

                                // Додаємо колір: Червоний < 4, Зелений > 7 
                                table.Cell().PaddingVertical(5).Text($"{s.AverageRating:F1} / 10")
                                     .FontColor(s.AverageRating < 4 ? Colors.Red.Medium
                                              : s.AverageRating > 7 ? Colors.Green.Medium
                                              : Colors.Black);
                            }
                        });

                        col.Item().Text("2. Критичні відгуки").FontSize(16).Bold().FontColor(Colors.Red.Medium);

                        if (criticalFeedbacks.Any())
                        {
                            col.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(80);
                                    columns.ConstantColumn(40);
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Дата");
                                    header.Cell().Element(CellStyle).Text("Оц.");
                                    header.Cell().Element(CellStyle).Text("Зміст відгуку");
                                    static IContainer CellStyle(IContainer container) => container.DefaultTextStyle(x => x.Bold()).PaddingVertical(5).BorderBottom(1);
                                });

                                foreach (var fb in criticalFeedbacks)
                                {
                                    table.Cell().PaddingVertical(5).Text($"{fb.SubmittedAt:dd.MM.yyyy}");
                                    table.Cell().PaddingVertical(5).Text($"{fb.Rate}/10");
                                    table.Cell().PaddingVertical(5).Text(fb.Comment);
                                }
                            });
                        }
                        else
                        {
                            col.Item().Text("Критичних відгуків не знайдено.").Italic();
                        }
                    });

                    page.Footer().AlignCenter().Text(x => x.CurrentPageNumber());
                });
            });

            document.GeneratePdf(filePath);
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }
    }
}