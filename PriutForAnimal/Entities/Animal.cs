using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimal.Entities
{
    public class Animal
    {
        public Guid AnimalId { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public HealthStatus HealthStatus { get; set; }
        public int VisitCount { get; set; }
        public List<Visitor> Visitors { get; set; } = new List<Visitor>();

        public string AnimalInfoDisplay() 
        {
            return $"ID: {AnimalId}, Type: {Type}, Name: {Name}, Age: {Age}, Health: {HealthStatus}, Visits: {VisitCount}";
        }
    }
    public enum Type
    {
        Cat,
        Dog
    }
    public enum HealthStatus
    {
        Healthy,
        Sick
    }
    
}
