using camp_sleepaway.ef_table_classes;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static camp_sleepaway.Helper;

// Represents Counselor table in Entity Framework

// Samuel Lööf, Simon Sörqvist, Adam Kumlin

namespace camp_sleepaway
{
    public enum WorkTitle
    {
        Teacher, Coach, Other
    }

    public class Counselor : Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Invalid work title.")]
        [EnumDataType(typeof(WorkTitle))]
        public WorkTitle WorkTitle { get; set; }

        [Required(ErrorMessage = "Invalid hire date.")]
        [DataType(DataType.Date)]
        public DateTime HiredDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        [ForeignKey("CabinId")]
        public int? CabinId { get; set; }

        // Reference navigation to Cabin
        public Cabin? Cabin { get; set; }

        // empty constructor for Entity Framework
        public Counselor()
        {
        }

        // Asks user for input via console, should primarily be called from main menu 
        public static Counselor InputCounselorData()
        {
            Console.Clear();
            Console.WriteLine("Add counselor");
            Console.WriteLine();

            Counselor counselor = null;
            string firstName;

            while (true)
            {
                Console.Write("First name: ");
                firstName = Console.ReadLine();

                if (IsLettersOnly(firstName))
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a name with only letter");
                Console.Write("First name: ");
            }

            string lastName;
            while (true)
            {
                Console.Write("Last name: ");
                lastName = Console.ReadLine();

                if (IsLettersOnly(lastName))
                {
                    break;
                }

                Console.WriteLine("Invalid input. Please enter a name with only letters.");
                Console.Write("Last name: ");
            }

            string phoneNumber;
            while (true)
            {
                try
                {
                    Console.Write("Phone number: ");
                    phoneNumber = Console.ReadLine();

                    if (IsPhoneNumberValid(phoneNumber, true))
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

            Console.Clear();

            WorkTitle workTitle = WorkTitle.Other;

            var workTitleChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Work title[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[]
                    {
                        "Teacher", "Coach", "Other"
                    }));

            Console.Clear();

            if (workTitleChoice == "Teacher")
            {
                workTitle = WorkTitle.Teacher;
            }
            else if (workTitleChoice == "Coach")
            {
                workTitle = WorkTitle.Coach;
            }

            DateTime hiredDate = DateTime.Now;
            bool validHiringDate = false;

            while (!validHiringDate)
            {
                Console.Write("Hire date: ");
                validHiringDate = DateTime.TryParse(Console.ReadLine(), out hiredDate);
                if (validHiringDate)
                {
                    // let validHiringDate be true to break the while-loop
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                }
            }


            DateTime terminationDate = new DateTime(1000, 01, 01);
            bool validTerminationDate = false;

            while (!validTerminationDate)
            {
                Console.Write("Enter termination date (if there is no termination date, just press 'Enter' to skip): ");
                string terminationDateInput = Console.ReadLine();

                if (string.IsNullOrEmpty(terminationDateInput))
                {
                    validTerminationDate = true; // breaks the loop
                }
                else
                {
                    validTerminationDate = DateTime.TryParse(terminationDateInput, out terminationDate);
                    if (validTerminationDate)
                    {
                        if (terminationDate < hiredDate)
                        {
                            Console.WriteLine("Termination date can not be earlier than hiring date.");
                            terminationDate = new DateTime(1000, 01, 01);
                            validTerminationDate = false;
                        }
                        else
                        {
                            // let validHiringDate be true to break the while-loop
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    }
                }
            }


            Cabin[] cabins = Cabin.GetAllFromDb();

            Console.WriteLine();
            Console.WriteLine("Cabins: ");

            foreach (Cabin cabin in cabins)
            {
                Console.WriteLine(cabin.Id + " " + cabin.CabinName);
            }
            Console.WriteLine();

            Console.Write("Enter the ID for the cabin to associate this counselor with (press Enter to skip): ");

            int cabinId;

            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    cabinId = 0; // Sets a special value to indicate that we don't assign the counselor to a cabin
                    break;
                }

                // Parse the input as an integer
                if (!int.TryParse(input, out cabinId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for cabin ID (or press Enter to skip).");
                    Console.Write("Enter the ID for the cabin to associate this counselor with (press Enter to skip): ");
                    continue;
                }

                bool cabinDoesExist = false;

                foreach (Cabin cabin in cabins)
                {
                    if (cabin.Id == cabinId)
                    {
                        cabinDoesExist = true;
                        break;
                    }
                }

                if (!cabinDoesExist)
                {
                    Console.WriteLine("This cabin does not exist!");
                    Console.Write("Enter the ID for the cabin to associate this counselor with (press Enter to skip): ");
                }
                else if (Camper.GetCabinFromCabinId(cabinId).CounselorId != null)
                {
                    var overrideCounselorChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("[red]This cabin already has a counselor. Do you want to override the current counselor?[/]")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                            .AddChoices(new[]
                            {
                                "No", "Yes"
                            }));

                    if (overrideCounselorChoice == "Yes")
                    {
                        Counselor previousCounselor = Cabin.GetCounselorFromCabinId(cabinId);
                        previousCounselor.CabinId = null;
                        previousCounselor.UpdateRecordInDb();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Returning...");
                    }
                }
                else
                {
                    break;
                }
            }

            counselor = new Counselor
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                WorkTitle = workTitle,
                HiredDate = hiredDate,
                CabinId = cabinId
            };

            // termination date handling
            if (terminationDate != new DateTime(1000, 01, 01))
            {
                counselor.TerminationDate = terminationDate;
            }
            else
            {
                counselor.TerminationDate = null;
            }

            return counselor;
        }

        public static Cabin UpdateCabinWithCounselorId(int? cabinId, Counselor counselor)
        {
            // When a councelor gets updated, also update their assigned cabin with their id
            using (var counselorContext = new CampContext())
            {
                Cabin cabin = counselorContext.Cabins.Where(c => c.Id == cabinId).FirstOrDefault();

                cabin.CounselorId = counselor.Id;
                cabin.Counselor = counselor;

                return cabin;
            }
        }

        public static Counselor ChooseCounselorMenu()
        {
            using (var counselorContext = new CampContext())
            {
                List<Counselor> counselors = counselorContext.Counselors.ToList();

                Console.WriteLine("ID | Full Name | Phone-nr. | WorkTitle | Cabin ID | " +
                    "Date Hired | Date Employment was/is Terminated");

                foreach (Counselor counselor in counselors)
                {
                    Console.WriteLine($"{counselor.Id} | " +
                        $"{counselor.FirstName} {counselor.LastName} | {counselor.PhoneNumber} |" +
                        $" {counselor.WorkTitle} | {counselor.CabinId} | " +
                        $"{Helper.FormatDate(counselor.HiredDate)} | {Helper.FormatDate(counselor.TerminationDate)}");
                }

                Console.Write("Enter ID for the 'counselor' you wish to select: ");
                int counselorId;
                while (!int.TryParse(Console.ReadLine(), out counselorId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    Console.Write("Enter ID for the 'counselor' you wish to select: ");
                }

                Counselor selectedCounselor = counselorContext.Counselors.Where(c => c.Id == counselorId).FirstOrDefault();

                return selectedCounselor;
            }
        }

        internal static Counselor EditCounselorMenu(Counselor counselorToEdit)
        {
            Cabin[] cabins = Cabin.GetAllFromDb();

            var editCounselorMenu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]What do you want to do[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(new[]
                    {
                        "Edit first name", "Edit last name", "Edit phone number", "Edit work title",
                        "Edit cabin id", "Edit hire date", "Edit termination date"
                    }));

            if (editCounselorMenu == "Edit first name")
            {
                Console.Write("Enter new first name: ");
                string newFirstName = Console.ReadLine();

                while (true)
                {
                    if (IsLettersOnly(newFirstName))
                    {
                        counselorToEdit.FirstName = newFirstName;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letters.");
                        Console.Write("First name: ");
                    }
                }
            }
            else if (editCounselorMenu == "Edit last name")
            {
                Console.Write("Enter new last name: ");
                string newLastName = Console.ReadLine();

                while (true)
                {
                    if (IsLettersOnly(newLastName))
                    {
                        counselorToEdit.LastName = newLastName;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a name with only letters.");
                        Console.Write("Last name: ");
                    }
                }
            }
            else if (editCounselorMenu == "Edit phone number")
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
                            counselorToEdit.PhoneNumber = newPhoneNumber;
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
            else if (editCounselorMenu == "Edit work title")
            {
                WorkTitle workTitle = WorkTitle.Other;
                var workTitleChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]Work title[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                        .AddChoices(new[]
                        {
                    "Teacher", "Coach", "Other"
                        }));

                if (workTitleChoice == "Teacher")
                {
                    workTitle = WorkTitle.Teacher;
                }
                else if (workTitleChoice == "Coach")
                {
                    workTitle = WorkTitle.Coach;
                }

                counselorToEdit.WorkTitle = workTitle;
            }

            else if (editCounselorMenu == "Edit cabin id")
            {
                Console.Write("Enter new cabin id (press Enter to skip): ");
                string cabinIdInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(cabinIdInput))
                {
                    // User chose to skip cabin association
                    counselorToEdit.CabinId = null;
                }
                else
                {
                    int newCabinId;

                    while (!int.TryParse(cabinIdInput, out newCabinId))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid integer for cabin ID (or press Enter to skip).");
                        Console.Write("Enter new cabin id (press Enter to skip): ");
                        cabinIdInput = Console.ReadLine();
                    }

                    bool cabinExists = cabins.Any(c => c.Id == newCabinId);

                    if (!cabinExists)
                    {
                        Console.WriteLine("This cabin does not exist!");
                    }
                    else if (cabinExists && Camper.GetCabinFromCabinId(newCabinId).CounselorId != null)
                    {
                        var overrideCounselorChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("[red]This cabin already has a counselor. Do you want to override the current counselor?[/]")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                            .AddChoices(new[]
                            {
                                "No", "Yes"
                            }));

                        if (overrideCounselorChoice == "Yes")
                        {
                            Counselor previousCounselor = Cabin.GetCounselorFromCabinId(newCabinId);
                            previousCounselor.CabinId = null;
                            previousCounselor.UpdateRecordInDb();

                            Cabin previousCabin = Camper.GetCabinFromCabinId(counselorToEdit.CabinId);
                            previousCabin.CounselorId = null;
                            previousCabin.UpdateRecordInDb();

                            counselorToEdit.CabinId = newCabinId;
                            Console.WriteLine($"Cabin ID updated to: {newCabinId}");
                        }
                        else
                        {
                            Console.WriteLine("Returning...");
                        }
                    }
                    else if (counselorToEdit.CabinId != null)
                    {
                        // Update the cabin ID
                        Cabin previousCabin = Camper.GetCabinFromCabinId(counselorToEdit.CabinId);
                        previousCabin.CounselorId = null;
                        previousCabin.UpdateRecordInDb();

                        counselorToEdit.CabinId = newCabinId;
                    }
                    else
                    {
                        counselorToEdit.CabinId = newCabinId;

                        Console.WriteLine($"Cabin ID updated to: {newCabinId}");
                    }
                }
            }
            else if (editCounselorMenu == "Edit hire date")
            {
                DateTime hiredDate;
                bool validDate = false;

                while (!validDate)
                {
                    Console.Write("Hire date: ");
                    validDate = DateTime.TryParse(Console.ReadLine(), out hiredDate);
                    if (validDate)
                    {
                        if (hiredDate > counselorToEdit.TerminationDate)
                        {
                            Console.WriteLine("Hiring date can not be after termination date.");
                            validDate = false;
                        }
                        else
                        {
                            counselorToEdit.HiredDate = hiredDate;
                            // let validHiringDate be true to break the while-loop
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    }
                }
            }

            else if (editCounselorMenu == "Edit termination date")
            {
                DateTime terminationDate;
                bool validDate = false;

                while (!validDate)
                {
                    Console.Write("Termination date: ");
                    validDate = DateTime.TryParse(Console.ReadLine(), out terminationDate);
                    if (validDate)
                    {
                        if (terminationDate < counselorToEdit.HiredDate)
                        {
                            Console.WriteLine("Termination date can not be earlier than hiring date.");
                            validDate = false;
                        }
                        else
                        {
                            counselorToEdit.TerminationDate = terminationDate;
                            // let validHiringDate be true to break the while-loop
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    }
                }
            }

            return counselorToEdit;
        }

        public static Counselor[] GetAllFromDb()
        {
            var result = new List<Counselor>();
            using (var context = new CampContext())
            {
                result = context.Counselors.ToList();
            }

            return result.ToArray();
        }

        public void SaveToDb()
        {
            using (var counselorContext = new CampContext())
            {
                counselorContext.Counselors.Add(this);
                counselorContext.SaveChanges();
            }
        }

        public void UpdateRecordInDb()
        {
            using (var counselorContext = new CampContext())
            {
                counselorContext.Update(this);
                counselorContext.SaveChanges();
            }
        }

        public void DeleteFromDb()
        {
            using (var counselorContext = new CampContext())
            {
                counselorContext.Counselors.Remove(this);
                counselorContext.SaveChanges();
            }
        }
    }
}
