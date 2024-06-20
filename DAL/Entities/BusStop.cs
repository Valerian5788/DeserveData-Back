using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class BusStop
    {
        public string StopId { get; set; }
        public string StopName { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }

        public string Provider { get; set; }

    }
}
