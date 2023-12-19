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
        public required WorkTitle WorkTitle { get; set; }
        public required DateTime HiredDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        // Reference navigation to Cabin
        public Cabin? Cabin { get; set; }

        [SetsRequiredMembers]
        public Counselor(string firstName, string lastName, string phoneNumber,
            WorkTitle workTitle, DateTime hiredDate, DateTime? terminationDate = null)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            WorkTitle = workTitle;
            HiredDate = hiredDate;
            TerminationDate = terminationDate;
        }

        // Asks user for input via console, should primarily be called from main menu 
        public static Counselor InputCounselorData()
        {
            Console.Clear();
            Console.WriteLine("Add counselor");
            Console.WriteLine();

            Console.Write("First name: ");
            string firstName = Console.ReadLine();

            Console.Write("Last name: ");
            string lastName = Console.ReadLine();

            Console.Write("Phone number: ");
            string phoneNumber = Console.ReadLine();

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
                WorkTitle = WorkTitle.Teacher;
            }
            else if (workTitleChoice == "Parent")
            {
                WorkTitle = WorkTitle.Parent;
            }
            else if (workTitleChoice == "Coach")
            {
                WorkTitle = WorkTitle.Coach;
            }

            Console.Write("Join date: ");
            DateTime hiredDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Leave date: ");
            DateTime terminationDate = DateTime.Parse(Console.ReadLine());

            Counselor counselor = new Counselor(firstName, lastName, phoneNumber, workTitle, hiredDate, terminationDate);

            return counselor;
        }

        public Counselor ChooseCounselorToEdit()
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

        public void SaveToDb()
        {
            using (var counselorContext = new CampContext())
            {
                counselorContext.Counselors.Add(this);
                counselorContext.SaveChanges();
            }
        }
    }
}
