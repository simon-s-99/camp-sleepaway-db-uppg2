using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camp_sleepaway
{
    public class Camper : Person
    {
        public required Id { get; set; } 
        public required DateTime BirthDate { get; set; }
        public required DateTime JoinedDate { get; set; }
        public DateTime? LeaveDate { get; set; }

    }
}
