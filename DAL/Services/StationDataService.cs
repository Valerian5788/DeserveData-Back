using DAL.AppDbContextFolder;
using DAL.Entities;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;


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

        public async Task<bool> FetchAndStoreFacilitiesAsync()
        {
            try
            {
                var facilitiesPath = "../DAL/Intern Data/facilities.json";
                var facilitiesJson = await File.ReadAllTextAsync(facilitiesPath);
                var facilitiesData = JsonSerializer.Deserialize<List<FacilityData>>(facilitiesJson);

                if (facilitiesData == null) return false;

                foreach (var facility in facilitiesData)
                {
                    // Attempt to find the station by name_fr, then name_en, and finally name_nl
                    var station = await _context.stations
                        .FirstOrDefaultAsync(s => s.name_fr == facility.station || s.name_eng == facility.station || s.name_nl == facility.station);

                    if (station == null)
                    {
                        Console.WriteLine($"Station not found for {facility.station}");
                        continue; // Skip this facility if the station is not found
                    }

                    // Attempt to parse the number of parking places for PMR
                    var parkingPlacesForPMRString = facility.facilities.FirstOrDefault(f => f.name == "Number of Parking Places for PMR")?.value;
                    int parkingPlacesForPMR = 0;
                    if (!string.IsNullOrEmpty(parkingPlacesForPMRString))
                    {
                        // Split the string on the colon and parse the last part
                        var parts = parkingPlacesForPMRString.Split(':');
                        if (parts.Length > 1 && int.TryParse(parts.Last().Trim(), out int parsedNumber))
                        {
                            parkingPlacesForPMR = parsedNumber;
                        }
                        else
                        {
                            Console.WriteLine($"Unable to parse number from string: '{parkingPlacesForPMRString}'");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Parking Places for PMR string is null or empty.");
                    }

                    var stationFacilities = new Facilities
                    {
                        Id_Station = station.Id_Station,
                        PaidToilets = facility.facilities.Any(f => f.name == "Paid Toilets"),
                        FreeToilets = facility.facilities.Any(f => f.name == "Free Toilets"), 
                        Taxi = facility.facilities.Any(f => f.name == "Location of taxis"), 
                        LuggageLockers = facility.facilities.Any(f => f.name == "Luggage Lockers"),
                        TVMCount = facility.facilities.Any(f => f.name == "TVM Count"),
                        Wifi = facility.facilities.Any(f => f.name == "Wifi presence"), 
                        BikesPointPresence = facility.facilities.Any(f => f.name == "Presence of the Bikes point"), 
                        CambioInformation = facility.facilities.Any(f => f.name == "Cambio information"),
                        ConnectingBusesPresence = facility.facilities.Any(f => f.name == "Presence of connecting buses"),
                        ConnectingTramPresence = facility.facilities.Any(f => f.name == "Presence of connecting tram"),
                        ParkingPlacesForPMR = parkingPlacesForPMR, 
                        PMRToilets = facility.facilities.Any(f => f.name == "PMR Toilets"),
                        Escalator = facility.facilities.Any(f => f.name == "Escalator"),
                        BlueBikesPresence = facility.facilities.Any(f => f.name == "Blue Bikes presence"), 
                        PMRAssistance = facility.facilities.Any(f => f.name == "Has PMR Assistance"), 
                        LiftOnPlatform = facility.facilities.Any(f => f.name == "Lift on the platform"),
                    };

                    _context.facilities.Add(stationFacilities);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        // Inner class to deserialize facilities.json
        private class FacilityData
        {
            public string station { get; set; }
            public List<FacilityDetail> facilities { get; set; }
        }

        private class FacilityDetail
        {
            public string name { get; set; }
            public string value { get; set; }
        }
    }
}
