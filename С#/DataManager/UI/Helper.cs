using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Infrastructure;

namespace UI
{
    public static class Helper
    {
        public static string ChooseFile()
        {
            string filePath = null;

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Виберіть файл для завантаження",
                Filter = "Текстові файли|*.txt|CSV файли (*.csv)|*.csv|XML файли (*.xml)|*.xml|JSON Files (*.json)|*.json|Excel файли(*.xlsx)|*.xlsx"
            };
            bool? res = openFileDialog.ShowDialog();

            if (res == true)
            {
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }
        public static ITransactionManager GetManager(string ext)
        {
            switch (ext)
            {
                case "csv":
                    return new TransactionCsvManager();
                case "json":
                    return new TransactionJsonManager();
                case "xml":
                    return new TransactionXmlManager();
                case "xlsx":
                    return new TransactionXlsxManager();
                default:
                    throw new NotSupportedException($"File extension '{ext}' is not supported.");
            }
        }
    }
}
