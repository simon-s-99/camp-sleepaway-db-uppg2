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

    public class Counselor : Person
    {
        public int CounselorID { get; set; }
        public required WorkTitle WorkTitle { get; set; }
        public required DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }

    }
}
