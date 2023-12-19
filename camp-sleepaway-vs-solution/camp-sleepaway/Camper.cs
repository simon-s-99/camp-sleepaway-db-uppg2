using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

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

        // Constructor for camper
        public Camper () { }

        [SetsRequiredMembers]
        public Camper (string firstName, string lastName, string phoneNumber,
        DateTime birthDate, DateTime joinDate, DateTime? leaveDate = null)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            JoinDate = joinDate;
            LeaveDate = leaveDate;          
        }

        private static bool IsLettersOnly(string input)
        {
            return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, "^[a-zA-ZåäöÅÄÖ]+$");
        }

        public Camper InputCamperData()
        {
            Console.Clear();
            Console.WriteLine("Add camper");
            Console.WriteLine();

            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Phone number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Birth date: ");
            DateTime birthDate;
            while (!DateTime.TryParse(Console.ReadLine(), out birthDate))
            {
                Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                Console.Write("Birth date: ");
            }

            Console.Write("Join date: ");
            DateTime joinDate;
            while (!DateTime.TryParse(Console.ReadLine(), out joinDate))
            {
                Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd.");
                Console.Write("Join date: ");
            }

            Console.Write("Leave date (if there is no leave date, just press 'Enter' to skip): ");
            DateTime? leaveDate = null;
            string leaveDateInput = Console.ReadLine();
            DateTime parsedLeaveDate;

            if (!string.IsNullOrEmpty(leaveDateInput))
            {
                while (!DateTime.TryParse(leaveDateInput, out parsedLeaveDate))
                {
                    Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd' or 'Enter' to skip.");
                    Console.Write("Leave date: ");
                    leaveDateInput = Console.ReadLine();
                }

                leaveDate = parsedLeaveDate;
            }



            // Code for once we have a method to create a cabin
            // Cabin cabin = CreateCabin():

            Camper camper = new Camper(firstName, lastName, phoneNumber, birthDate, joinDate, leaveDate);

            return camper;
        }

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
