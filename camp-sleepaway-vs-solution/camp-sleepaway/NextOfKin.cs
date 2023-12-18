namespace camp_sleepaway
{
    // Represents a NextOfKin table in Entity Frameworks
    public class NextOfKin : Person
    {
        public required int RelatedToCamper { get; set; }
        public string RelationType { get; set; }
    }
}
