using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriutForAnimal.Entities
{
    public class Visitor
    {
        public int VisitorId { get; set; }
        public string MiddleName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string ResidentialAddress { get; set; } = null!;


        public Animal Animal { get; set; } = null!;
        public int AnimalId { get; set; }
    }
}
