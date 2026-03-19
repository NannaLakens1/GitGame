using System.ComponentModel.DataAnnotations;

namespace GameBackend.Models
{
    public class Patient
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 1)]
        public string? Name { get; set; }
        [Range(20, 200)]
        public int? MaxLength { get; set; }
        [Range(10, 100)]
        public int? MaxHeight { get; set; }
        public string? UserId { get; set; }
    }
}
