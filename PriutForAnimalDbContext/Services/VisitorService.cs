using PriutForAnimal.Entities;
using PriutForAnimalDbContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimalDbContext.Services
{
    public class VisitorService
    {
        private readonly AppDbContext _context;

        public VisitorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Visitor>> RegisterVisitor(Visitor visitor)
        {
            if (visitor == null)
                return Result.Fail<Visitor>("Посетитель не может быть пустым");

            if (string.IsNullOrWhiteSpace(visitor.PhoneNumber) || visitor.PhoneNumber.Length < 10)
                return Result.Fail<Visitor>("Требуется действительный номер телефона");

            if (string.IsNullOrWhiteSpace(visitor.ResidentialAddress))
                return Result.Fail<Visitor>("Требуется указать адрес");

            try
            {
                _context.Visitors.Add(visitor);
                await _context.SaveChangesAsync();
                return Result.Ok(visitor);
            }
            catch (Exception ex)
            {
                return Result.Fail<Visitor>($"Не удалось зарегистрировать посетителя: {ex.Message}");
            }
        }

        public async Task<Result> RecordAnimalVisit(int visitorId, Guid animalId)
        {
            var visitor = await _context.Visitors.FindAsync(visitorId);
            if (visitor == null)
                return Result.Fail("Посетитель не найден");

            var animal = await _context.Animals.FindAsync(animalId);
            if (animal == null)
                return Result.Fail("Животное не найдено");

            animal.VisitCount++;

            try
            {
                await _context.SaveChangesAsync();
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Не удалось записать посещение: {ex.Message}");
            }
        }
    }
}
