using System.ComponentModel.DataAnnotations;

namespace camp_sleepaway
{
    public enum WorkTitle
    {
        Teacher, Parent, Coach, Other
    }

    public class Counselor : Person
    {
        [Key]
        public int Id { get; set; }
        public required WorkTitle WorkTitle { get; set; }
        public required DateTime HiredDate { get; set; }
        public DateTime? TerminationDate { get; set; }

    }
}
