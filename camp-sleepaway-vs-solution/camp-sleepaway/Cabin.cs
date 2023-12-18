using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace camp_sleepaway
{
    public class Cabin
    {
        public required int Id { get; set; }
        public required string CabinName { get; set; }
        public Counselor? CounselorId { get; set; }
    }
}
