using camp_sleepaway.ef_table_classes;
using static camp_sleepaway.Helper;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations.Schema;

// Represents Counselor table in Entity Framework

namespace camp_sleepaway
{
    public enum WorkTitle
    {
        Teacher, Parent, Coach, Other
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
                        "Teacher", "Parent", "Coach", "Other"
                    }));

            Console.Clear();

            if (workTitleChoice == "Teacher")
            {
                workTitle = WorkTitle.Teacher;
            }
            else if (workTitleChoice == "Parent")
            {
                workTitle = WorkTitle.Parent;
            }
            else if (workTitleChoice == "Coach")
            {
                workTitle = WorkTitle.Coach;
            }

            Console.Write("Hired date: ");
            DateTime hiredDate;
            while (!DateTime.TryParse(Console.ReadLine(), out hiredDate))
            {
                Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                Console.Write("Join date: ");
            }

            Console.Write("Enter termination date (if there is no termination date, just press 'Enter' to skip): ");
            DateTime? terminationDate = null;

            string TerminationDateInput = Console.ReadLine();
            DateTime parsedTerminationDate;

            //Check so that the input is not empty
            if (!string.IsNullOrEmpty(TerminationDateInput))
            {
                // Looop until the user enters a valid date
                while (!DateTime.TryParse(TerminationDateInput, out parsedTerminationDate) || parsedTerminationDate <= hiredDate)
                {
                    //Checking if the leave date is before or athe same day to the join date
                    if (parsedTerminationDate <= hiredDate)
                    {
                        Console.WriteLine("termination date must be set after the joined date");
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-MM-dd' or 'Enter' to skip.");
                    }
                    Console.Write("Termination date: ");
                    TerminationDateInput = Console.ReadLine();
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

            Console.Write("Enter the ID for the cabin to associate this counselor with: ");

            int cabinId;

            while (true)
            {
                bool cabinDoesExist = false;

                while (!int.TryParse(Console.ReadLine(), out cabinId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for cabin ID.");
                    Console.Write("Enter the ID for the cabin to associate this counselor with: ");
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
                    Console.Write("Enter the ID for the cabin to associate this counselor with: ");
                }
                else if (Camper.GetCabinFromCabinId(cabinId).CounselorId != null)
                {
                    Console.WriteLine("This cabin already has a counselor!");
                    Console.Write("Enter the ID for the cabin to associate this counselor with: ");
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
                CabinId = cabinId,
                TerminationDate = terminationDate
            };

            return counselor;
        }

        public static Cabin UpdateCabinWithCounselorId(int? cabinId, Counselor counselor)
        {
            using (var counselorContext = new CampContext())
            {
                Cabin cabin = counselorContext.Cabins.Where(c => c.Id == cabinId).FirstOrDefault();

                cabin.CounselorId = counselor.Id;

                cabin.Counselor = counselor;

                return cabin;
            }
        }

        public static Counselor ChooseCounselorToEdit()
        {
            using (var counselorContext = new CampContext())
            {
                List<Counselor> counselors = counselorContext.Counselors.ToList();

                foreach (Counselor counselor in counselors)
                {
                    Console.WriteLine($"{counselor.Id} | {counselor.FirstName} {counselor.LastName} | {counselor.PhoneNumber} |" +
                        $" {counselor.WorkTitle} | {counselor.CabinId} | {counselor.HiredDate} | {counselor.TerminationDate}");
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
                    "Teacher", "Parent", "Coach", "Other"
                        }));

                if (workTitleChoice == "Teacher")
                {
                    workTitle = WorkTitle.Teacher;
                }
                else if (workTitleChoice == "Parent")
                {
                    workTitle = WorkTitle.Parent;
                }
                else if (workTitleChoice == "Coach")
                {
                    workTitle = WorkTitle.Coach;
                }

                counselorToEdit.WorkTitle = workTitle;
            }

            else if (editCounselorMenu == "Edit cabin id")
            {
                Console.Write("Enter cabin id: ");
                
            }

            else if (editCounselorMenu == "Edit hire date")
            {
                Console.Write("Hire date: ");
                DateTime hiredDate;
                while (!DateTime.TryParse(Console.ReadLine(), out hiredDate))
                {
                    Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    Console.Write("Hire date: ");
                }
                counselorToEdit.HiredDate = hiredDate;
            }
            else if (editCounselorMenu == "Edit termination date")
            {
                Console.Write("Termination date: ");
                DateTime terminationDate;
                while (!DateTime.TryParse(Console.ReadLine(), out terminationDate))
                {
                    Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    Console.Write("Termination date: ");
                }
                counselorToEdit.TerminationDate = terminationDate;
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
