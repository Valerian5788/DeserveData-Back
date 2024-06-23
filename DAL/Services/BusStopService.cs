
using DAL.AppDbContextFolder;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace DAL.Services
{
    public class BusStopService : IBusStopService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;

        public BusStopService(HttpClient httpClient, AppDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<bool> ImportBusStopsFromMultipleFiles()
        {
            string[] filePaths = 
            [
                "../DAL/Intern Data/stops_TEC.txt",
                "../DAL/Intern Data/stops_STIB.txt",
                "../DAL/Intern Data/stops_LIJN.txt"
            ];

            try
            {
                foreach (var filePath in filePaths)
                {
                    string provider = Path.GetFileNameWithoutExtension(filePath).Split('_').Last().ToUpperInvariant();
                    using (var reader = new StreamReader(filePath))
                    {
                        reader.ReadLine(); // Skip header

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');

                            if (values.Length < 6) // Assuming there should be at least 6 columns
                            {
                                Console.WriteLine($"Skipping malformed line: {line}");
                                continue;
                            }

                            var stopId = values[0].Trim('"');
                            var stopName = values[2].Trim('"');

                            // Safely parse latitude and longitude
                            if (!double.TryParse(values[4].Trim('"'), NumberStyles.Any, CultureInfo.InvariantCulture, out double lat))
                            {
                                Console.WriteLine($"Skipping line due to invalid latitude: {line}");
                                continue;
                            }

                            if (!double.TryParse(values[5].Trim('"'), NumberStyles.Any, CultureInfo.InvariantCulture, out double lon))
                            {
                                Console.WriteLine($"Skipping line due to invalid longitude: {line}");
                                continue;
                            }

                            var existingStop = await _context.busStop.FindAsync(stopId);
                            if (existingStop == null)
                            {
                                _context.busStop.Add(new BusStop
                                {
                                    StopId = stopId,
                                    StopName = stopName,
                                    Lat = lat,
                                    Lon = lon,
                                    Provider = provider
                                });
                            }
                            else
                            {
                                // Update existing record if needed
                                existingStop.StopName = stopName;
                                existingStop.Lat = lat;
                                existingStop.Lon = lon;
                                existingStop.Provider = provider;
                            }
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return true; // Return true to indicate success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Return false if an exception occurs
            }
        }




        // Convert degrees to radians
        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        // Calculate the Haversine distance between two points
        private double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusKm = 6371.0;

            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);

            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c;
        }

        public async Task<object> GetBusStopsAroundStation(double lat, double lon, double radiusKm)
        {
            // Define the zone around the given coordinates
            const double KmToDegrees = 0.009; // Approximate conversion for small distances
            double radiusDegrees = radiusKm * KmToDegrees;
            double min_lat = lat - radiusDegrees;
            double max_lat = lat + radiusDegrees;
            double min_lon = lon - radiusDegrees;
            double max_lon = lon + radiusDegrees;

            try
            {
                // Query the database for bus stops within the specified bounds
                var stopsInZone = await _context.busStop
                    .Where(bs => bs.Lat >= min_lat && bs.Lat <= max_lat && bs.Lon >= min_lon && bs.Lon <= max_lon)
                    .Select(bs => new StopInfo
                    {
                        stop_id = bs.StopId,
                        stop_name = bs.StopName,
                        stop_coordinates = new Coordinates { lat = bs.Lat, lon = bs.Lon }
                    })
                    .ToListAsync();

                // Filter stops to only include those within the radius
                var stopsInCircle = stopsInZone
                    .Where(stop => Haversine(lat, lon, stop.stop_coordinates.lat, stop.stop_coordinates.lon) <= radiusKm)
                    .ToList();

                return new { StopsInZone = stopsInCircle, TotalStopsInZone = stopsInCircle.Count };
            }
            catch (Exception ex)
            {
                return new { Error = $"An error occurred: {ex.Message}" };
            }
        }
    }

    // Ensure the Coordinates class is accessible
    public class Coordinates
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    // Ensure the StopInfo class is accessible
    public class StopInfo
    {
        public string stop_id { get; set; }
        public string stop_name { get; set; }
        public Coordinates stop_coordinates { get; set; }
    }
}
