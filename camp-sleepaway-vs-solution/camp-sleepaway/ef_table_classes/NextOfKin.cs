using camp_sleepaway.ef_table_classes;
using camp_sleepaway.helper_classes;
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
        [Required(ErrorMessage = "Invalid camper id.")]
        public int CamperId { get; set; }
        // Reference navigation to Camper
        public Camper Camper { get; set; } = null!;

        // empty constructor for Entity Framework
        public NextOfKin()
        {
        }

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
        public static NextOfKin InputNextOfKinData()
        {
            Console.Clear();
            Console.WriteLine("Add NextOfKin");
            Console.WriteLine();

            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            string phoneNumber;
            while (true)
            {
                try
                {
                    Console.Write("Phone number: ");
                    phoneNumber = Console.ReadLine();

                    if (IsPhoneNumberValid.IsPhoneNumber(phoneNumber))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid phone number");
                    }
                }
                catch
                {
                    Console.WriteLine("Error creating phone number");
                }
            }

            int relatedToCamper = int.Parse(Console.ReadLine());

            string relationType = Console.ReadLine();

            NextOfKin nextOfKin = new NextOfKin(firstName, lastName, phoneNumber, relatedToCamper, relationType);

            return nextOfKin;
        }

        public static NextOfKin ChooseNextOfKinToEdit()
        {
            using (var nextOfKinContext = new CampContext())
            {
                List<NextOfKin> nextOfKins = nextOfKinContext.NextOfKins.ToList();

                foreach (NextOfKin nextOfKin in nextOfKins)
                {
                    Console.WriteLine(nextOfKin.Id + " | " + nextOfKin.FirstName + " " + nextOfKin.LastName + " | " + nextOfKin.PhoneNumber);
                }

                Console.Write("Enter ID for camper you wish to edit: ");
                int nextOfKinId = int.Parse(Console.ReadLine());

                NextOfKin selectedNextOfKin = nextOfKinContext.NextOfKins.Where(c => c.Id == nextOfKinId).FirstOrDefault();

                return selectedNextOfKin;
            }
        }

        internal static NextOfKin EditNextOfKinMenu(NextOfKin nextOfKinToEdit)
        {
            var editNextOfKinMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]What do you want to do[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[]
                    {
                "Edit first name", "Edit last name", "Edit phone number", "Edit relation type"
                    }));

            if (editNextOfKinMenu == "Edit first name")
            {
                Console.Write("Enter new first name: ");
                string newFirstName = Console.ReadLine();

                while (true)
                {
                    if (Camper.IsLettersOnly(newFirstName))
                    {
                        nextOfKinToEdit.FirstName = newFirstName;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letters.");
                        Console.Write("First name: ");
                    }
                }
            }
            else if (editNextOfKinMenu == "Edit last name")
            {
                Console.Write("Enter new last name: ");
                string newLastName = Console.ReadLine();

                while (true)
                {
                    if (Camper.IsLettersOnly(newLastName))
                    {
                        nextOfKinToEdit.LastName = newLastName;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letters.");
                        Console.Write("Last name: ");
                    }
                }
            }
            else if (editNextOfKinMenu == "Edit phone number")
            {
                string newPhoneNumber;
                while (true)
                {
                    try
                    {
                        Console.Write("Phone number: ");
                        newPhoneNumber = Console.ReadLine();

                        if (IsPhoneNumberValid.IsPhoneNumber(newPhoneNumber))
                        {
                            nextOfKinToEdit.PhoneNumber = newPhoneNumber;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid phone number");
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error creating phone number");
                    }
                }
            }
            else if (editNextOfKinMenu == "Edit relation type")
            {
                Console.Write("Enter new relation type: ");
                string newRelationType = Console.ReadLine();

                while (true)
                {
                    if (Camper.IsLettersOnly(newRelationType))
                    {
                        nextOfKinToEdit.RelationType = newRelationType;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letters.");
                        Console.Write("Last name: ");
                    }
                }
            }

            return nextOfKinToEdit;
        }

        public void SaveToDb()
        {
            using (var nextOfKinContext = new CampContext())
            {
                nextOfKinContext.NextOfKins.Add(this);
                nextOfKinContext.SaveChanges();
            }
        }

        public void DeleteFromDb()
        {
            using (var nextOfKinContext = new CampContext())
            {
                nextOfKinContext.NextOfKins.Remove(this);
                nextOfKinContext.SaveChanges();
            }
        }
    }
}
