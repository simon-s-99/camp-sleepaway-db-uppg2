using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camp_sleepaway
{
    public class Camper : Person
    {
        public required int CamperId { get; set; } // Using "required" for now
        public required DateTime BirthDate { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime LeaveDate { get; set; }

    }
}
