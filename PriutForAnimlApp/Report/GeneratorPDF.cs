using PriutForAnimal.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PriutForAnimal.Reports
{
    public class PdfReportGenerator
    {
        public void GenerateEmployeeReport(
            Employee employee,
            IReadOnlyCollection<Animal> animals,
            IReadOnlyCollection<Visitor> visitors,
            IReadOnlyCollection<Service> services,
            IReadOnlyCollection<Pay> payments,
            string filePath)
        {
            ValidateInputParameters(employee, animals, visitors, services, payments, filePath);

            try
            {
                QuestPDF.Settings.License = LicenseType.Community;

                Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        ConfigurePageLayout(page);
                        AddHeader(page);
                        AddContent(page, employee, animals, visitors, services, payments);
                    });
                })
                .GeneratePdf(filePath);
            }
            catch (Exception ex)
            {
                throw new ReportGenerationException("Ошибка при генерации PDF отчета", ex);
            }
        }

        private void ValidateInputParameters(
            Employee employee,
            IReadOnlyCollection<Animal> animals,
            IReadOnlyCollection<Visitor> visitors,
            IReadOnlyCollection<Service> services,
            IReadOnlyCollection<Pay> payments,
            string filePath)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (animals == null)
                throw new ArgumentNullException(nameof(animals));

            if (visitors == null)
                throw new ArgumentNullException(nameof(visitors));

            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (payments == null)
                throw new ArgumentNullException(nameof(payments));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Не указан путь для сохранения файла", nameof(filePath));
        }

        private void ConfigurePageLayout(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.MarginVertical(50);
            page.MarginHorizontal(25);
            page.DefaultTextStyle(TextStyle.Default.FontSize(12));
        }

        private void AddHeader(PageDescriptor page)
        {
            page.Header()  // Используем .Header() вместо .Container()
                .PaddingBottom(20)
                .AlignCenter()
                .Text("Отчет приюта для животных")
                .FontSize(18)
                .Bold();
        }

        private void AddContent(
            PageDescriptor page,
            Employee employee,
            IReadOnlyCollection<Animal> animals,
            IReadOnlyCollection<Visitor> visitors,
            IReadOnlyCollection<Service> services,
            IReadOnlyCollection<Pay> payments)
        {
            page.Content()
                .PaddingVertical(10)
                .Column(column =>
                {
                    AddEmployeeInfo(column, employee);
                    AddAnimalsSection(column, animals);
                    AddVisitorsSection(column, visitors);
                    AddFinancialSection(column, services, payments);
                    AddSummarySection(column, animals, visitors, services, payments);
                    AddGenerationDate(column);
                });
        }

        private void AddEmployeeInfo(ColumnDescriptor column, Employee employee)
        {
            column.Item()
                .Text($"Сотрудник: {employee.LastName} {employee.FirstName} {employee.MiddleName}")
                .FontSize(12);

            column.Item()
                .Text($"Телефон: {employee.PhoneNumber}");

            column.Item()
                .Text($"Email: {employee.Email}");

            // Отступ добавляется к контейнеру, а не к тексту
            column.Item()
                .PaddingBottom(15)  // <-- Вот так правильно!
                .Text($"Должность: {employee.Role}");
        }

        private void AddAnimalsSection(ColumnDescriptor column, IReadOnlyCollection<Animal> animals)
        {
            // Заголовок с отступом снизу
            column.Item()
                .PaddingBottom(10)  // <-- Отступ применяется к контейнеру (Item), а не к тексту
                .Text("Животные в приюте:")
                .FontSize(14).Bold();

            // Таблица с отступом снизу
            column.Item()
                .PaddingBottom(15)  // <-- Отступ применяется к контейнеру перед таблицей
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2); // Тип
                        columns.RelativeColumn(3); // Имя
                        columns.RelativeColumn(2); // Возраст
                        columns.RelativeColumn(3); // Состояние здоровья
                        columns.RelativeColumn(2); // Посещений
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Тип").Bold();
                        header.Cell().Text("Имя").Bold();
                        header.Cell().Text("Возраст").Bold();
                        header.Cell().Text("Состояние здоровья").Bold();
                        header.Cell().Text("Посещений").Bold();
                    });

                    foreach (var animal in animals)
                    {
                        table.Cell().Text(animal.Type.ToString());
                        table.Cell().Text(animal.Name);
                        table.Cell().Text(animal.Age.ToString());
                        table.Cell().Text(animal.HealthStatus.ToString());
                        table.Cell().Text(animal.VisitCount.ToString());
                    }
                });
        }

        private void AddVisitorsSection(ColumnDescriptor column, IReadOnlyCollection<Visitor> visitors)
        {
            // Заголовок с отступом
            column.Item()
                .PaddingBottom(10)  // Переносим PaddingBottom перед Text
                .Text("Посетители:")
                .FontSize(14).Bold();

            // Таблица с отступом
            column.Item()
                .PaddingBottom(15)  // Переносим PaddingBottom перед Table
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // ФИО
                        columns.RelativeColumn(3); // Телефон
                        columns.RelativeColumn(3); // Адрес
                        columns.RelativeColumn(4); // Посещенные животные
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("ФИО").Bold();
                        header.Cell().Text("Телефон").Bold();
                        header.Cell().Text("Адрес").Bold();
                        header.Cell().Text("Посещенные животные").Bold();
                    });

                    foreach (var visitor in visitors)
                    {
                        table.Cell().Text($"{visitor.LastName} {visitor.FirstName} {visitor.MiddleName}");
                        table.Cell().Text(visitor.PhoneNumber);
                        table.Cell().Text(visitor.ResidentialAddress);

                        string visitedAnimals = string.Join(", ",
                            visitor.VisitedAnimals.Select(a => $"{a.Name} ({a.Type})"));
                        table.Cell().Text(visitedAnimals);
                    }
                });
        }

        private void AddFinancialSection(ColumnDescriptor column,IReadOnlyCollection<Service> services,IReadOnlyCollection<Pay> payments)
        {
            // Заголовок с отступом
            column.Item()
                .PaddingBottom(10)  // Отступ применяется к контейнеру
                .Text("Финансовая информация:")
                .FontSize(14).Bold();

            // Таблица с отступом
            column.Item()
                .PaddingBottom(15)  // Отступ применяется к контейнеру таблицы
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3); // Услуга
                        columns.RelativeColumn(3); // Описание
                        columns.RelativeColumn(2); // Сумма
                        columns.RelativeColumn(2); // Дата
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Услуга").Bold();
                        header.Cell().Text("Описание").Bold();
                        header.Cell().Text("Сумма").Bold();
                        header.Cell().Text("Дата").Bold();
                    });

                    foreach (var pay in payments)
                    {
                        var service = services.FirstOrDefault(s => s.Id == pay.ServiceId);
                        if (service != null)
                        {
                            table.Cell().Text(service.ServiceName);
                            table.Cell().Text(service.Description);
                            table.Cell().Text(pay.Amount.ToString("C"));
                            table.Cell().Text(pay.Date.ToString("dd.MM.yyyy"));
                        }
                    }
                });
        }

        private void AddSummarySection(ColumnDescriptor column,IReadOnlyCollection<Animal> animals,IReadOnlyCollection<Visitor> visitors,IReadOnlyCollection<Service> services,IReadOnlyCollection<Pay> payments)
        {
            // Заголовок с отступом
            column.Item()
                .PaddingBottom(10)  // Отступ применяется к контейнеру
                .Text("Итоговая информация:")
                .FontSize(14).Bold();

            decimal totalAmount = payments.Sum(p => p.Amount);
            int totalAnimals = animals.Count;
            int totalVisitors = visitors.Count;
            int totalServices = services.Count;

            // Добавляем информацию с отступами между строками
            column.Item().Text($"Общее количество животных: {totalAnimals}");
            column.Item().Text($"Общее количество посетителей: {totalVisitors}");
            column.Item().Text($"Общее количество услуг: {totalServices}");

            // Последний элемент с дополнительным отступом
            column.Item()
                .PaddingBottom(15)
                .Text($"Общая сумма платежей: {totalAmount.ToString("C")}");
        }

        private void AddGenerationDate(ColumnDescriptor column)
        {
            column.Item()
                .AlignRight()
                .Text($"Отчет сгенерирован: {DateTime.Now:dd.MM.yyyy HH:mm}")
                .FontColor(Colors.Grey.Medium);
        }
    }

    public class ReportGenerationException : Exception
    {
        public ReportGenerationException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}