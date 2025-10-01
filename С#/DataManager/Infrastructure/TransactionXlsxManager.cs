using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using Core;

namespace Infrastructure
{
    public class TransactionXlsxManager : ITransactionManager
    {
        public List<Transaction> Read(string path)
        {
            var transactions = new List<Transaction>();
            using (var workbook = new XLWorkbook(path))
            {
                var worksheet = workbook.Worksheets.First();
                var rows = worksheet.RowsUsed().Skip(1); // Skip header row

                foreach (var row in rows)
                {
                    if (row.IsEmpty()) continue; // Skip empty rows

                    var transaction = new Transaction
                    {
                        TransactionID = row.Cell(1).GetValue<string>(),
                        TransactionAmount = row.Cell(3).GetValue<decimal>(),
                        TransactionDate = row.Cell(4).GetValue<DateTime>(),
                        TransactionType = row.Cell(5).GetValue<string>(),
                        Location = row.Cell(6).GetValue<string>(),
                        MerchantID = row.Cell(9).GetValue<string>(),
                        Channel = row.Cell(10).GetValue<string>(),
                        TransactionDuration = row.Cell(13).GetValue<int>(),
                        PreviousTransactionDate = row.Cell(16).GetValue<DateTime>(),

                        Account = new Account
                        {
                            AccountId = row.Cell(2).GetValue<string>(),
                            LoginAttempts = row.Cell(14).GetValue<int>(),
                            AccountBalance = row.Cell(15).GetValue<decimal>()
                        },

                        Device = new Device
                        {
                            DeviceId = row.Cell(7).GetValue<string>(),
                            IPAddress = row.Cell(8).GetValue<string>()
                        },

                        Customer = new Customer
                        {
                            Age = row.Cell(11).GetValue<int>(),
                            Occupation = row.Cell(12).GetValue<string>()
                        }
                    };
                    transactions.Add(transaction);
                }
            }
            return transactions;
        }

        public void Write(string path, List<Transaction> transactions)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Transactions");

                // Create Header
                worksheet.Cell(1, 1).Value = "TransactionID";
                worksheet.Cell(1, 2).Value = "AccountID";
                worksheet.Cell(1, 3).Value = "TransactionAmount";
                worksheet.Cell(1, 4).Value = "TransactionDate";
                worksheet.Cell(1, 5).Value = "TransactionType";
                worksheet.Cell(1, 6).Value = "Location";
                worksheet.Cell(1, 7).Value = "DeviceID";
                worksheet.Cell(1, 8).Value = "IPAddress";
                worksheet.Cell(1, 9).Value = "MerchantID";
                worksheet.Cell(1, 10).Value = "Channel";
                worksheet.Cell(1, 11).Value = "CustomerAge";
                worksheet.Cell(1, 12).Value = "CustomerOccupation";
                worksheet.Cell(1, 13).Value = "TransactionDuration";
                worksheet.Cell(1, 14).Value = "LoginAttempts";
                worksheet.Cell(1, 15).Value = "AccountBalance";
                worksheet.Cell(1, 16).Value = "PreviousTransactionDate";

                
                for (int i = 0; i < transactions.Count; i++)
                {
                    var transaction = transactions[i];
                    var row = i + 2;

                    worksheet.Cell(row, 1).Value = transaction.TransactionID;
                    worksheet.Cell(row, 3).Value = transaction.TransactionAmount;
                    worksheet.Cell(row, 4).Value = transaction.TransactionDate;
                    worksheet.Cell(row, 5).Value = transaction.TransactionType;
                    worksheet.Cell(row, 6).Value = transaction.Location;
                    worksheet.Cell(row, 9).Value = transaction.MerchantID;
                    worksheet.Cell(row, 10).Value = transaction.Channel;
                    worksheet.Cell(row, 13).Value = transaction.TransactionDuration;
                    worksheet.Cell(row, 16).Value = transaction.PreviousTransactionDate;

                    if (transaction.Account != null)
                    {
                        worksheet.Cell(row, 2).Value = transaction.Account.AccountId;
                        worksheet.Cell(row, 14).Value = transaction.Account.LoginAttempts;
                        worksheet.Cell(row, 15).Value = transaction.Account.AccountBalance;
                    }

                    if (transaction.Device != null)
                    {
                        worksheet.Cell(row, 7).Value = transaction.Device.DeviceId;
                        worksheet.Cell(row, 8).Value = transaction.Device.IPAddress;
                    }

                    if (transaction.Customer != null)
                    {
                        worksheet.Cell(row, 11).Value = transaction.Customer.Age;
                        worksheet.Cell(row, 12).Value = transaction.Customer.Occupation;
                    }
                }

                workbook.SaveAs(path);
            }
        }
    }
}
