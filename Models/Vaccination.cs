using System.ComponentModel.DataAnnotations;
namespace hadsimnew.Models
{
    public class Vaccination
    {
        [Key]
        public int id { get; set; }
        public DateTime? DateVaccination { get; set; }
        public string? manufacturer { get; set; }
        public int MemberId { get; set; }

    }
}
