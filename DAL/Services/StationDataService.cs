using DAL.AppDbContextFolder;
using DAL.Entities;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System.Text.Json.Serialization;


namespace DAL.Services
{
    public class StationDataService : IStationDataService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public StationDataService(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }



        public async Task<IEnumerable<Station>> GetStationData()
        {
            return await _context.stations.ToListAsync();
        }

        public async Task<bool> FetchAndStoreStationsAsync(string lang)
        {
            try
            {
                var apiUrl = $"https://api.irail.be/v1/stations/?format=json&lang={lang}";

                var response = await _httpClient.GetFromJsonAsync<ApiResponse>(apiUrl);

                if (response != null && response.Station != null)
                {
                    foreach (var stationData in response.Station)
                    {
                        var existingStation = await _context.stations.FirstOrDefaultAsync(s => s.Official_Station_id == stationData.id); // Assuming each station has a unique Id

                        if (existingStation == null)
                        {
                            // If the station doesn't exist, create a new one
                            existingStation = new Station
                            {
                                Official_Station_id = stationData.id,
                                lon = stationData.locationX,
                                lat = stationData.locationY
                            };
                            _context.stations.Add(existingStation);
                        }

                        // Update the name based on the language
                        switch (lang.ToLower())
                        {
                            case "fr":
                                existingStation.name_fr = stationData.name;
                                break;
                            case "en":
                                existingStation.name_eng = stationData.name;
                                break;
                            case "nl":
                                existingStation.name_nl = stationData.name;
                                break;
                        }
                    }

                    // Save changes to database
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }


        // Inner class to deserialize API response
        private class ApiResponse
        {
            [JsonPropertyName("station")] //maps the C# property to the JSON key 
            public List<StationData> Station { get; set; }
        }

        private class StationData
        {
            public string id { get; set; }
            public string name { get; set; }
            public float locationX { get; set; }
            public float locationY { get; set; }
        }
    }
}
