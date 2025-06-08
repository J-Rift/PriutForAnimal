using Microsoft.EntityFrameworkCore;
using PriutForAnimal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimalDbContext.Context
{
    public class DbController : DbContext
    {
        private DbController()
        {
            try
            {
                SetSqlServerConnection();
                Debug.WriteLine($"{GetType().Name} created! Context type is {_appDbContext.GetType().Name}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private AppDbContext _appDbContext = null!;
        private static DbController _instance = null!;

        internal AppDbContext GetContext()
        {
            return _appDbContext;
        }

        internal static DbController GetInstance()
        {
            return _instance ??= new DbController();
        }

        internal void SetSqlServerConnection()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-RCMJD1T;Database=PriutForAnimal;Trusted_Connection=True;TrustServerCertificate=True;");

            _appDbContext = new AppDbContext(optionsBuilder.Options);
        }
    }
}
