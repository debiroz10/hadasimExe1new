using hadsimnew.Models;

namespace hadasimExe1new.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string? adress { get; set; }
        public string? city { get; set; }

        public int numAdress { get; set; }

        public System.DateTime DateOfBirth { get; set; }

        public string? Phone { get; set; }
        public string? mobilePhone { get; set; }
        //public ICollection<Vaccination>? Vaccinations { get; set; }
        public DateTime? DateOfSickness { get; set; }
        public DateTime? DateOfRecovery { get; set; }
        public string? ImageUrl { get; set; } // Variable for image



    }
}
