using System.ComponentModel.DataAnnotations;

// This does not represent a table in Entity Framework Core
// this is a parent class that other EF Table classes inherit from 

// Samuel Lööf, Simon Sörqvist, Adam Kumlin

namespace camp_sleepaway.ef_table_classes
{
    public abstract class Person
    {
        [Required(ErrorMessage = "Invalid first name.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Invalid last name.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Invalid phone number.")]
        [StringLength(16)] // supports a swedish phone-nr. formatted like this: +46 707 11 22 33 (with white spaces)
        public string PhoneNumber { get; set; }
    }
}
