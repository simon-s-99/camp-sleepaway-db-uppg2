using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camp_sleepaway
{
    public class Cabin
    {
        public required string CabinName { get; set; }
        public Counselor ?AssignedCounselorId { get; set; }

        private static readonly List<string> AvailableCabinNames = new List<string>
        {
            "Lion Hearts",
            "Power Rangers",
            "Lone wolves",
            "Forest Retreat",
            "Lake Bungalow",
            "Destiny's End"
        };

        // Keeps track of the assigned cabin names
        private static readonly HashSet<string> AssignedCabinNames = new HashSet<string>();


        public Cabin()
        {
            //Generate a random cabin name and assign it to CabinName
            CabinName = GetRandomCabinName(); 
            AssignedCabinNames.Add(CabinName);
        }

        private string GetRandomCabinName()
        {
            if (AvailableCabinNames.Count == 0)
            {
                throw new ArgumentException("There is no available cabin name, dude");
            }

            Random random = new Random();
            string selectedName;

            // Here we try to find a unique name for the cabin
            do
            {
                int randomIndex = random.Next(AvailableCabinNames.Count);
                selectedName = AvailableCabinNames[randomIndex];
            } while (!IsNameUnique(selectedName));

            // If a name gets taken it gets removed from the list of available names
            AvailableCabinNames.Remove(selectedName);

            return selectedName;
        }

        private bool IsNameUnique(string name)
        {
            // Check if the name is not already assigned to another cabin
            return !AssignedCabinNames.Contains(name);
        }
    }
}
