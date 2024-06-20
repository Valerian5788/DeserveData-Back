
using DAL.Interfaces;
using System.Text.Json;

namespace DAL.Services
{
    public class BusStopService : IBusStopService
    {
        private readonly HttpClient _httpClient;

        public BusStopService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<object> GetBusStopsAroundStation(string city_name, double lat, double lon, double radius)
        {
            // Define the zone around the given coordinates
            double min_lat = lat - radius;
            double max_lat = lat + radius;
            double min_lon = lon - radius;
            double max_lon = lon + radius;

            // Construct the API URL with the search parameters
            string url = $"https://www.odwb.be/api/explore/v2.1/catalog/datasets/gtfs_tec_stops/records?select=*&where=stop_name%20like%20%27{city_name.ToUpper()}%27&order_by=stop_name&limit=99";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Response succces");
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<ApiResponse>(content);

                    var stopsInZone = new List<StopInfo>();
                    if (data != null && data.results != null)
                    {
                        Console.WriteLine("Data is not null");
                        foreach (var results in data.results)
                        {
                            if (results.stop_id != null && results.stop_name != null && results.stop_coordinates != null &&
                                min_lat < results.stop_coordinates.lat && results.stop_coordinates.lat < max_lat &&
                                min_lon < results.stop_coordinates.lon && results.stop_coordinates.lon < max_lon)
                            {
                                Console.WriteLine("Adding stop in zone");
                                stopsInZone.Add(new StopInfo
                                {
                                    stop_id = results.stop_id,
                                    stop_name = results.stop_name,
                                    stop_coordinates = results.stop_coordinates
                                });
                            }
                        }
                    }

                    return new { ArretAutourZone = stopsInZone, TotalDesArretsDansLaZone = stopsInZone.Count };
                }
                else
                {
                    return new { Error = $"Erreur lors de la récupération des données API, status code: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                return new { Error = $"An error occurred: {ex.Message}" };
            }
        }

        private class ApiResponse
        {
            public List<Result> results { get; set; }
        }

        private class Result
        {
            public string stop_id { get; set; }
            public string stop_name { get; set; }
            public Coordinates stop_coordinates { get; set; }
        }

        private class Coordinates
        {
            public double lat { get; set; }
            public double lon { get; set; }
        }

        private class StopInfo
        {
            public string stop_id { get; set; }
            public string stop_name { get; set; }
            public Coordinates stop_coordinates { get; set; }
        }
    }
}
