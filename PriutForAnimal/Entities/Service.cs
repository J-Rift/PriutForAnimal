using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimal.Entities
{
    public class Service
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public Guid AdminId { get; set; } 
        public Admin Admin { get; set; }  
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
