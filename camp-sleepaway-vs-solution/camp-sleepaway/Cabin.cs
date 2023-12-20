﻿using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace camp_sleepaway
{
    public class Cabin
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Invalid cabin name.")]
        public string CabinName { get; set; }

        // Foreign key property to Counselor
        public int? CounselorId { get; set; }
        // Reference navigation to Counselor
        public Counselor? Counselor { get; set; }

        // Collection reference to Camper
        public List<Camper> Campers { get; set; } = new();

        // empty constructor for Entity Framework
        public Cabin()
        {
        }

        [SetsRequiredMembers]
        public Cabin(string cabinName, Counselor? counselor)
        {
            CabinName = cabinName;
            Counselor = counselor;
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

            Console.Write("CounselorID: ");
            int counselorID = int.Parse(Console.ReadLine());

            var context = new CampContext();

            Counselor counselor = context.Counselors.Where(c => c.Id == counselorID).FirstOrDefault();

            Cabin cabin = new Cabin(cabinName, counselor);

            return cabin;
        }

        public void SaveToDb()
        {
            using (var cabinContext = new CampContext())
            {
                cabinContext.Cabins.Add(this);
                cabinContext.SaveChanges();
            }
        }
      
        // used hashset to guarantee unique names 
        private static readonly HashSet<string> AssignedCabinNames = new HashSet<string>();

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

        private static bool IsNameUnique(string attribute, string noun)
        {
            return !AssignedCabinNames.Contains($"{attribute} {noun}");
        }
    }
}

