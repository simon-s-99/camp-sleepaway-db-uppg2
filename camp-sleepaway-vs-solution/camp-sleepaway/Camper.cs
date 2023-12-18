using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camp_sleepaway
{
    public class Camper : Person
    {
        public required Id { get; set; } 
        public required DateTime BirthDate { get; set; }
        public required DateTime JoinedDate { get; set; }
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
