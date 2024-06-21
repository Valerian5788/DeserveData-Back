

using DAL.AppDbContextFolder;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DAL.Services
{
    public class PlatformHeightService : IPlatformHeightService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public PlatformHeightService(AppDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<bool> FetchAndStoreAllPlatformData()
        {
            string baseUrl = "https://opendata.infrabel.be/api/explore/v2.1/catalog/datasets/perronhoogten-in-stations/records";
            int limit = 99;
            int offset = 0;
            int totalFetched = 0;
            int totalCount = 0;

            try
            {
                do
                {
                    string url = $"{baseUrl}?limit={limit}&offset={offset}";
                    var response = await _httpClient.GetStringAsync(url);

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    var data = JsonSerializer.Deserialize<ApiResponse>(response, options);

                    if (data?.results != null)
                    {
                        totalCount = data.total_count;
                        foreach (var platformData in data.results)
                        {
                            var matchingStation = await _context.stations
                                .FirstOrDefaultAsync(s => s.name_fr == platformData.LongNameFrench);

                            if (matchingStation != null)
                            {
                                var existingPlatform = await _context.platforms.FindAsync(platformData.perron_id);
                                if (existingPlatform == null)
                                {
                                    var newPlatform = new Platforms
                                    {
                                        Perron_Id = platformData.perron_id,
                                        Id_Quai = platformData.Quai,
                                        LongNameDutch = platformData.LongNameDutch,
                                        LongNameFrench = platformData.LongNameFrench,
                                        Hauteur = platformData.Hauteur,
                                        Id_Station = matchingStation.Id_Station
                                    };
                                    _context.platforms.Add(newPlatform);
                                }
                                else
                                {
                                    // Optionally, update existingPlatform fields here
                                }
                            }
                            else
                            {
                                Console.WriteLine($"station : {platformData.LongNameFrench} n'a pas été trouvée");
                                // Handle the case where no matching station is found
                            }
                        }
                        await _context.SaveChangesAsync();

                        totalFetched += data.results.Count;
                        offset += limit;
                    }
                } while (totalFetched < totalCount);

                return true; // Success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false; // Failure
            }
        }

        public class ApiResponse
        {
            public int total_count { get; set; }
            public List<PlatformData> results { get; set; }
        }

        public class PlatformData
        {
            public string perron_id { get; set; }
            public string Quai { get; set; }
            public string LongNameDutch { get; set; }
            public string LongNameFrench { get; set; }
            public string Hauteur { get; set; }
        }

    }
}
