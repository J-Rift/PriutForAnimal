using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimal.Entities
{
    public class Employee
    {
        public Guid EmployeeId { get; set; }
        public string MiddleName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public Role Role { get; set; } = null!;
        public Guid RoleId { get; set; }
        public List<Visitor> Visitors { get; set; } = new List<Visitor>();
        public List<Service> Services { get; set; } = new List<Service>();
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
