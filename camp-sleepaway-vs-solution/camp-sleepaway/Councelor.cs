using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camp_sleepaway
{
    public enum WorkTitle
    {
        Teacher, Parent, Coach, Other
    }

    public class Councelor
    {
        public int CouncelorID { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required WorkTitle WorkTitle { get; set; }
        public required DateTime JoinDate { get; set; }
        public required DateTime LeaveDate { get; set; }

    }
}
