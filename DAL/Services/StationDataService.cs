using DAL.AppDbContextFolder;
using DAL.Entities;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;


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

                if (response != null && response.Stations != null)
                {
                    foreach (var stationData in response.Stations)
                    {
                        // Check if station already exists in database
                        var existingStation = await _context.Stations.FirstOrDefaultAsync(s => s.Name == stationData.Name);

                        if (existingStation == null)
                        {
                            // Create new station entity
                            var station = new Station
                            {
                                Name = stationData.Name,
                                Longitude = Convert.ToDouble(stationData.LocationX),
                                Latitude = Convert.ToDouble(stationData.LocationY)
                            };

                            // Add station to context
                            _context.Stations.Add(station);
                        }
                        // You can add logic here if you want to update existing stations

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
                // Handle exceptions (log, etc.)
                return false;
            }
        }

        // Inner class to deserialize API response
        private class ApiResponse
        {
            public List<StationData> Stations { get; set; }
        }

        private class StationData
        {
            public string Name { get; set; }
            public string LocationX { get; set; }
            public string LocationY { get; set; }
        }
    }
}
