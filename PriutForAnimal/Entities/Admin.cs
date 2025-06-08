using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimal.Entities
{
    public class Admin
    {
        public Guid AdminId { get; set; }
        public string MiddleName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public Role Role { get; set; } = null!;
        public List<Service> Services { get; set; } = new List<Service>();
    }
}
