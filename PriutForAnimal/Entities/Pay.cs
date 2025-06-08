using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimal.Entities
{
    public class Pay
    {
        public Guid PayId { get; set; }
        public string Name {  get; set; }
        public int EmployeeId { get; set; }
        public int ServiceId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
