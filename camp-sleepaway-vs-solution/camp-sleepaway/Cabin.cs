﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace camp_sleepaway
{
    public class Cabin
    {
        private static readonly HashSet<string> AssignedCabinNames = new HashSet<string>();

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

        // Parameterless constructor required by EF Core
        public Cabin() { }

        [SetsRequiredMembers]
        public Cabin(string cabinName, Counselor counselor)
        {
            CabinName = cabinName;
            Counselor = counselor;
        }

        // Asks user for input via console, should primarily be called from the main menu 
        public Cabin InputCabinData()
        {
            Console.Clear();
            Console.WriteLine("Add cabin");
            Console.WriteLine();

            Console.Write("Cabin name: ");
            string cabinName = Console.ReadLine();

            Console.Write("CounselorID: ");
            int counselorID = int.Parse(Console.ReadLine());

            var context = new CampContext();

            Counselor counselor = context.Counselors.Where(c => c.Id == counselorID).FirstOrDefault();

            Cabin cabin = new Cabin(cabinName, counselor);

            return cabin;
        }

        // Method that generates a random cabin name. Could always add more names if needed.
        public static string GenerateRandomCabinName()
        {
            string[] attributes =
            {
                "Lion", "Power", "Lone",
                "Forest", "Lake", "Destiny",
                "Majestic", "Eternal", "Golden",
                "Whispering", "Enchanted", "Radiant",
                "Sapphire", "Crimson", "Emerald",
                "Celestial", "Tranquil", "Harmonious",
                "Blazing", "Mystic", "Epic"
            };

            string[] nouns =
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

            string attribute, noun;
            do
            {
                int attributeIndex = random.Next(attributes.Length);
                int nounIndex = random.Next(nouns.Length);

                attribute = attributes[attributeIndex];
                noun = nouns[nounIndex];

            } while (!IsNameUnique(attribute, noun));

            // If a name gets taken it gets added to the list and cannot be used again.
            AssignedCabinNames.Add($"{attribute} {noun}");

            return $"{attribute} {noun}";
        }

        private static bool IsNameUnique(string attribute, string noun)
        {
            return !AssignedCabinNames.Contains($"{attribute} {noun}");
        }
    }
}

