using Microsoft.EntityFrameworkCore.Storage.Json;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace camp_sleepaway
{
    public class Camper : Person
    {
        [Key]
        public int Id { get; set; }

        //[Required(ErrorMessage = "Invalid date of birth.")]
        public DateTime DateOfBirth { get; set; }

        //[Required(ErrorMessage = "Invalid join date.")]
        public DateTime JoinDate { get; set; }

        public DateTime? LeaveDate { get; set; }

        // Reference navigation to Cabin
        public Cabin Cabin { get; set; } = null!;

        // Collection navigation to NextOfKin
        public List<NextOfKin> NextOfKins { get; set; } = new();

        // empty constructor for Entity Framework
        public Camper()
        {
        }

        // Constructor for camper
        [SetsRequiredMembers]
        public Camper(string firstName, string lastName, string phoneNumber,
        DateTime dateOfBirth, DateTime joinDate, DateTime? leaveDate = null)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            JoinDate = joinDate;
            LeaveDate = leaveDate;
        }

        private static bool IsLettersOnly(string input)
        {
            // Check if a string contains only letters
            // returns true if the input string contains only english and swedish letters, false otherwise          
            return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, "^[a-zA-ZåäöÅÄÖ]+$");
        }

        public static Camper InputCamperData()
        {
            Console.Clear();
            Console.WriteLine("Add camper");
            Console.WriteLine();

            Console.Write("First name: ");
            string firstName;
            while (true)
            {
                firstName = Console.ReadLine();
                if (IsLettersOnly(firstName))
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a name with only letter");
                Console.Write("First name: ");
            }

            Console.Write("Last name: ");
            string lastName;
            while (true)
            {
                lastName = Console.ReadLine();
                if (IsLettersOnly(lastName))
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a name with only letter");
                Console.Write("Last name: ");
            }

            string phoneNumber;
            while (true)
            {
                Console.Write("Phone number: ");
                phoneNumber = Console.ReadLine();

                if (phoneNumber.Length > 7 && phoneNumber.Length < 17)
                {
                    break;
                }

                Console.WriteLine("Please enter a phone number between 8 and 16 digits.");
            }

            Console.Write("Birth date: ");
            DateTime dateOfBirth;
            while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
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
            Console.WriteLine("");
            Console.WriteLine("Your camper has been added successfully.");

            Camper camper = new Camper(firstName, lastName, phoneNumber, dateOfBirth, joinDate, leaveDate);

            return camper;
        }



        public Camper ChooseCamperToEdit()
        {
            using (var camperContext = new CampContext())
            {
                List<Camper> campers = camperContext.Campers.ToList();

                foreach (Camper camper in campers)
                {
                    Console.WriteLine(camper.Id + " - " + camper.FirstName + " " + camper.LastName + " - " + camper.PhoneNumber);
                }

                Console.Write("Enter ID for camper you wish to edit: ");
                int camperID = int.Parse(Console.ReadLine());

                Camper selectedCamper = camperContext.Campers.Where(c => c.Id == camperID).FirstOrDefault();

                return selectedCamper;
            }
        }

        internal static void EditCamperMenu(Camper camperToEdit)
        {
            var editCamperMenu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]What do you want to do[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                .AddChoices(new[] {
                    "Edit first name", "Edit last name", "Edit phone number", "Edit birth date",
                    "Edit joined date", "Edit leave date"
                }));

            if (editCamperMenu == "Edit first name")
            {
                Console.Write("Enter new first name: ");
                string newFirstName = Console.ReadLine();

                while (true)
                {
                    if (IsLettersOnly(newFirstName))
                    {
                        camperToEdit.FirstName = newFirstName;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letter");
                        Console.Write("First name: ");
                    }
                }
            }
            else if (editCamperMenu == "Edit last name")
            {
                Console.Write("Enter new last name: ");
                string newLastName = Console.ReadLine();

                while (true)
                {
                    if (IsLettersOnly(newLastName))
                    {
                        camperToEdit.FirstName = newLastName;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letter");
                        Console.Write("Last name: ");
                    }
                }
            }
            else if (editCamperMenu == "Edit phone number")
            {
                Console.Write("Enter new phone number: ");
                string newPhoneNumber = Console.ReadLine();
                if (newPhoneNumber.Length > 7 && newPhoneNumber.Length < 17)
                {
                    camperToEdit.PhoneNumber = newPhoneNumber;
                }
                else
                {
                    Console.WriteLine("Invalid phone number. Phone number has not been updated.");
                }
            }
            else if (editCamperMenu == "Edit birth date")
            {
                Console.Write("Birth date: ");
                DateTime dateOfBirth;
                while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth))
                {
                    Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    Console.Write("Birth date: ");
                }
            }
            else if (editCamperMenu == "Edit joined date")
            {
                Console.Write("Join date: ");
                DateTime joinDate;
                while (!DateTime.TryParse(Console.ReadLine(), out joinDate))
                {
                    Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd.");
                    Console.Write("Join date: ");
                }
            }
            else if (editCamperMenu == "Edit leave date")
            {
                Console.Write("Enter new leave date (if there is no leave date, just press 'Enter' to skip): ");
                string leaveDateInput = Console.ReadLine();

                Console.Write("Join date: ");
                DateTime joinDate;
                while (!DateTime.TryParse(Console.ReadLine(), out joinDate))
                {
                    Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd.");
                    Console.Write("Join date: ");
                }
            }
        }

        public static void SearchCamper()
        {
            Console.Write("Search for camper by cabin or counselor: ");
            string searchQuery = Console.ReadLine();
            Console.WriteLine();

            using (var camperContext = new CampContext())
            {
                List<Camper> results = camperContext.Campers.Where(c => c.Cabin.CabinName == searchQuery ||
                c.Cabin.Counselor.FirstName == searchQuery ||
                c.Cabin.Counselor.LastName == searchQuery).ToList();
                // Return campers who satisfy one or more conditions

                foreach (Camper result in results)
                {
                    Console.WriteLine("Id: " + result.Id);
                    Console.WriteLine("Full name: " + result.FirstName + " " + result.LastName);
                    Console.WriteLine("Phone number: " + result.PhoneNumber);
                    Console.WriteLine("Birth date: " + result.DateOfBirth);
                    Console.WriteLine("Date joined: " + result.JoinDate);
                    Console.WriteLine("Date left/date to leave: " + result.LeaveDate);
                    Console.WriteLine("Cabin: " + result.Cabin.Id + " " + result.Cabin.CabinName);

                    Console.WriteLine(result.Cabin.Counselor != null ? "Cabin counselor: " + result.Cabin.Counselor.FirstName + " " + result.Cabin.Counselor.LastName : "Warning! This cabin has no active counselor!");
                    // If counselor is not null then print out normally, if it is null then warn the user
                }
            }
        }

        public void DisplayCampersAndNextOfKins()
        {
            // Temporarily commented out, implement result or other method body 
            /*
            Console.Write("NextOfKins: ");
            foreach (NextOfKin nextOfKin in result.NextOfKins)
            {
                Console.WriteLine(nextOfKin.FirstName + " " + nextOfKin.LastName + " - " + nextOfKin.RelationType);
            }
            // Print each NextOfKin, for each result
            */
        }
    }
}
