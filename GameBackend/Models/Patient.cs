using System.ComponentModel.DataAnnotations;

namespace GameBackend.Models
{
    public class Patient
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? UserId { get; set; }
    }
}
