using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace camp_sleepaway
{
    public abstract class Person
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string PhoneNumber { get; set; }
    }
}
