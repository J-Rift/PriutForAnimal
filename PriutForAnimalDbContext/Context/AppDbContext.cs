using Microsoft.EntityFrameworkCore;
using PriutForAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimalDbContext.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Pay> Pays { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Visitor> Visitors { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связей и ключей

            modelBuilder.Entity<Animal>()
                .HasKey(a => a.AnimalId);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Type)
                .HasConversion<string>(); // Сохраняем enum как строку в БД

            modelBuilder.Entity<Animal>()
                .Property(a => a.HealthStatus)
                .HasConversion<string>();

           
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.EmployeeId);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.Admin)
                .WithMany()
                .HasForeignKey(r => r.AdminId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.Employee)
                .WithMany()
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Service>()
                .HasKey(s => s.ServiceId);

            
            modelBuilder.Entity<Pay>()
                .HasKey(p => p.PayId);

            modelBuilder.Entity<Pay>()
                .Property(p => p.EmployeeId)
                .HasConversion<int>();

            modelBuilder.Entity<Pay>()
                .Property(p => p.ServiceId)
                .HasConversion<int>();

          
            modelBuilder.Entity<Visitor>()
                .HasKey(v => v.VisitorId);

            modelBuilder.Entity<Visitor>()
                .Property(v => v.AnimalId)
                .HasConversion<int>();

            modelBuilder.Entity<Visitor>()
                .HasOne(v => v.Animal)
                .WithMany(a => a.Visitors)
                .HasForeignKey(v => v.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь Employee ↔ Service (многие-ко-многим, только чтение)
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Services)
                .WithMany() // У Service нет обратной навигации к Employee
                .UsingEntity(j => j.ToTable("EmployeeServices"));

            // Связь Admin ↔ Service (один-ко-многим, полный доступ)
            modelBuilder.Entity<Admin>()
                .HasMany(a => a.Services)
                .WithOne() // У Service нет обратной навигации к Admin
                .HasForeignKey(s => s.AdminId) // Добавляем AdminId в Service
                .OnDelete(DeleteBehavior.Cascade); // При удалении админа его услуги удаляются
        }
    }
}
