using camp_sleepaway.ef_table_classes;
using camp_sleepaway.helper_classes;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace camp_sleepaway
{
    public enum WorkTitle
    {
        Teacher, Parent, Coach, Other
    }

    public class Counselor : Person
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Invalid work title."), EnumDataType(typeof(WorkTitle))]
        public WorkTitle WorkTitle { get; set; }

        [Required(ErrorMessage = "Invalid hire date.")]
        public DateTime HiredDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        // Reference navigation to Cabin
        public Cabin? Cabin { get; set; }

        // empty constructor for Entity Framework
        public Counselor()
        {
        }

        [SetsRequiredMembers]
        public Counselor(string firstName, string lastName, string phoneNumber,
            WorkTitle workTitle, DateTime hiredDate, Cabin? cabin = null, DateTime? terminationDate = null)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            WorkTitle = workTitle;
            HiredDate = hiredDate;
            Cabin = cabin;
            TerminationDate = terminationDate;
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

                if (Camper.IsLettersOnly(firstName))
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

                if (Camper.IsLettersOnly(lastName))
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

            Console.Write("Enter cabin id: ");
            var cabinIdString = Console.ReadLine();
            int cabinId;

            while (!int.TryParse(cabinIdString, out cabinId))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer for cabin ID.");
                Console.Write("Enter cabin id: ");
                cabinIdString = Console.ReadLine();
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
                while (!DateTime.TryParse(TerminationDateInput, out parsedTerminationDate) || parsedTerminationDate <= counselor.HiredDate)
                {
                    //Checking if the leave date is before or athe same day to the join date
                    if (parsedTerminationDate <= counselor.HiredDate)
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

            var tempCabin = new Cabin();
            using (var context = new CampContext()) 
            {
                // add code to get cabin from context here 
            }
            counselor = new Counselor(firstName, lastName, phoneNumber, workTitle, hiredDate, tempCabin, terminationDate);

            return counselor;
        }

        public static Counselor ChooseCounselorToEdit()
        {
            using (var counselorContext = new CampContext())
            {
                List<Counselor> counselors = counselorContext.Counselors.ToList();

                foreach (Counselor counselor in counselors)
                {
                    Console.WriteLine(counselor.Id + " | " + counselor.FirstName + " " + counselor.LastName + " | " + counselor.PhoneNumber);
                }

                Console.Write("Enter ID for camper you wish to edit: ");
                int counselorID = int.Parse(Console.ReadLine());

                Counselor selectedCounselor = counselorContext.Counselors.Where(c => c.Id == counselorID).FirstOrDefault();

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
                    if (Camper.IsLettersOnly(newFirstName))
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
                    if (Camper.IsLettersOnly(newLastName))
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
                result = context.Counselors
                    .ToList();
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
