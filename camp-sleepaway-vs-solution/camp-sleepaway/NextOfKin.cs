using System.ComponentModel.DataAnnotations;

namespace camp_sleepaway
{
    // Represents a NextOfKin table in Entity Frameworks
    public class NextOfKin : Person
    {
        [Key]
        public int Id { get; set; }
        public required int RelatedToCamper { get; set; }
        public string RelationType { get; set; }
    }
}
