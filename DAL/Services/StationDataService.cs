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

        public async Task<bool> FetchAndStoreStationsAsync()
        {
            try
            {
                var apiUrl = "https://api.irail.be/v1/stations/?format=json&lang=fr";
                var response = await _httpClient.GetFromJsonAsync<ApiResponse>(apiUrl);

                if (response != null && response.Station != null)
                {
                    foreach (var stationData in response.Station)
                    {
                        // Check if station already exists in database
                        var existingStationNames = new HashSet<string>(await _context.stations_fr.Select(s => s.name).ToListAsync());

                        if (!existingStationNames.Contains(stationData.Name))
                        {
                            var station = new Station
                            {
                                name = stationData.Name,
                                lon = stationData.LocationX,
                                lat = stationData.LocationY
                            };

                            // Add station to context
                            _context.stations_fr.Add(station);
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
            public string Name { get; set; }
            public float LocationX { get; set; }
            public float LocationY { get; set; }
        }
    }
}
