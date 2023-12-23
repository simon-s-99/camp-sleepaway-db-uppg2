using System.Text.RegularExpressions;

namespace camp_sleepaway.helper_classes
{
    //Method to check if a american phone number is valid
    //public static bool IsPhoneNumber(string number)
    //{
    //    return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
    //}

    public class IsPhoneNumberValid
    {

        //Method to check if a phone number with different variations is valid
        public static bool IsPhoneNumber(string number, bool mustBeUnique)
        {
            bool isFormattedCorrectly = Regex.Match(number, @"^(\+\d{1,3}\s?)?(\(\d{1,4}\))?[0-9\- \(\)]{7,15}$").Success;

            if (!isFormattedCorrectly)
            {
                return false;
            }

            if (mustBeUnique)
            {
                using (var personContext = new CampContext())
                {
                    List<Camper> campers = personContext.Campers.ToList();
                    List<Counselor> counselors = personContext.Counselors.ToList();
                    List <NextOfKin> nextOfKins = personContext.NextOfKins.ToList();

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

    }


}
