using System.ComponentModel.DataAnnotations;

namespace SolarSystem.Data.DTOs
{
    public class CommonField
    {
        [Required]
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public double EarthMass { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public double DistanceToTheSun { get; set; }
    }

    public class BodyDTO : CommonField
    {
        public int Id { get; set; }
    }

    public class BodyDetailDTO : BodyDTO
    {
        public RegionDTO Region { get; set; }
        public ComponentDTO Component { get; set; }
    }

    public class ManageBodyDTO : CommonField
    {
        public int ComponentId { get; set; }
        public int RegionId { get; set; }
    }
}