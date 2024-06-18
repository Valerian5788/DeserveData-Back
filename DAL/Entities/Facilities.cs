namespace DAL.Entities
{
    public class Facilities
    {
        public int Id_Stations_fr { get; set; }
        public bool PaidToilets { get; set; }
        public bool Taxi { get; set; }
        public bool LuggageLockers { get; set; }
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
        // Navigation property back to Station
        public Station Station { get; set; }

        // Additional properties can be added here based on further requirements
    }
}
