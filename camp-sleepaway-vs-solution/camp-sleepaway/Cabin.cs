using System.ComponentModel.DataAnnotations;

namespace camp_sleepaway
{
    public class Cabin
    {
        [Key]
        public int Id { get; set; }
        public required string CabinName { get; set; }
        public Counselor? CounselorId { get; set; }
    }
}
