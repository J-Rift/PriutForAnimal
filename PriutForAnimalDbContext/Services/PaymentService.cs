using PriutForAnimal.Entities;
using PriutForAnimalDbContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimalDbContext.Services
{
    public class PaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Pay>> ProcessPayment(int employeeId, int serviceId, decimal amount)
        {
            if (amount <= 0)
                return Result.Fail<Pay>("Сумма должна быть положительной");

            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return Result.Fail<Pay>("Сотрудник не найден");

            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
                return Result.Fail<Pay>("Сервис не найден");

            if (amount < service.Price)
                return Result.Fail<Pay>("Сумма меньше стоимости услуги");

            var payment = new Pay
            {
                EmployeeId = employeeId,
                ServiceId = serviceId,
                Amount = amount,
                Date = DateTime.UtcNow
            };

            try
            {
                _context.Pays.Add(payment);
                await _context.SaveChangesAsync();
                return Result.Ok(payment);
            }
            catch (Exception ex)
            {
                return Result.Fail<Pay>($"Платеж не состоялся: {ex.Message}");
            }
        }
    }
}
