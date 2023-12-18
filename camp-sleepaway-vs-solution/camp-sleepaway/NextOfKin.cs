using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace camp_sleepaway
{
    // Represents a NextOfKin table in Entity Frameworks
    public class NextOfKin : Person
    {
        [Key]
        public int Id { get; set; }
        public string RelationType { get; set; }

        // Foreign key property to Camper
        public required int CamperId { get; set; }
        // Reference navigation to Camper
        public Camper Camper { get; set; } = null!;

        [SetsRequiredMembers]
        public NextOfKin(string firstName, string lastName, string phoneNumber,
            int relatedToCamper, string relationType)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            CamperId = relatedToCamper;
            RelationType = relationType;
        }

        // Asks user for input via console, should primarily be called from main menu 
        public NextOfKin InputCounselorData()
        {
            Console.Clear();
            Console.WriteLine("Add NextOfKin");
            Console.WriteLine();

            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Phone number: ");
            string phoneNumber = Console.ReadLine();

            int relatedToCamper = int.Parse(Console.ReadLine());

            string relationType = Console.ReadLine();

            NextOfKin nextOfKin = new NextOfKin(firstName, lastName, phoneNumber, relatedToCamper, relationType);

            return nextOfKin;
        }
    }
}
