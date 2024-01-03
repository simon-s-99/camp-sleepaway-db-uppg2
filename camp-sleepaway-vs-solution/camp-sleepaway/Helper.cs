using System.Text.RegularExpressions;

namespace camp_sleepaway
{
    // Samuel Lööf, Simon Sörqvist, Adam Kumlin

    internal class Helper
    {
        public static bool IsLettersOnly(string input) // moved from NameCheck
        {
            // Check if a string contains only letters
            // returns true if the input string contains only english and swedish letters, false otherwise
            return !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, "^[a-zA-ZåäöÅÄÖ]+$");
        }

        internal static int CalculateAge(DateTime birthDate) // moved from DateManager
        {
            DateTime today = DateTime.Today;

            //Calculate the age of the person
            int age = today.Year - birthDate.Year;

            // If the birthdate hasn't occured yet this year we subtract 1 year from the age
            if (birthDate.Date > today.AddYears(age)) age--;

            return age;
        }

        //Method to check if a phone number with different variations is valid
        public static bool IsPhoneNumberValid(string number, bool mustBeUnique) // moved from IsPhoneNumber
        {
            bool isFormattedCorrectly = Regex.Match(number, @"^(\+\d{1,3}\s?)?(\(\d{1,4}\))?[0-9\- \(\)]{7,16}$").Success;

            if (!isFormattedCorrectly)
            {
                return false;
            }

            if (mustBeUnique)
            {
                // The boolean 'mustBeUnique' determines if the input must be unique, if so, the code below will check the input against every other phone number
                // in the database
                using (var personContext = new CampContext())
                {
                    List<Camper> campers = personContext.Campers.ToList();
                    List<Counselor> counselors = personContext.Counselors.ToList();
                    List<NextOfKin> nextOfKins = personContext.NextOfKins.ToList();

                    foreach (Camper camper in campers)
                    {
                        if (camper.PhoneNumber == number)
                        {
                            return false;
                        }
                    }

                    foreach (Counselor counselor in counselors)
                    {
                        if (counselor.PhoneNumber == number)
                        {
                            return false;
                        }
                    }

                    foreach (NextOfKin nextOfKin in nextOfKins)
                    {
                        if (nextOfKin.PhoneNumber == number)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        // returns shortdate (ex: 2010-12-10) 
        public static string FormatDate(DateTime? date)
        {
            string formattedDate = string.Empty;

            if (date != null)
            {
                formattedDate = date.ToString().Substring(0, 10);
                return formattedDate;
            }
            else
            {
                return formattedDate;
            }
        }
    }
}
