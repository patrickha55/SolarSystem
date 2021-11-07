using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.Data.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Range(0.1, double.MaxValue)]
        public double DistanceToTheSun { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public IList<Body> Bodies { get; set; }
    }
}
