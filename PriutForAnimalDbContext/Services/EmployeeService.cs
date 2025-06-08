using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PriutForAnimal.Entities;
using PriutForAnimalDbContext.Context;
using PriutForAnimalDbContext.Services;


namespace PriutForAnimal.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Employee>> RegisterEmployee(Employee employee, string roleName)
        {
            try
            {
                // Валидация входных данных
                if (employee == null)
                    return await Task.FromResult(Result.Fail<Employee>("Сотрудник не может быть пустым"));

                // Проверка email с регулярным выражением
                if (string.IsNullOrWhiteSpace(employee.Email))
                    return await Task.FromResult(Result.Fail<Employee>("Требуется указать адрес электронной почты"));

                if (!IsValidEmail(employee.Email))
                    return await Task.FromResult(Result.Fail<Employee>("Указан недействительный адрес электронной почты"));

                // Проверка пароля
                if (string.IsNullOrWhiteSpace(employee.Password))
                    return await Task.FromResult(Result.Fail<Employee>("Требуется указать пароль"));

                if (employee.Password.Length < 8)
                    return await Task.FromResult(Result.Fail<Employee>("Пароль должен содержать минимум 8 символов"));

                if (!HasUpperCase(employee.Password) || !HasLowerCase(employee.Password))
                    return await Task.FromResult(Result.Fail<Employee>("Пароль должен содержать буквы в верхнем и нижнем регистре"));

                if (!HasDigit(employee.Password))
                    return await Task.FromResult(Result.Fail<Employee>("Пароль должен содержать хотя бы одну цифру"));

                // Проверка уникальности email
                if (await _context.Employees.AnyAsync(e => e.Email == employee.Email))
                    return await Task.FromResult(Result.Fail<Employee>("Пользователь с таким email уже существует"));

                // Поиск роли
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
                if (role == null)
                    return await Task.FromResult(Result.Fail<Employee>("Указанная роль не найдена"));

                // Назначение роли и сохранение
                employee.Role = role;
                employee.RoleId = role.RoleId;

                // Хеширование пароля перед сохранением
                employee.Password = HashPassword(employee.Password);

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return await Task.FromResult(Result.Ok(employee));
            }
            catch (DbUpdateException dbEx)
            {
                return await Task.FromResult(Result.Fail<Employee>($"Ошибка базы данных: {dbEx.InnerException?.Message ?? dbEx.Message}"));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(Result.Fail<Employee>($"Не удалось зарегистрировать сотрудника: {ex.Message}"));
            }
        }

        // Методы валидации
        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool HasUpperCase(string password) => password.Any(char.IsUpper);
        private bool HasLowerCase(string password) => password.Any(char.IsLower);
        private bool HasDigit(string password) => password.Any(char.IsDigit);

        // Метод хеширования пароля
        private string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData
                (System.Text.Encoding.UTF8.GetBytes(password)));
        }

        public async Task<Result<Employee>> GetEmployeeById(Guid id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.Role)
                    .Include(e => e.Visitors)
                    .FirstOrDefaultAsync(e => e.EmployeeId == id);

                if (employee == null)
                {
                    return Result.Fail<Employee>("Сотрудник не найден");
                }

                return Result.Ok(employee);
            }
            catch (Exception ex)
            {
                return Result.Fail<Employee>($"Ошибка при получении сотрудника: {ex.Message}");
            }
        }

        public async Task<Result<Employee>> Authenticate(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result.Fail<Employee>("Требуется указать email");

            if (string.IsNullOrWhiteSpace(password))
                return Result.Fail<Employee>("Требуется указать пароль");

            var employee = await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Email == email && e.Password == password);

            if (employee == null)
                return Result.Fail<Employee>("Неверный email или пароль");

            return Result.Ok(employee);
        }

        public async Task<Result<Employee>> UpdateEmployee(Employee employee)
        {
            if (employee == null)
                return Result.Fail<Employee>("Сотрудник не может быть пустым");

            try
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return Result.Ok(employee);
            }
            catch (Exception ex)
            {
                return Result.Fail<Employee>($"Не удалось обновить данные сотрудника: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return Result.Fail<bool>("Сотрудник не найден");

            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return Result.Ok(true);
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>($"Не удалось удалить сотрудника: {ex.Message}");
            }
        }
    }
}