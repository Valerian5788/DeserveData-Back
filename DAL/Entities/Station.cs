namespace DAL.Entities
{
    public class Station
    {
        public int Id_Station { get; set; }
        public string Official_Station_id { get; set; }

        public string? name_fr { get; set; }
        public string? name_nl { get; set; }
        public string name_eng { get; set; }

        public float lon { get; set; }
        public float lat { get; set; }

        // Navigation property for the one-to-one relationship with Facilities
        public Facilities Facilities { get; set; }
    }
}
