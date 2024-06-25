using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class FacilitiesDTO
    {
        public int Id_Station { get; set; }
        public bool PaidToilets { get; set; }
        public bool Taxi { get; set; }
        public bool LuggageLockers { get; set; }
        public bool FreeToilets { get; set; }
        public bool TVMCount { get; set; }
        public bool Wifi { get; set; }
        public bool BikesPointPresence { get; set; }
        public bool CambioInformation { get; set; }
        public bool ConnectingBusesPresence { get; set; }
        public bool ConnectingTramPresence { get; set; }
        public int ParkingPlacesForPMR { get; set; }
        public bool PMRToilets { get; set; }
        public bool Escalator { get; set; }
        public bool BlueBikesPresence { get; set; }
        public bool PMRAssistance { get; set; }
        public bool LiftOnPlatform { get; set; }
    }
}
