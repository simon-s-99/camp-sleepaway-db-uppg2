namespace camp_sleepaway
{
    public class CamperCabin
    {
        // collection navigation to Camper
        public List<Camper> Campers { get; set; } = new List<Camper>();

        // reference navigation to Cabin
        public Cabin Cabin { get; set; } = null!;

        // empty constructor for EF core
        public CamperCabin()
        {
        }
    }
}
