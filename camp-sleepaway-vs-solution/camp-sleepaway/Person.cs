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
        [Required(ErrorMessage = "Invalid first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Invalid last name.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }
    }
}
