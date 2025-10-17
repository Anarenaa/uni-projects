# Лабораторна робота 1

**Тема:** Робота з файлами та даними у C# (CSV/JSON/XML/XLSX, звіти DOCX/XLSX, графіки)

**Студент(ка):** *Мікулич, Анастасія*

**Варіант / набір даних:** *Варіант 2, https://www.kaggle.com/datasets/valakhorasani/bank-transaction-dataset-for-fraud-detection*

**Дата:** *17.10.2025*

**Посилання на репозиторій/архів:** *https://github.com/Anarenaa/uni-projects/tree/main/%D0%A1%23/DataManager*

## Мета та завдання

**Мета: **Розробити додаток для імпорту, обробки, експорту даних та генерації звітів з візуалізацією у C#. Навчитися працювати з різними форматами файлів та створювати звіти.**

**Завдання:**

- Імпорт даних із CSV/JSON/XML/XLSX.
- Обробка/валідація/агрегація.
- Експорт у всі формати (CSV/JSON/XML/XLSX).
- Генерація звітів **XLSX** та **DOCX**.
- Візуалізація (мінімум 2 графіки) зі стороннім компонентом.
- Архітектура у 3 збірках (Domain/Data/UI), діаграма класів.

## Опис датасету

### Transaction
- TransactionID
- TransactionAmount
- TransactionDate
- TransactionType
- Location
- MerchantID:
- Channel
- TransactionDuration
- PreviousTransactionDate
- Account
- Customer
- Device

### Account
- AccountId
- AccountBalance
- LoginAttempts

### Customer
- CustomerId
- Age
- Occupation

### Device
- DeviceId
- IPAddress

### OperationRecord
- RecordID
- TransactionID
- OperationName
- OperationDateTime

## Архітектура застосунку

```
/Core
	/Account.cs
	/Customer.cs
	/Device.cs
	/OperationRecord.cs
	/Transaction.cs
/Infrastructure
	/ITransactionManager.cs
	/TransactionChartManager.cs
	/TransactionCsvManager.cs
	/TransactionDocxManager.cs
	/TransactionJsonManager.cs
	/TransactionMap.cs 
	/TransactionXlsxManager.cs
	/TransactionXmlManager.cs
/UI
	/Windows
		/Diagram.xaml
		/EditTransactionWindow.xaml
		/OperationHistoryWindow.xaml
	/Helper.cs
	/MainWindow.xaml
```

**Залежності/NuGet**: ClosedXML, CsvHelper, Newtonsoft.Json, , Xceed.Words.NET, ScottPlot.WPF, Extended.Wpf.Toolkit.

## Функціонал програми
- Створення нового файлу
- Імпорт даних із CSV/JSON/XML/XLSX.
- Експорт даних в CSV/JSON/XML/XLSX.
- Додавання транзакцій.
- Редагування транзакцій.
- Видалення транзакцій.
- Перегляд історії операцій.
- Генерація звітів у DOCX та XLSX.
- Візуалізація даних за допомогою графіків.

## Скріншоти

![Головне вікно програми]()

## Використання AI
Під час розробки цього проєкту я використовувала ChatGPT для генерації прикладів коду, пояснення концепцій та помилок коду, отримання порад щодо архітектури програми.

## Додатки
//додати файли експорт/звіти