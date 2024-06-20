
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

        public async Task<bool> ImportBusStopsFromFileTec()
        {
            try
            {
                string filePath = "../DAL/Intern Data/stops.txt";
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

                        var stopId = values[0];
                        var stopName = values[2].Trim('"');

                        // Safely parse latitude and longitude
                        if (!double.TryParse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture, out double lat))
                        {
                            Console.WriteLine($"Skipping line due to invalid latitude: {line}");
                            continue;
                        }

                        if (!double.TryParse(values[5], NumberStyles.Any, CultureInfo.InvariantCulture, out double lon))
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
                                Provider = "Tec"
                            });
                        }
                        else
                        {
                            // Update existing record if needed
                            existingStop.StopName = stopName;
                            existingStop.Lat = lat;
                            existingStop.Lon = lon;
                            existingStop.Provider = "Tec";
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                return true; // Return true to indicate success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Return false if an exception occurs
            }
        }



        public async Task<object> GetBusStopsAroundStation(double lat, double lon, double radius)
        {
            // Define the zone around the given coordinates
            double min_lat = lat - radius;
            double max_lat = lat + radius;
            double min_lon = lon - radius;
            double max_lon = lon + radius;

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

                return new { StopsInZone = stopsInZone, TotalStopsInZone = stopsInZone.Count };
            }
            catch (Exception ex)
            {
                return new { Error = $"An error occurred: {ex.Message}" };
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
}
