using System;
using System.ComponentModel.DataAnnotations;

namespace SolarSystem.Data.Entities
{
    public class Body
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(0.01, double.MaxValue)]
        public double EarthMass { get; set; }
        [Range(0, double.MaxValue)]
        public double DistanceToTheSun { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int ComponentId { get; set; }
        public Component Component { get; set; }

        public int RegionId { get; set; }
        public Region Region { get; set; }
    }
}