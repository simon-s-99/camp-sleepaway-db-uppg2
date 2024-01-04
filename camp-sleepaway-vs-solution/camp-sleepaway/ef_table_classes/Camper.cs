using camp_sleepaway.ef_table_classes;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static camp_sleepaway.Helper;

// Represents Camper table in Entity Framework

// Samuel Lööf, Simon Sörqvist, Adam Kumlin

namespace camp_sleepaway
{
    public class Camper : Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Invalid date of birth.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Invalid join date.")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LeaveDate { get; set; }

        [ForeignKey("CabinId")]
        public int? CabinId { get; set; }
        // Reference navigation to Cabin
        public Cabin? Cabin { get; set; } = null!;

        // Collection navigation to NextOfKin
        public ICollection<NextOfKin> NextOfKins { get; set; } = new List<NextOfKin>();

        // empty constructor for Entity Framework
        public Camper()
        {
        }

        public static Camper InputCamperData()
        {
            Console.Clear();
            Console.WriteLine("Add camper");
            Console.WriteLine();

            var camper = new Camper();

            // first name input
            camper = EditCamperMenu(camper, 0);

            // last name input
            camper = EditCamperMenu(camper, 1);

            // phone number input
            camper = EditCamperMenu(camper, 2);

            // date of birth input 
            camper = EditCamperMenu(camper, 3);

            // join date input
            camper = EditCamperMenu(camper, 4);

            // leave date input 
            camper = EditCamperMenu(camper, 5);

            Cabin[] cabins = Cabin.GetAllFromDb();

            Console.WriteLine();
            Console.WriteLine("Cabins: ");

            foreach (Cabin cabin in cabins)
            {
                Console.WriteLine(cabin.Id + " " + cabin.CabinName);
            }
            Console.WriteLine();

            Console.Write("Enter the ID for the cabin to associate this camper with: ");

            int cabinId;

            while (true)
            {
                bool cabinDoesExist = false;

                while (!int.TryParse(Console.ReadLine(), out cabinId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    Console.Write("Enter the ID for the cabin to associate this camper with: ");
                }

                foreach (Cabin cabin in cabins)
                {
                    if (cabin.Id == cabinId)
                    {
                        cabinDoesExist = true;
                    }
                }

                if (!cabinDoesExist)
                {
                    Console.WriteLine("This cabin does not exist!");
                    Console.Write("Enter the ID for the cabin to associate this camper with: ");
                }
                else if (GetCabinFromCabinId(cabinId).Campers.Count >= 4)
                {
                    Console.WriteLine("This cabin is full!");
                    Console.Write("Enter the ID for the cabin to associate this camper with: ");
                }
                else if (GetCabinFromCabinId(cabinId).CounselorId == null)
                {
                    Console.WriteLine("This cabin has no active counselor. Assigning a camper to this cabin is therefore not possible.");
                    Console.Write("Enter the ID for the cabin to associate this camper with: ");
                }
                else
                {
                    break;
                }
            }

            camper.CabinId = cabinId;

            Console.WriteLine("");
            Console.WriteLine("Your camper has been added successfully.");

            return camper;
        }

        public static Camper ChooseCamperMenu()
        {
            using (var camperContext = new CampContext())
            {
                List<Camper> campers = camperContext.Campers.ToList();

                Console.WriteLine("ID | Full Name | Phone-nr. | Date of Birth | " +
                    "Joined Camp Date | Will Leave/Left Camp Date");

                foreach (Camper camper in campers)
                {
                    Console.WriteLine($"{camper.Id} | {camper.FirstName} {camper.LastName} | " +
                        $"{camper.PhoneNumber} | {Helper.FormatDate(camper.DateOfBirth)} | " +
                        $"{Helper.FormatDate(camper.JoinDate)} | {Helper.FormatDate(camper.LeaveDate)}");
                }

                Console.Write("Enter ID for the 'camper' you wish to select: ");
                int camperId;
                while (!int.TryParse(Console.ReadLine(), out camperId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    Console.Write("Enter ID for the 'camper' you wish to select: ");
                }

                Camper selectedCamper = camperContext.Campers.FirstOrDefault(c => c.Id == camperId);

                return selectedCamper;
            }
        }

        internal static Camper EditCamperMenu(Camper camperToEdit, int? menuChoice)
        {
            string? editCamperMenu = null;
            string[] editCamperMenuChoices =
            {
                "Edit first name", "Edit last name", "Edit phone number",
                "Edit birth date", "Edit joined date", "Edit leave date","Edit cabin"
            };

            if (menuChoice.HasValue)
            {
                editCamperMenu = editCamperMenuChoices[menuChoice.Value];
            }
            else
            {
                Console.Clear();

                editCamperMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]What do you want to do[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(editCamperMenuChoices));

                Console.Clear();
            }

            // edit first name
            if (editCamperMenu == editCamperMenuChoices[0])
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
            // edit last name
            else if (editCamperMenu == editCamperMenuChoices[1])
            {
                Console.Write("Enter new last name: ");
                string newLastName = Console.ReadLine();

                while (true)
                {
                    if (IsLettersOnly(newLastName))
                    {
                        camperToEdit.LastName = newLastName;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letter");
                        Console.Write("Last name: ");
                    }
                }
            }
            // edit phone nr. 
            else if (editCamperMenu == editCamperMenuChoices[2])
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
                            camperToEdit.PhoneNumber = newPhoneNumber;
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
            // edit date of birth
            else if (editCamperMenu == editCamperMenuChoices[3])
            {
                DateTime dateOfBirth;
                bool validDate = false;

                while (!validDate)
                {
                    Console.Write("Birth date: ");
                    validDate = DateTime.TryParse(Console.ReadLine(), out dateOfBirth);

                    if (validDate)
                    {
                        int comparisonAge = CalculateAge(dateOfBirth);

                        if (comparisonAge < 7 || comparisonAge > 17)
                        {
                            Console.WriteLine("The camper must be at least 7 years " +
                                "old but not older than 17 years old.");
                            validDate = false;
                        }
                        else
                        {
                            camperToEdit.DateOfBirth = dateOfBirth;
                            // let validDate be true so that while-loop breaks 
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    }
                }
            }
            // edit join date (camp join date)
            else if (editCamperMenu == editCamperMenuChoices[4])
            {
                DateTime joinDate;
                bool validDate = false;

                while (!validDate)
                {
                    Console.Write("Camp join date: ");
                    validDate = DateTime.TryParse(Console.ReadLine(), out joinDate);

                    if (validDate)
                    {
                        DateTime comparisonTime = camperToEdit.DateOfBirth;
                        if (joinDate < comparisonTime.AddYears(7))
                        {
                            Console.WriteLine("The camper must be at least 7 years " +
                                "old on their join date.");
                            validDate = false;
                        }
                        else if (joinDate > DateTime.Now)
                        {
                            Console.WriteLine("Join date cannot be in the future.");
                            validDate = false;
                        }
                        else if (joinDate > camperToEdit.LeaveDate)
                        {
                            Console.WriteLine("Join date can not be after leave date.");
                            validDate = false;
                        }
                        else
                        {
                            camperToEdit.JoinDate = joinDate;
                            // let validDate be true so that while-loop breaks 
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    }
                }
            }
            // edit date camper will leave / date camper left 
            else if (editCamperMenu == editCamperMenuChoices[5])
            {
                DateTime leaveDate;
                bool validDate = false;

                while (!validDate)
                {
                    Console.Write("Enter new leave date (if there is no leave date, just press 'Enter' to skip): ");
                    string leaveDateInput = Console.ReadLine();

                    // This skips adding a leave date
                    if (string.IsNullOrEmpty(leaveDateInput))
                    {
                        camperToEdit.LeaveDate = null;
                        validDate = true;
                        break;
                    }

                    validDate = DateTime.TryParse(leaveDateInput, out leaveDate);

                    if (validDate)
                    {
                        if (leaveDate < camperToEdit.JoinDate)
                        {
                            Console.WriteLine("Leave date can not be set to before join date.");
                            validDate = false;
                        }
                        else
                        {
                            camperToEdit.LeaveDate = leaveDate;
                            // let validDate be true so that while-loop breaks 
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    }
                }
            }

            else if (editCamperMenu == editCamperMenuChoices[6]) // Edit cabin
            {
                Cabin[] cabins = Cabin.GetAllFromDb();

                Console.WriteLine();
                Console.WriteLine("Cabins: ");

                foreach (Cabin cabin in cabins)
                {
                    Console.WriteLine(cabin.Id + " " + cabin.CabinName);
                }
                Console.WriteLine();

                Console.Write("Enter the ID for the new cabin to associate this camper with: ");

                int newCabinId;

                while (true)
                {
                    bool cabinDoesExist = false;

                    while (!int.TryParse(Console.ReadLine(), out newCabinId))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer.");
                        Console.Write("Enter the ID for the new cabin to associate this camper with: ");
                    }

                    foreach (Cabin cabin in cabins)
                    {
                        if (cabin.Id == newCabinId)
                        {
                            cabinDoesExist = true;
                        }
                    }

                    int nrOfCampersInCabin = GetNumberOfCampersInCabin(newCabinId);

                    if (!cabinDoesExist)
                    {
                        Console.WriteLine("This cabin does not exist!");
                        Console.Write("Enter the ID for the new cabin to associate this camper with: ");
                    }
                    else if (nrOfCampersInCabin >= 4)
                    {
                        Console.WriteLine("This cabin is full!");
                        Console.Write("Enter the ID for the new cabin to associate this camper with: ");
                    }
                    else if (GetCabinFromCabinId(newCabinId).CounselorId == null)
                    {
                        Console.WriteLine("This cabin has no active counselor. Assigning a camper to this cabin is therefore not possible.");
                        Console.Write("Enter the ID for the new cabin to associate this camper with: ");
                    }
                    else
                    {
                        camperToEdit.CabinId = newCabinId;
                        Console.WriteLine("Cabin successfully updated!");
                        break;
                    }
                }
            }

            return camperToEdit;
        }

        public static void SearchCamper()
        {
            Console.WriteLine("Search for camper based on the name of the cabin " +
                "or counselor they are assigned to. ");
            Console.Write("(Press enter if you want to go back) : ");
            string searchQuery = Console.ReadLine();
            Console.WriteLine();

            if (string.IsNullOrEmpty(searchQuery))
            { // skips below code
            }
            else
            {
                using (var camperContext = new CampContext())
                {
                    // Return campers who satisfy one or more conditions
                    List<Camper> results = camperContext.Campers.Where(c => c.Cabin.CabinName.Contains(searchQuery) ||
                    c.Cabin.Counselor.FirstName.Contains(searchQuery) ||
                    c.Cabin.Counselor.LastName.Contains(searchQuery)).ToList();

                    foreach (Camper result in results)
                    {
                        Console.WriteLine("Id: " + result.Id);
                        Console.WriteLine("Full name: " + result.FirstName + " " + result.LastName);
                        Console.WriteLine("Phone number: " + result.PhoneNumber);
                        Console.WriteLine("Birth date: " + Helper.FormatDate(result.DateOfBirth));
                        Console.WriteLine("Date joined: " + Helper.FormatDate(result.JoinDate));
                        Console.WriteLine("Date left/date to leave: " + Helper.FormatDate(result.LeaveDate));

                        Cabin resultCabin = GetCabinFromCabinId(result.CabinId);
                        Console.WriteLine("Cabin: " + resultCabin.Id + " " + resultCabin.CabinName);

                        // If counselor is not null then print out normally, if it is null then warn the user
                        Counselor resultCounselor = GetCounselorFromCabinId(result.CabinId);
                        Console.WriteLine(resultCounselor != null ? "Cabin counselor: " + resultCounselor.FirstName + " "
                            + resultCounselor.LastName : "Warning! This cabin has no active counselor!");

                        Console.WriteLine();
                    }
                }
            }
        }

        public static void DisplayCampersAndNextOfKins()
        {
            using (var camperContext = new CampContext())
            {
                List<Camper> campers = camperContext.Campers.OrderBy(c => c.Cabin.Id).ToList();
                List<NextOfKin> nextOfKins = NextOfKin.GetAllFromDb().ToList();

                foreach (Camper camper in campers)
                {
                    Console.WriteLine("Id: " + camper.Id);
                    Console.WriteLine("Full name: " + camper.FirstName + " " + camper.LastName);
                    Console.WriteLine("Phone number: " + camper.PhoneNumber);
                    Console.WriteLine("Birth date: " + Helper.FormatDate(camper.DateOfBirth));
                    Console.WriteLine("Date joined: " + Helper.FormatDate(camper.JoinDate));
                    Console.WriteLine("Date left/date to leave: " + Helper.FormatDate(camper.LeaveDate));

                    Cabin resultCabin = GetCabinFromCabinId(camper.CabinId);
                    Console.WriteLine("Cabin: " + resultCabin.Id + " " + resultCabin.CabinName);

                    List<NextOfKin> campersNextOfKins = nextOfKins.Where(n => n.CamperId == camper.Id).ToList();

                    // Check if camper has nextofkins
                    if (campersNextOfKins.Count() <= 0)
                    {
                        Console.WriteLine("This camper has no NextOfKins.");
                    }
                    else
                    {
                        Console.WriteLine("NextOfKins: ");
                        foreach (NextOfKin nextOfKin in campersNextOfKins)
                        {
                            Console.WriteLine(nextOfKin.FirstName + " " + nextOfKin.LastName + 
                                " - " + nextOfKin.RelationType);
                        }
                    }

                    Console.WriteLine();
                }
            }
        }


        public static NextOfKin[] GetNextOfKinsFromCamperID(int camperId)
        {
            using (var camperContext = new CampContext())
            {
                NextOfKin[] nextOfKins = camperContext.NextOfKins.Where(c => c.CamperId == camperId).ToArray();

                return nextOfKins;
            }
        }

        public static Cabin GetCabinFromCabinId(int? cabinId)
        {
            using (var camperContext = new CampContext())
            {
                Cabin cabin = camperContext.Cabins.Where(c => c.Id == cabinId).FirstOrDefault();

                return cabin;
            }
        }

        private static Counselor GetCounselorFromCabinId(int? cabinId)
        {
            using (var camperContext = new CampContext())
            {
                Counselor counselor = camperContext.Counselors.Where(c => c.Cabin.Id == cabinId).FirstOrDefault();

                return counselor;
            }
        }

        private static int GetNumberOfCampersInCabin(int cabinId)
        {
            Camper[] campers = Camper.GetAllFromDb();

            int counter = 0;

            foreach (Camper camper in campers)
            {
                if (camper.CabinId == cabinId)
                {
                    counter++;
                }
            }

            return counter;
        }

        public static Camper[] GetAllFromDb()
        {
            var result = new List<Camper>();
            using (var context = new CampContext())
            {
                result = context.Campers.ToList();
            }

            return result.ToArray();
        }

        public void SaveToDb()
        {
            using (var camperContext = new CampContext())
            {
                camperContext.Campers.Add(this);
                camperContext.SaveChanges();
            }
        }

        public void UpdateRecordInDb()
        {
            using (var camperContext = new CampContext())
            {
                camperContext.Update(this);
                camperContext.SaveChanges();
            }
        }

        public void DeleteFromDb()
        {
            using (var camperContext = new CampContext())
            {
                camperContext.Campers.Remove(this);
                camperContext.SaveChanges();
            }
        }
    }
}
