using Microsoft.EntityFrameworkCore;
using PriutForAnimal.Entities;
using PriutForAnimalDbContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimalDbContext.Services
{
    public class AnimalService
    {
        private readonly AppDbContext _context;

        public AnimalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Animal>> AddAnimal(Animal animal)
        {
            if (animal == null)
                return Result.Fail<Animal>("Животное не может быть пустым");

            if (string.IsNullOrWhiteSpace(animal.Name))
                return Result.Fail<Animal>("Требуется указать имя животного");

            if (animal.Age <= 0)
                return Result.Fail<Animal>("Возраст должен быть больше нуля");

            if (!Enum.IsDefined(typeof(PriutForAnimal.Entities.Type), animal.Type))
                return Result.Fail<Animal>("Недопустимый тип животного");

            if (!Enum.IsDefined(typeof(HealthStatus), animal.HealthStatus))
                return Result.Fail<Animal>("Не существует такого статуса здоровья");

            try
            {
                _context.Animals.Add(animal);
                await _context.SaveChangesAsync();
                return Result.Ok(animal);
            }
            catch (Exception ex)
            {
                return Result.Fail<Animal>($"Не удалось добавить животное: {ex.Message}");
            }
        }

        public async Task<Result<List<Animal>>> GetAnimalsByHealthStatus(HealthStatus status)
        {
            if (!Enum.IsDefined(typeof(HealthStatus), status))
                return Result.Fail<List<Animal>>("Недопустимый статус здоровья");

            var animals = await _context.Animals
                .Where(a => a.HealthStatus == status)
                .ToListAsync();

            return Result.Ok(animals);
        }

        public async Task<Result<Animal>> GetAnimalById(Guid id)
        {
            var animal = await _context.Animals.FindAsync(id);

            if (animal == null)
                return Result.Fail<Animal>("Животное не найдено");

            return Result.Ok(animal);
        }

        public async Task<Result<Animal>> UpdateAnimal(Animal animal)
        {
            if (animal == null)
                return Result.Fail<Animal>("Животное не может быть пустым");

            try
            {
                _context.Animals.Update(animal);
                await _context.SaveChangesAsync();
                return Result.Ok(animal);
            }
            catch (Exception ex)
            {
                return Result.Fail<Animal>($"Не удалось обновить животное: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAnimal(Guid id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
                return Result.Fail<bool>("Животное не найдено");

            try
            {
                _context.Animals.Remove(animal);
                await _context.SaveChangesAsync();
                return Result.Ok(true);
            }
            catch (Exception ex)
            {
                return Result.Fail<bool>($"Не удалось удалить животное: {ex.Message}");
            }
        }

        public async Task<Result<List<Animal>>> GetAllAnimals()
        {
            var animals = await _context.Animals.ToListAsync();
            return Result.Ok(animals);
        }
    }
}