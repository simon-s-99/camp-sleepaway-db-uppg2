using Microsoft.VisualStudio.TestTools.UnitTesting;

// Adds example data to database, primarily used for testing 

// Samuel Lööf, Simon Sörqvist, Adam Kumlin

namespace camp_sleepaway.test_data_for_tables
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
            result = AddCabins();
            result = AddCampers();
            result = AddNextOfKin();

            // Below logic simply does not work, the idea was to remove data if not all of the data
            // was added but this does not work as intended. 
            /*
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
            }*/

            return result;
        }

        /// <summary>
        /// Retrieves data from filepath and returns it with all double quotation
        /// marks removed (") and all trailing/leading spaces trimmed off around each word.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        internal static string[] GetFormattedData(string filepath)
        {
            string[] lines = File.ReadAllLines(filepath);
            return GetFormattedData(lines);
        }
        // tests are done on this overload (string[])
        internal static string[] GetFormattedData(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string formattedLine = lines[i].Replace("\"", "");
                string[] splitLine = formattedLine.Split(',');

                for (int j = 0; j < splitLine.Length; j++)
                {
                    splitLine[j] = splitLine[j].Trim();
                }

                string finalFormatLine = string.Join(",", splitLine);
                lines[i] = finalFormatLine;
            }

            return lines;
        }

        // generates cabins with no assigned counselors 
        private static bool AddCabins()
        {
            string dir = _dir + "Cabin_Example_Data.csv";

            try
            {
                string[] lines = GetFormattedData(dir);

                foreach (string line in lines)
                {
                    string[] l = line.Split(',');

                    string cabinName = l[0];
                    int counselorId = int.Parse(l[1]);

                    var cabin = new Cabin
                    {
                        CabinName = cabinName,
                        CounselorId = counselorId
                    };

                    cabin.SaveToDb();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool AddCounselors()
        {
            string dir = _dir + "Counselor_Example_Data.csv";

            try
            {
                string[] lines = GetFormattedData(dir);

                foreach (string line in lines)
                {
                    string[] l = line.Split(',');

                    string firstName = l[0];
                    string lastName = l[1];
                    string phoneNumber = l[2];
                    WorkTitle workTitle = Enum.Parse<WorkTitle>(l[3]);
                    DateTime dateTime = DateTime.Parse(l[4]);
                    int cabinId = int.Parse(l[5]);

                    var counselor = new Counselor
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        WorkTitle = workTitle,
                        HiredDate = dateTime,
                        TerminationDate = null,
                        CabinId = cabinId
                    };

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
                string[] lines = GetFormattedData(dir);

                foreach (string line in lines)
                {
                    string[] l = line.Split(',');

                    string firstName = l[0];
                    string lastName = l[1];
                    string phoneNumber = l[2];
                    DateTime dateOfBirth = DateTime.Parse(l[3]);
                    DateTime joinDate = DateTime.Parse(l[4]);
                    int cabinId = int.Parse(l[6]); // the integers in the example data ended up at index 6
                    DateTime leaveDate = DateTime.Parse(l[5]);

                    var camper = new Camper
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        DateOfBirth = dateOfBirth,
                        JoinDate = joinDate,
                        LeaveDate = leaveDate,
                        CabinId = cabinId
                    };

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
                string[] lines = GetFormattedData(dir);

                foreach (string line in lines)
                {
                    string[] l = line.Split(',');

                    string firstName = l[0];
                    string lastName = l[1];
                    string phoneNumber = l[2];
                    int relatedToCamper = int.Parse(l[3]);
                    string relationType = l[4];

                    NextOfKin nextOfKin = new NextOfKin
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        CamperId = relatedToCamper,
                        RelationType = relationType
                    };

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

    [TestClass]
    public class UnitTestsGetFormattedData
    {
        [TestMethod]
        public void HappyPath()
        {
            string[] input =
            {
                "  \"hi\"  ,    there,hello, test   , input\"",
                "Normally, formatted, line, with, nothing, weird, going, on,,",
                "\"\"\", Weird, line           , \"\"\""
            };

            string[] expectedOutput =
            {
                "hi,there,hello,test,input",
                "Normally,formatted,line,with,nothing,weird,going,on,,",
                ",Weird,line,"
            };

            string[] actualResult = AddExampleDataToDb.GetFormattedData(input);

            CollectionAssert.AreEqual(expectedOutput, actualResult);
        }
    }
}
