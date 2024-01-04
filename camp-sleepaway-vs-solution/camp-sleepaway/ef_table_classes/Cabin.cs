using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Represents Cabin table in Entity Framework

// Samuel Lööf, Simon Sörqvist, Adam Kumlin

namespace camp_sleepaway
{
    public class Cabin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Invalid cabin name.")]
        [StringLength(100)]
        public string CabinName { get; set; }

        // Foreign key property to Counselor
        [ForeignKey("CounselorId")]
        public int? CounselorId { get; set; }
        // Reference navigation to Counselor
        public Counselor? Counselor { get; set; }

        // Collection reference to Camper
        public ICollection<Camper> Campers { get; set; } = new List<Camper>();

        // empty constructor for Entity Framework
        public Cabin()
        {
        }

        // used hashset to guarantee unique names 
        private static readonly HashSet<string> AssignedCabinNames = new HashSet<string>();

        // Method to check if name is unique
        private static bool IsNameUnique(string attribute, string noun)
        {
            return !AssignedCabinNames.Contains($"{attribute} {noun}");
        }

        // Method that generates a random cabin name. Could always add more names if needed.
        public static string GenerateRandomCabinName()
        {
            // Current method (GenerateRandomCabinName()) supports ~441 uniquely generated names
            // Changing the do-while loop to handle the names running out, i.e. returning another value
            // (maybe returning false if unique names run out) or adding more names would solve this. 
            string[] namesPrefix =
            {
                "Lion", "Power", "Lone",
                "Forest", "Lake", "Destiny",
                "Majestic", "Eternal", "Golden",
                "Whispering", "Enchanted", "Radiant",
                "Sapphire", "Crimson", "Emerald",
                "Celestial", "Tranquil", "Harmonious",
                "Blazing", "Mystic", "Epic"
            };

            string[] namesSuffix =
            {
                "Hearts", "Rangers", "Wolves",
                "Retreat", "Bungalow", "End",
                "Harmony", "Oasis", "Serenity",
                "Haven", "Zenith", "Eclipse",
                "Infinity", "Horizon", "Spectra",
                "Cascades", "Aurora", "Pinnacle",
                "Quasar", "Nebula", "Solstice"
            };

            Random random = new Random();

            string prefix = string.Empty;
            string suffix = string.Empty;

            do
            {
                int prefixIndex = random.Next(namesPrefix.Length);
                int suffixIndex = random.Next(namesSuffix.Length);

                prefix = namesPrefix[prefixIndex];
                suffix = namesSuffix[suffixIndex];

            } while (!IsNameUnique(prefix, suffix));

            // If a name gets taken it gets added to the list and cannot be used again.
            AssignedCabinNames.Add($"{prefix} {suffix}");

            return $"{prefix} {suffix}";
        }

        // Asks user for input via console, should primarily be called from main menu 
        public static Cabin InputCabinData()
        {
            Console.Clear();
            Console.WriteLine("Add cabin");
            Console.WriteLine();

            // user can choose to manually enter cabin name or get a generated cabin name
            string[] cabinNameChoiceOptions = { "Input cabin name manually", "Generate cabin name for me" };
            string? cabinNameChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[red]Do you want the program to generate a cabin name or do you " +
                    "wish to input this manually?[/]")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                    .AddChoices(cabinNameChoiceOptions));

            string cabinName = string.Empty;
            if (cabinNameChoice == cabinNameChoiceOptions[0])
            {
                Console.Write("Cabin name: ");
                cabinName = Console.ReadLine();
            }
            else
            {
                cabinName = GenerateRandomCabinName();
            }

            Cabin cabin = new Cabin
            {
                CabinName = cabinName,
                CounselorId = null
            };

            return cabin;
        }

        public static Counselor UpdateCounselorWithCabinId(int? counselorId, Cabin cabin)
        {
            using (var cabinContext = new CampContext())
            {
                Counselor cabinCounselor = cabinContext.Counselors.Where(c => c.Id == counselorId).FirstOrDefault();

                cabinCounselor.CabinId = cabin.Id;
                cabinCounselor.Cabin = cabin;

                return cabinCounselor;
            }
        }

        public static Cabin ChooseCabinMenu()
        {
            using (var cabinContext = new CampContext())
            {
                List<Cabin> cabins = cabinContext.Cabins.ToList();

                Console.WriteLine("ID | Cabin Name | Counselor ID");

                foreach (Cabin cabin in cabins)
                {
                    Console.WriteLine($"{cabin.Id} | {cabin.CabinName} | {cabin.CounselorId}");
                }

                Console.Write("Enter ID for the 'cabin' you wish to select: ");
                int cabinId;
                while (!int.TryParse(Console.ReadLine(), out cabinId))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    Console.Write("Enter ID for the 'cabin' you wish to select: ");
                }

                Cabin selectedCabin = cabinContext.Cabins.FirstOrDefault(c => c.Id == cabinId);

                return selectedCabin;
            }
        }

        internal static Cabin EditCabinMenu(Cabin cabinToEdit)
        {
            var editCabinMenu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[red]What do you want to do[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                .AddChoices(new[] {
                    "Edit cabin name", "Go back"
                }));

            if (editCabinMenu == "Edit cabin name")
            {
                string[] cabinNameChoiceOptions = { "Input cabin name manually", "Generate cabin name for me" };
                string? cabinNameChoice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[red]Do you want the program to generate a cabin name or do you " +
                        "wish to input this manually?[/]")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to select an option)[/]")
                        .AddChoices(cabinNameChoiceOptions));

                string cabinName = string.Empty;
                if (cabinNameChoice == cabinNameChoiceOptions[0])
                {
                    Console.Write("Cabin name: ");
                    cabinName = Console.ReadLine();
                }
                else
                {
                    cabinName = GenerateRandomCabinName();
                }
                cabinToEdit.CabinName = cabinName;
            }
            else // go back 
            {
            }
            return cabinToEdit;
        }

        public static Counselor GetCounselorFromCabinId(int? cabinId)
        {
            using (var cabinContext = new CampContext())
            {
                Counselor counselor = cabinContext.Counselors.Where(c => c.CabinId == cabinId).FirstOrDefault();

                return counselor;
            }
        }

        public static Camper[] GetCampersFromCabinId(int cabinId)
        {
            using (var cabinContext = new CampContext())
            {
                Camper[] campers = cabinContext.Campers.Where(c => c.CabinId == cabinId).ToArray();

                return campers;
            }
        }

        public static Cabin[] GetAllFromDb()
        {
            var result = new List<Cabin>();
            using (var context = new CampContext())
            {
                result = context.Cabins.ToList();
            }

            return result.ToArray();
        }

        public void SaveToDb()
        {
            using (var cabinContext = new CampContext())
            {
                cabinContext.Cabins.Add(this);
                cabinContext.SaveChanges();
            }
        }

        public void UpdateRecordInDb()
        {
            using (var cabinContext = new CampContext())
            {
                cabinContext.Update(this);
                cabinContext.SaveChanges();
            }
        }

        public void DeleteFromDb()
        {
            using (var cabinContext = new CampContext())
            {
                cabinContext.Cabins.Remove(this);
                cabinContext.SaveChanges();
            }
        }
    }
}

