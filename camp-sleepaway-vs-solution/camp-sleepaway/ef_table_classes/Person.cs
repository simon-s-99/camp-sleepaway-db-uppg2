using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// This does not represent a table in Entity Framework Core
// this is a parent class that other EF Table classes inherit from 

namespace camp_sleepaway.ef_table_classes
{
    public abstract class Person
    {
        [Required(ErrorMessage = "Invalid first name.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Invalid last name.")]
        public string LastName { get; set; }

        // maybe add [PhoneAttribute] to PhoneNumber ?? (annotation) 
        [Required(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }
    }
}
