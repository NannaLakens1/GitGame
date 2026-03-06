using System.ComponentModel.DataAnnotations;

namespace GameBackend.Models
{
    public class Object2D
    {
        public Guid Id { get; set; }
        //[Required]
        public Guid EnvironmentId { get; set; }

        [Required]
        public string? PrefabId { get; set; }
        [Required]
        public float PositionX { get; set; }
        [Required]
        public float PositionY { get; set; }
        public float ScaleX { get; set; }
        public float ScaleY { get; set; }
        public float RotationZ { get; set; }
        public int SortingLayer { get; set; }

    }
}
