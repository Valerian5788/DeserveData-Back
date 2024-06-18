namespace DAL.Entities
{
    public class Station
    {
        public int Id_Stations_fr { get; set; }
        public string name { get; set; }
        public float lon { get; set; }
        public float lat { get; set; }

        // Navigation property for the one-to-one relationship with Facilities
        public Facilities Facilities { get; set; }
    }
}
