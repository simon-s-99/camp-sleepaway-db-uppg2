using System.ComponentModel.DataAnnotations;

namespace camp_sleepaway
{
    public class Camper : Person
    {
        [Key]
        public int Id { get; set; }
        public required DateTime BirthDate { get; set; }
        public required DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }

        // Reference navigation to Cabin
        public Cabin Cabin { get; set; } = null!;

        // Collection navigation to NextOfKin
        public List<NextOfKin> NextOfKins { get; set; } = new();

        public Camper ChooseCamperToEdit()
        {
            using (var context = new Context())
            {
                List<Camper> campers = context.Campers.ToList();
            
                foreach (Camper camper in campers)
                {
                    Console.WriteLine(camper.Id + " | " + camper.FirstName + " " + camper.LastName + " | " + camper.PhoneNumber);
                }

                Console.Write("Enter ID for camper you wish to edit: ");
                int camperID = int.Parse(Console.ReadLine());

                Camper selectedCamper = context.Campers.Where(c => c.Id == camperID).FirstOrDefault();

                return selectedCamper;
            }
        }
    }
}
