using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimal.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public string AdminPassword { get; set; } = null!;
        public string EmployeePassword { get; set; } = null!;
        public Guid AdminId { get; set; }
        public Admin Admin { get; set; } = null!;
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
    }
}
