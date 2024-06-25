using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class StationDataDTO
    {
        public int Id_Station { get; set; }
        public string Official_Station_id { get; set; }

        public string? name_fr { get; set; }
        public string? name_nl { get; set; }
        public string name_eng { get; set; }

        public float lon { get; set; }
        public float lat { get; set; }

        public FacilitiesDTO? Facilities { get; set; }
    }
}
