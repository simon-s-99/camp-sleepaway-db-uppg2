using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace camp_sleepaway
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
