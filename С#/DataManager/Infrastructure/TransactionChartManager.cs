using ScottPlot;
using ScottPlot.WPF;
using System.Collections.Generic;
using System.Linq;
using Core;
using System;

namespace Infrastructure
{
    public class TransactionChartManager
    {
        public void PlotTransactionsOverTime(WpfPlot plot, List<Transaction> transactions)
        {
            var monthlyData = transactions
                .GroupBy(t => new DateTime(t.TransactionDate.Year, t.TransactionDate.Month, 1))
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    MonthStart = g.Key.ToOADate(), // X: Числове представлення 1-го числа місяця
                    Count = (double)g.Count()      // Y: Кількість транзакцій
                })
                .ToList();

            double[] xs = monthlyData.Select(d => d.MonthStart).ToArray();
            double[] ys = monthlyData.Select(d => d.Count).ToArray();

            var barPlot = plot.Plot.Add.Bars(xs, ys);
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = 30.0 * 0.8;
            }

            plot.Plot.Axes.DateTimeTicksBottom();

            plot.Plot.Axes.Left.Label.Text = "Count";
            plot.Plot.Title("Transactions Over Time");
        }
        public void PlotTransactionsAmount(WpfPlot plot, List<Transaction> transactions)
        {
            var values = transactions.OrderBy(t => t.TransactionAmount)
                .Select(t => (double)t.TransactionAmount).ToArray();

            var hist = ScottPlot.Statistics.Histogram.WithBinSize(20, values);

            var barPlot = plot.Plot.Add.Bars(hist.Bins, hist.Counts);
            foreach (var bar in barPlot.Bars)
            {
                bar.Size = hist.FirstBinSize * .9;
            }
            plot.Plot.Axes.Bottom.Label.Text = "Amount";
            plot.Plot.Axes.Left.Label.Text = "Count";
            plot.Plot.Title("Count of Transaction Amounts");
        }

        public void PlotTransactionTypes(WpfPlot plot, List<Transaction> transactions)
        {
            var typeGroups = transactions
                .GroupBy(t => t.TransactionType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var values = typeGroups.Select(x => (double)x.Count).ToArray();
            var labels = typeGroups.Select(x => x.Type).ToArray();

            var pie = plot.Plot.Add.Pie(values);

            for (int i = 0; i < pie.Slices.Count; i++)
            {
                double percent = values[i] / values.Sum() * 100;
                pie.Slices[i].Label = $"{labels[i]} ({percent:F0}%)";
            }

            pie.ExplodeFraction = .1;
            pie.SliceLabelDistance = 1.4;

            // hide unnecessary plot components
            plot.Plot.Axes.Frameless();
            plot.Plot.HideGrid();

            plot.Plot.Title("Transaction Distribution by Type");
        }
        public void PlotTransactionChannel(WpfPlot plot, List<Transaction> transactions)
        {
            var channelGroups = transactions
                .GroupBy(t => t.Channel)
                .Select(g => new { Channel = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var values = channelGroups.Select(x => (double)x.Count).ToArray();
            var labels = channelGroups.Select(x => x.Channel).ToArray();

            var pie = plot.Plot.Add.Pie(values);

            for (int i = 0; i < pie.Slices.Count; i++)
            {
                double percent = values[i] / values.Sum() * 100;
                pie.Slices[i].Label = $"{labels[i]} ({percent:F0}%)";
            }

            pie.SliceLabelDistance = 1.4;

            // hide unnecessary plot components
            plot.Plot.Axes.Frameless();
            plot.Plot.HideGrid();

            plot.Plot.Title("Transaction Distribution by Channel");
        }
    }
}