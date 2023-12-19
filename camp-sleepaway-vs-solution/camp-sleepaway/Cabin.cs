using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace camp_sleepaway
{
    public class Cabin
    {
        [Key]
        public int Id { get; set; }
        public required string CabinName { get; set; }

        // Foreign key property to Counselor
        public int? CounselorId { get; set; }
        // Reference navigation to Counselor
        public Counselor? Counselor { get; set; }

        // collection reference to Camper
        public List<Camper> Campers { get; set; } = new();

        [SetsRequiredMembers]
        public Cabin(string cabinName, Counselor counselor)
        {
            CabinName = cabinName;
            Counselor = counselor;
        }

        // Asks user for input via console, should primarily be called from main menu 
        public static Cabin InputCabinData()
        {
            Console.Clear();
            Console.WriteLine("Add cabin");
            Console.WriteLine();

            Console.Write("Cabin name: ");
            string cabinName = Console.ReadLine();

            Console.Write("CounselorID: ");
            int counselorID = int.Parse(Console.ReadLine());

            var context = new CampContext();

            Counselor counselor = context.Counselors.Where(c => c.Id == counselorID).FirstOrDefault();

            Cabin cabin = new Cabin(cabinName, counselor);

            return cabin;
        }

        public void SaveToDb()
        {
            using (var cabinContext = new CampContext())
            {
                cabinContext.Cabins.Add(this);
                cabinContext.SaveChanges();
            }
        }
    }
}
