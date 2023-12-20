using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camp_sleepaway.helper_classes
{
    public static class DateManager
    {
        public static int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;

            //Calculate the age of the person
            int age = today.Year - birthDate.Year;

            // If the birthdate hasn't occured yet this year we subtract 1 year from the age
            if (birthDate.Date > today.AddYears(age)) age--;

            return age;
        }
    }
}
