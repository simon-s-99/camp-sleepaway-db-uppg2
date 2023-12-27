using camp_sleepaway.ef_table_classes;
using camp_sleepaway.helper_classes;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static camp_sleepaway.Helper;

// Represents Camper table in Entity Framework

namespace camp_sleepaway
{

    public class Camper : Person
    {
        [Key]
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
        public int CabinId { get; set; }
        // Reference navigation to Cabin
        public Cabin Cabin { get; set; } = null!;

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

            Console.Write("First name: ");
            string firstName;
            while (true)
            {
                firstName = Console.ReadLine();
                if (NameCheck.IsLettersOnly(firstName))
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
                if (NameCheck.IsLettersOnly(lastName))
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

                    if (IsPhoneNumberValid.IsPhoneNumber(phoneNumber, false))
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

            Console.Write("Birth date: ");
            DateTime dateOfBirth;

            while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth) || DateManager.CalculateAge(dateOfBirth) < 7
                || DateManager.CalculateAge(dateOfBirth) > 17)
            {
                if (DateManager.CalculateAge(dateOfBirth) < 7)
                {
                    Console.WriteLine("The camper must be at least 7 years old.");
                }
                else if (DateManager.CalculateAge(dateOfBirth) > 17)
                {
                    Console.WriteLine("The camper cannot be older than 17 years old");
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                }
                Console.Write("Birth date: ");
            }

            Console.Write("Join date: ");
            DateTime joinDate;

            //Try parsing the date from the console into a DateTime object, and checks if the join date
            //is at least 7 years after the campers birth date. if true, the camper cannot join before the age of 7
            while (!DateTime.TryParse(Console.ReadLine(), out joinDate) || joinDate < dateOfBirth.AddYears(7) || joinDate > DateTime.Now)
            {
                if (joinDate < dateOfBirth.AddYears(7))
                {
                    Console.WriteLine("The camper cannot join before the age of 7.");
                }
                else if (joinDate > DateTime.Now)
                {
                    Console.WriteLine("Join date cannot be in the future.");
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please enter a valid date.");
                }

                Console.Write("Join date: ");
            }

            Console.Write("Enter leave date (if there is no leave date, just press 'Enter' to skip): ");
            DateTime? leaveDate = null;

            string leaveDateInput = Console.ReadLine();
            DateTime parsedLeaveDate;

            //Check so that the input is not empty
            if (!string.IsNullOrEmpty(leaveDateInput))
            {
                // Looop until the user enters a valid date
                while (!DateTime.TryParse(leaveDateInput, out parsedLeaveDate) || parsedLeaveDate <= joinDate)
                {
                    //Checking if the leave date is before or athe same day to the join date
                    if (parsedLeaveDate <= joinDate)
                    {
                        Console.WriteLine("Leave date must be set after the joined date");
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-MM-dd' or 'Enter' to skip.");
                    }
                    Console.Write("Leave date: ");
                    leaveDateInput = Console.ReadLine();
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
                else
                {
                    break;
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Your camper has been added successfully.");

            Camper camperData = new Camper
            {
                FirstName = firstName,
                LastName = lastName,
                PhoneNumber = phoneNumber,
                DateOfBirth = dateOfBirth,
                JoinDate = joinDate,
                LeaveDate = leaveDate,
                CabinId = cabinId
            };

            return camperData;
        }

        public static Camper ChooseCamperToEdit()
        {
            using (var camperContext = new CampContext())
            {
                List<Camper> campers = camperContext.Campers.ToList();

                foreach (Camper camper in campers)
                {
                    Console.WriteLine($"{camper.Id} - {camper.FirstName} {camper.LastName} - {camper.PhoneNumber}");
                }

                Console.Write("Enter ID for the 'camper' you wish to edit: ");
                int camperId;
                while (!int.TryParse(Console.ReadLine(), out camperId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    Console.Write("Enter ID for the 'camper' you wish to edit: ");
                }

                Camper selectedCamper = camperContext.Campers.FirstOrDefault(c => c.Id == camperId);

                return selectedCamper;
            }
        }


        internal static Camper EditCamperMenu(Camper camperToEdit)
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
                    if (NameCheck.IsLettersOnly(newFirstName))
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
                    if (NameCheck.IsLettersOnly(newLastName))
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
                string phoneNumber;
                while (true)
                {
                    try
                    {
                        Console.Write("Phone number: ");
                        phoneNumber = Console.ReadLine();

                        if (IsPhoneNumberValid.IsPhoneNumber(phoneNumber, false))
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
            else if (editCamperMenu == "Edit birth date")
            {
                Console.Write("Birth date: ");
                DateTime dateOfBirth;

                while (!DateTime.TryParse(Console.ReadLine(), out dateOfBirth) || DateManager.CalculateAge(dateOfBirth) < 7
                    || DateManager.CalculateAge(dateOfBirth) > 17)
                {
                    if (DateManager.CalculateAge(dateOfBirth) < 7)
                    {
                        Console.WriteLine("The camper must be at least 7 years old.");
                    }
                    else if (DateManager.CalculateAge(dateOfBirth) > 17)
                    {
                        Console.WriteLine("The camper cannot be older than 17 years old");
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-mm-dd'");
                    }
                    Console.Write("Birth date: ");
                }
            }
            else if (editCamperMenu == "Edit joined date")
            {
                Console.Write("Join date: ");
                DateTime joinDate;

                //Try parsing the date from the console into a DateTime object, and checks if the join date
                //is at least 7 years after the campers birth date. if true, the camper cannot join before the age of 7
                while (!DateTime.TryParse(Console.ReadLine(), out joinDate) || joinDate < camperToEdit.DateOfBirth.AddYears(7) || joinDate > DateTime.Now)
                {
                    if (joinDate < camperToEdit.DateOfBirth.AddYears(7))
                    {
                        Console.WriteLine("The camper cannot join before the age of 7.");
                    }
                    else if (joinDate > DateTime.Now)
                    {
                        Console.WriteLine("Join date cannot be in the future.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please enter a valid date.");
                    }

                    Console.Write("Join date: ");
                }
            }
            else if (editCamperMenu == "Edit leave date")
            {
                Console.Write("Enter new leave date (if there is no leave date, just press 'Enter' to skip): ");
                DateTime? leaveDate = null;

                string leaveDateInput = Console.ReadLine();
                DateTime parsedLeaveDate;

                //Check so that the input is not empty
                if (!string.IsNullOrEmpty(leaveDateInput))
                {
                    // Looop until the user enters a valid date
                    while (!DateTime.TryParse(leaveDateInput, out parsedLeaveDate) || parsedLeaveDate <= camperToEdit.JoinDate)
                    {
                        //Checking if the leave date is before or athe same day to the join date
                        if (parsedLeaveDate <= camperToEdit.JoinDate)
                        {
                            Console.WriteLine("Leave date must be set after the joined date");
                        }
                        else
                        {
                            Console.WriteLine("Invalid date format. Please enter date in this format: 'yyyy-MM-dd' or 'Enter' to skip.");
                        }
                        Console.Write("Leave date: ");
                        leaveDateInput = Console.ReadLine();
                    }
                }
            }
            return camperToEdit;
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

                    Cabin resultCabin = GetCabinFromCabinId(result.CabinId);
                    Console.WriteLine("Cabin: " + resultCabin.Id + " " + resultCabin.CabinName);

                    Counselor resultCounselor = GetCounselorFromCabinId(result.CabinId);
                    Console.WriteLine(resultCounselor != null ? "Cabin counselor: " + resultCounselor.FirstName + " " + resultCounselor.LastName : "Warning! This cabin has no active counselor!");
                    // If counselor is not null then print out normally, if it is null then warn the user

                    Console.WriteLine();
                }
            }
        }
        public static void DisplayCampersAndNextOfKins()
        {
            using (var camperContext = new CampContext())
            {
                List<Camper> campers = camperContext.Campers.OrderBy(c => c.Cabin.Id).ToList();
                // Get every camper, and order them by CabinId

                foreach (Camper camper in campers)
                {
                    Console.WriteLine("Id: " + camper.Id);
                    Console.WriteLine("Full name: " + camper.FirstName + " " + camper.LastName);
                    Console.WriteLine("Phone number: " + camper.PhoneNumber);
                    Console.WriteLine("Birth date: " + camper.DateOfBirth);
                    Console.WriteLine("Date joined: " + camper.JoinDate);
                    Console.WriteLine("Date left/date to leave: " + camper.LeaveDate);

                    Cabin resultCabin = GetCabinFromCabinId(camper.CabinId);
                    Console.WriteLine("Cabin: " + resultCabin.Id + " " + resultCabin.CabinName);

                    if (camper.NextOfKins.Count != 0)
                    {
                        Console.WriteLine("NextOfKins: ");
                        foreach (NextOfKin nextOfKin in camper.NextOfKins)
                        {
                            Console.WriteLine(nextOfKin.FirstName + " " + nextOfKin.LastName + " - " + nextOfKin.RelationType);
                        }
                        // Print each NextOfKin, for each camper
                    }
                    else
                    {
                        Console.WriteLine("This camper has no NextOfKins.");
                    }

                    Console.WriteLine();
                }
            }
        }

        public static Cabin GetCabinFromCabinId(int cabinId)
        {
            using (var camperContext = new CampContext())
            {
                Cabin cabin = camperContext.Cabins.Where(c => c.Id == cabinId).FirstOrDefault();

                return cabin;
            }
        }

        private static Counselor GetCounselorFromCabinId(int cabinId)
        {
            using (var camperContext = new CampContext())
            {
                Counselor counselor = camperContext.Counselors.Where(c => c.Cabin.Id == cabinId).FirstOrDefault();

                return counselor;
            }
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
