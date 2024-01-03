using camp_sleepaway.ef_table_classes;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static camp_sleepaway.Helper;

// Represents NextOfKin table in Entity Framework

// Samuel Lööf, Simon Sörqvist, Adam Kumlin

namespace camp_sleepaway
{
    public class NextOfKin : Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? RelationType { get; set; }

        // Foreign key property to Camper
        [ForeignKey("CamperId")]
        public int? CamperId { get; set; }
        // Reference navigation to Camper
        public Camper? Camper { get; set; }

        // empty constructor for Entity Framework
        public NextOfKin()
        {
        }

        // Asks user for input via console, should primarily be called from main menu 
        public static NextOfKin InputNextOfKinData()
        {
            Console.Clear();
            Console.WriteLine("Add NextOfKin");
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
                try
                {
                    Console.Write("Phone number: ");
                    phoneNumber = Console.ReadLine();

                    if (IsPhoneNumberValid(phoneNumber, false))
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

            string relationType;
            Console.Write("Relation type to the camper: ");

            while (true)
            {
                relationType = Console.ReadLine();
                if (IsLettersOnly(relationType))
                {
                    break;
                }
                Console.WriteLine("Invalid input. Please enter a name with only letter");
                Console.Write("Relation type: ");
            }

            Console.Clear();
            Console.WriteLine("Which camper should she/he be related to? ");

            Camper relatedToCamper = SelectCamper();

            NextOfKin nextOfKin = new NextOfKin
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                CamperId = relatedToCamper.Id,
                RelationType = relationType
            };

            return nextOfKin;
        }

        public static Camper SelectCamper()
        {
            using (var camperContext = new CampContext())
            {
                var campers = camperContext.Campers.ToList();

                // Create a root node for the tree
                var root = new Tree("Select camper to relate to NextOfKin");

                foreach (var camper in campers)
                {
                    // TreeNode and Narkup are used here to create structured and formatted console output
                    var camperNode = new TreeNode(new Markup($"{camper.Id}  {camper.FirstName} {camper.LastName}"));
                    root.AddNode(camperNode);
                }

                AnsiConsole.Render(root);

                // Prompt the user to enter the name or ID of the camper they want to relate to
                var userInput = AnsiConsole.Prompt<string>(
                    new TextPrompt<string>("Enter the name or ID of the camper you want to relate NextOfKin to")
                        .Validate(input =>
                        {
                            // Validate that the entered input matches either the ID or the full name of a camper
                            return campers.Any(c => c.Id.ToString() == input || $"{c.FirstName} {c.LastName}" == input);
                        }));

                // Check if the input is a camper ID or a camper name
                if (int.TryParse(userInput, out int selectedCamperId))
                {
                    // Return the selected camper based on ID
                    return campers.FirstOrDefault(c => c.Id == selectedCamperId);
                }
                else
                {
                    // Return the selected camper based on name
                    return campers.FirstOrDefault(c => $"{c.FirstName} {c.LastName}" == userInput);
                }
            }
        }

        public static NextOfKin ChooseNextOfKinMenu()
        {
            using (var nextOfKinContext = new CampContext())
            {
                List<NextOfKin> nextOfKins = nextOfKinContext.NextOfKins.ToList();

                Console.WriteLine("ID | Full Name | Phone-nr. | Related to Camper with ID | Relation Type");

                foreach (NextOfKin nextOfKin in nextOfKins)
                {
                    Console.WriteLine($"{nextOfKin.Id} | {nextOfKin.FirstName} {nextOfKin.LastName} |" +
                        $" {nextOfKin.PhoneNumber} | {nextOfKin.CamperId} | {nextOfKin.RelationType}");

                }

                Console.Write("Enter ID for the 'next of kin' you wish to select: ");
                int nextOfKinId;
                while (!int.TryParse(Console.ReadLine(), out nextOfKinId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    Console.Write("Enter ID for the 'next of kin' you wish to select: ");
                }

                NextOfKin selectedNextOfKin = nextOfKinContext.NextOfKins.FirstOrDefault(c => c.Id == nextOfKinId);

                return selectedNextOfKin;
            }
        }

        internal static NextOfKin EditNextOfKinMenu(NextOfKin nextOfKinToEdit)
        {
            Console.Clear();

            var editNextOfKinMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]What do you want to do[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[]
                    {
                "Edit first name", "Edit last name", "Edit phone number", "Edit relation type/name", "Edit which camper she/he is related to"
                    }));

            Console.Clear();

            if (editNextOfKinMenu == "Edit first name")
            {
                Console.Write("Enter new first name: ");
                string newFirstName = Console.ReadLine();

                while (true)
                {
                    if (IsLettersOnly(newFirstName))
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
                    if (IsLettersOnly(newLastName))
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

                        if (IsPhoneNumberValid(newPhoneNumber, false))
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
            else if (editNextOfKinMenu == "Edit relation type/name")
            {
                Console.Write("Enter new relation type/name: ");
                string newRelationType = Console.ReadLine();

                while (true)
                {
                    if (IsLettersOnly(newRelationType))
                    {
                        nextOfKinToEdit.RelationType = newRelationType;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letters.");
                        Console.Write("Last name: ");
                        newRelationType = Console.ReadLine();
                    }
                }
            }
            else if (editNextOfKinMenu == "Edit which camper she/he is related to")
            {
                Console.WriteLine("Select a camper:");

                // Call the SelectCamper method to get the Camper object
                Camper selectedCamper = SelectCamper();

                // Set the selected camper's Id as the new related camper Id
                nextOfKinToEdit.CamperId = selectedCamper.Id;

                Console.WriteLine($"Relation updated to camper with ID: {selectedCamper.Id}");
            }

            return nextOfKinToEdit;
        }

        public static NextOfKin[] GetAllFromDb()
        {
            var result = new List<NextOfKin>();
            using (var context = new CampContext())
            {
                result = context.NextOfKins.ToList();
            }

            return result.ToArray();
        }

        public void SaveToDb()
        {
            using (var nextOfKinContext = new CampContext())
            {
                nextOfKinContext.NextOfKins.Add(this);
                nextOfKinContext.SaveChanges();
            }
        }

        public void UpdateRecordInDb()
        {
            using (var nextOfKinContext = new CampContext())
            {
                nextOfKinContext.Update(this);
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
