using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Xceed.Words.NET;

namespace Infrastructure
{
    public class TransactionsDocxReport
    {
        public void GenerateTransactionsReport(List<Transaction> transactions, string filePath)
        {
            var sortedtransactions = transactions.OrderBy(t => t.TransactionDate).ToList();

            Xceed.Words.NET.Licenser.LicenseKey = "WDN51-XUTHD-14U1Y-5AFA";

            using (var doc = DocX.Create(filePath))
            {

                // Заголовок
                doc.InsertParagraph("Звіт по транзакціях")
                    .FontSize(18)
                    .Bold()
                    .Alignment = Xceed.Document.NET.Alignment.center;
                doc.InsertParagraph($"з {sortedtransactions[0].TransactionDate.ToString("dd.MM.yyyy")} по {sortedtransactions[sortedtransactions.Count - 1].TransactionDate.ToString("dd.MM.yyyy")}")
                    .FontSize(18)
                    .Bold()
                    .Alignment = Xceed.Document.NET.Alignment.center;

                doc.InsertParagraph(Environment.NewLine);

                doc.InsertParagraph($"Кількість записів: {transactions.Count<Transaction>()}")
                    .FontSize(12);

                // Інфо про дату створення
                doc.InsertParagraph($"Дата створення: {DateTime.Now:dd.MM.yyyy}")
                    .FontSize(12);

                doc.InsertParagraph().InsertPageBreakAfterSelf();

                // Таблиця (рядків стільки, скільки транзакцій + 1 для шапки)
                var table = doc.AddTable(sortedtransactions.Count + 1, 4);
                table.Alignment = Xceed.Document.NET.Alignment.center;
                table.Design = Xceed.Document.NET.TableDesign.TableGrid;

                // Шапка
                table.Rows[0].Cells[0].Paragraphs[0].Append("Дата").Bold();
                table.Rows[0].Cells[1].Paragraphs[0].Append("ID Транзакції").Bold();
                table.Rows[0].Cells[2].Paragraphs[0].Append("Клієнт").Bold();
                table.Rows[0].Cells[3].Paragraphs[0].Append("Сума").Bold();

                // Заповнення
                for (int i = 0; i < sortedtransactions.Count; i++)
                {
                    var t = sortedtransactions[i];
                    table.Rows[i + 1].Cells[0].Paragraphs[0].Append(t.TransactionDate.ToString("dd.MM.yyyy"));
                    table.Rows[i + 1].Cells[1].Paragraphs[0].Append(t.TransactionID);
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append(t.Customer?.CustomerId);
                    table.Rows[i + 1].Cells[3].Paragraphs[0].Append($"{t.TransactionAmount:#,##0.00}");
                }

                doc.InsertTable(table);

                doc.Save(filePath);
            }
        }
    }
}
