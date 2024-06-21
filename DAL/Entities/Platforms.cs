using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Platforms
    {
        public string Perron_Id { get; set; }
        public string Id_Quai { get; set; }
        public string Hauteur { get; set; }
        public string LongNameFrench { get; set; }
        public string LongNameDutch { get; set; }
        public int Id_Station { get; set; }
        public Station Station { get; set; }
    }
}
