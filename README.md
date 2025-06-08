# PriutForAnimal

🐾 Приют для животных - Система управления
📌 О проекте
Система автоматизации работы приюта для животных с функциями учета:

Животных (кошки/собаки)
Посетителей и посещений
Сотрудников и администраторов
Услуг и финансовых операций
Генерации отчетов в PDF


🚀 Основные возможности:

🏷️ Управление данными
CRUD операции для всех сущностей
Валидация вводимых данных
Гибкая система ролей (Админ/Сотрудник)
Поиск и фильтрация записей
📊 Отчетность
Генерация PDF отчетов с аналитикой
Статистика по посещениям
Финансовые отчеты
🔐 Безопасность
Авторизация и аутентификация
Хеширование паролей
Разграничение прав доступа



🛠 Технологический стек
Язык: C#
Платформа: .NET 6
UI: WPF (MVVM)
База данных: SQL Server
Библиотеки:
Entity Framework Core
QuestPDF (генерация отчетов)
Material Design (UI компоненты)


📁 Структура проекта:
text
/PriutForAnimal
├── /PriutForAnimalLib
│   ├── /Entities        # Модели данных
│   ├── /Services        # Бизнес-логика
│   └── /Reports         # Генерация отчетов
├── /PriutForAnimalApp
│   ├── /Views           # WPF страницы
│   ├── /Assets          # Ресурсы
│   └── App.xaml         # Конфигурация
└── README.md            # Документация
// Это не полная комплекция проекта! 


🐶 Основные сущности:
Животные (Animal)

public class Animal 
{
    public Guid AnimalId { get; set; }
    public Type Type { get; set; } // Cat/Dog
    public string Name { get; set; }
    public int Age { get; set; }
    public HealthStatus HealthStatus { get; set; } // Healthy/Sick
    public int VisitCount { get; set; }
    public List<Visitor> Visitors { get; set; }
}

Посетители (Visitor)

public class Visitor 
{
    public int VisitorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string ResidentialAddress { get; set; }
    public Animal Animal { get; set; }
}

Сотрудники (Employee)

public class Employee
{
    public Guid EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // Хешированный
    public Role Role { get; set; }
    public List<Service> Services { get; set; }
}

Платежи (Pay)

public class Pay 
{
    public Guid PayId { get; set; }
    public string Name { get; set; }
    public int EmployeeId { get; set; }
    public int ServiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}


🖥️ Интерфейсы:
Окно входа
https:///Assets/Images/screenshots/login.png

Главное окно администратора
https:///Assets/Images/screenshots/admin.png

Страница управления животными
https:///Assets/Images/screenshots/animals.png


⚙️ Установка и запуск:
Требования
.NET 6.0+
SQL Server 2019+
Visual Studio 2022 (для разработки)


Инструкция по развертыванию
Клонировать репозиторий:

bash
git clone https://github.com/your-repo/PriutForAnimal.git
cd PriutForAnimal
Настроить подключение к БД в AppDbContext.cs:

optionsBuilder.UseSqlServer("Server=your-server;Database=PriutForAnimal;Trusted_Connection=True;");
Применить миграции:

bash
dotnet ef database update
Запустить приложение:

bash
dotnet run
📄 Генерация отчетов
Система использует QuestPDF для создания PDF отчетов:


var report = new PdfReportGenerator();
report.GenerateEmployeeReport(employee, animals, visitors, "report.pdf");


Пример отчета включает:
Информацию о сотруднике
Список животных с их статусом
Данные о посетителях
Финансовую сводку

📜 Лицензия:
Проект распространяется под лицензией MIT. Подробнее см. в файле LICENSE.
📧 Контакты
По вопросам сотрудничества и поддержки:
Email: sergo98pro@gmail.com
Телефон: +7 (903) 434 76 80


Приют для животных © 2025
Документ создан при поддержке курсового проекта на тему: "Разработка программного модуля автоматизации работы в приюте для животных"
