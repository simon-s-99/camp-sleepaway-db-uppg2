using camp_sleepaway.ef_table_classes;
using Newtonsoft.Json.Linq;

// Adds example data to database, primarily used for testing 

namespace camp_sleepaway
{
    internal class AddExampleDataToDb
    {
        private static readonly string _dir = 
            Directory.GetCurrentDirectory() + "\\test_data_for_tables\\";

        // Adds all exampledata, returns true if successful, false if any of the methods failed
        internal static bool AddAllData()
        {
            bool result = false;

            result = AddCounselors();
            result = AddCabins(25);
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

        // add method for readin all lines from dir here, the implementation
        // looks the same in all Add... methods below 

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
            string dir = _dir + "Counselor_Example_Data.csv";

            try
            {
                string[] lines = File.ReadAllLines(dir);

                foreach (string line in lines)
                {
                    string formattedLine = line.Replace("\"", "");
                    string[] l = formattedLine.Split(',');

                    string firstName = l[0];
                    string lastName = l[1];
                    string phoneNumber = l[2];
                    WorkTitle workTitle = Enum.Parse<WorkTitle>(l[3]);
                    DateTime dateTime = DateTime.Parse(l[4]);

                    var counselor = new Counselor(firstName, lastName, phoneNumber, workTitle, dateTime, null, null);

                    counselor.SaveToDb();
                }

                return true;
            }
            catch 
            { 
                return false;
            }
        }

        private static bool AddCampers()
        {
            string dir = _dir + "Camper_Example_Data.csv";

            try
            {
                string[] lines = File.ReadAllLines(dir);

                foreach (string line in lines)
                {
                    string formattedLine = line.Replace("\"", "");
                    string[] l = formattedLine.Split(',');

                    string firstName = l[0];
                    string lastName = l[1];
                    string phoneNumber = l[2];
                    DateTime dateOfBirth = DateTime.Parse(l[3]);
                    DateTime joinDate = DateTime.Parse(l[4]);
                    DateTime leaveDate = DateTime.Parse(l[5]);

                    var camper = new Camper(firstName, lastName, phoneNumber, dateOfBirth, joinDate, leaveDate);

                    camper.SaveToDb();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        
        private static bool AddNextOfKin()
        {
            string dir = _dir + "NextOfKin_Example_Data.csv";

            try
            {
                string[] lines = File.ReadAllLines(dir);

                foreach (string line in lines)
                {
                    string formattedLine = line.Replace("\"", "");
                    string[] l = formattedLine.Split(',');

                    string firstName = l[0];
                    string lastName = l[1];
                    string phoneNumber = l[2];
                    int relatedToCamper = int.Parse(l[3]);
                    string relationType = l[4];

                    var nextOfKin = new NextOfKin(firstName, lastName, phoneNumber, relatedToCamper, relationType);

                    nextOfKin.SaveToDb();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
