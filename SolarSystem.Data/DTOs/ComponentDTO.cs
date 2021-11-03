using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolarSystem.Data.DTOs
{
    public class ComponentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ComponentDetailDTO : ComponentDTO
    {
        public IList<BodyDTO> Bodies { get; set; }
    }

    public class ManageComponentDTO
    {
        [StringLength(255, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(255, MinimumLength = 1)]
        public string Type { get; set; }
    }
}
