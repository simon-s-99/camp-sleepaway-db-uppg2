using System.ComponentModel.DataAnnotations;

namespace camp_sleepaway
{
    public class Camper : Person
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Invalid date of birth.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Invalid join date.")]
        public DateTime JoinDate { get; set; }

        public DateTime? LeaveDate { get; set; }

        // Reference navigation to Cabin
        public Cabin Cabin { get; set; } = null!;

        // Collection navigation to NextOfKin
        public List<NextOfKin> NextOfKins { get; set; } = new();

        public Camper ChooseCamperToEdit()
        {
            using (var camperContext = new CampContext())
            {
                List<Camper> campers = camperContext.Campers.ToList();
            
                foreach (Camper camper in campers)
                {
                    Console.WriteLine(camper.Id + " | " + camper.FirstName + " " + camper.LastName + " | " + camper.PhoneNumber);
                }

                Console.Write("Enter ID for camper you wish to edit: ");
                int camperID = int.Parse(Console.ReadLine());

                Camper selectedCamper = camperContext.Campers.Where(c => c.Id == camperID).FirstOrDefault();

                return selectedCamper;
            }
        }
    }
}
