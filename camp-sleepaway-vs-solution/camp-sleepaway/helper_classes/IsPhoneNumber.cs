using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+\d{1,3}\s?)?(\(\d{1,4}\))?[0-9\- \(\)]{7,15}$").Success;
        }

    }


}
