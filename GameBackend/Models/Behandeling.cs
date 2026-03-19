using System.ComponentModel.DataAnnotations;

namespace GameBackend.Models
{
    public class Behandeling
    {
        public Guid Id { get; set; }
        //[Required]
        public Guid PatientId { get; set; }

        [Required]
        public string? Type { get; set; }
        [Required]
        public string? Arts { get; set; }
        [Required]
        public DateTime? Datum { get; set; }
        public string? Locatie { get; set; }
    }
}
