using camp_sleepaway.ef_table_classes;

// Adds example data to database, primarily used for testing 

namespace camp_sleepaway
{
    internal class AddExampleDataToDb
    {
        // Adds all exampledata, returns true if successful, false if any of the methods failed
        internal static bool AddAllData()
        {
            bool result = false;

            result = AddCabins(25);
            result = AddCounselors();
            result = AddCampers();
            result = AddNextOfKin();

            if (!result) // removes the previously added data if any of the above methods fail (ACID)
            {
                using (var context = new CampContext())
                {
                    context.NextOfKins.RemoveRange(context.NextOfKins);
                    context.Campers.RemoveRange(context.Campers);
                    context.Counselors.RemoveRange(context.Counselors);
                    context.Cabins.RemoveRange(context.Cabins);
                }
                return result;
            }

            return result;
        }

        // generates cabins with no assigned counselors 
        private static bool AddCabins(int nrOfCabins)
        {
            var cabins = new List<Cabin>();
            try
            {
                for (int i = 0; i < nrOfCabins; i++)
                {
                    string cabinName = Cabin.GenerateRandomCabinName();
                    var cabin = new Cabin(cabinName, null);
                    cabin.SaveToDb();
                }
                return true;
            }
            catch
            {
                return false; // return false if any errors occur 
            }
        }

        private static bool AddCounselors()
        {
            string dir = Directory.GetCurrentDirectory();

            // to-do here:
            // check counselor ctor and translate .csv to ctor here 

            return true;
        }

        private static bool AddCampers()
        {
            return true;
        }

        private static bool AddNextOfKin()
        {
            return true;
        }
    }
}