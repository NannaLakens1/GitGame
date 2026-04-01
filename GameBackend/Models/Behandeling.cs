using System.ComponentModel.DataAnnotations;

namespace GameBackend.Models
{
    public class Behandeling
    {
        public Guid Id { get; set; }
        //[Required]
        public Guid PatientId { get; set; }

        public string? Type { get; set; }
        public string? Arts { get; set; }
        public DateTime? Datum { get; set; }
        public string? Locatie { get; set; }
    }
}
