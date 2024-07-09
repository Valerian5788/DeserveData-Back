using DAL.AppDbContextFolder;
using DAL.Entities;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Globalization;
using System.Text;
using DAL.DTO;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.Extensions.Logging;
using Microsoft.ML.Runtime;
using System.Reflection;


namespace DAL.Services
{
    public class StationDataService : IStationDataService
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly MLContext _mlContext;
        private readonly ILogger<StationDataService> _logger;

        public StationDataService(AppDbContext context, HttpClient httpClient, ILogger<StationDataService> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _mlContext = new MLContext();
            _logger = logger;
            RegisterCustomMapping();
        }



        public async Task<IEnumerable<StationDataDTO>> GetStationData()
        {
            var stations = await _context.stations
                .Include(s => s.Facilities)
                .Select(s => new StationDataDTO
                {
                    Id_Station = s.Id_Station,
                    Official_Station_id = s.Official_Station_id,
                    name_fr = s.name_fr,
                    name_nl = s.name_nl,
                    name_eng = s.name_eng,
                    lon = s.lon,
                    lat = s.lat,
                    Facilities = s.Facilities == null ? null : new FacilitiesDTO
                    {
                        Id_Station = s.Id_Station,
                        PaidToilets = s.Facilities.PaidToilets,
                        Taxi = s.Facilities.Taxi,
                        LuggageLockers = s.Facilities.LuggageLockers,
                        FreeToilets = s.Facilities.FreeToilets,
                        TVMCount = s.Facilities.TVMCount,
                        Wifi = s.Facilities.Wifi,
                        BikesPointPresence = s.Facilities.BikesPointPresence,
                        CambioInformation = s.Facilities.CambioInformation,
                        ConnectingBusesPresence = s.Facilities.ConnectingBusesPresence,
                        ConnectingTramPresence = s.Facilities.ConnectingTramPresence,
                        ParkingPlacesForPMR = s.Facilities.ParkingPlacesForPMR,
                        PMRToilets = s.Facilities.PMRToilets,
                        Escalator = s.Facilities.Escalator,
                        BlueBikesPresence = s.Facilities.BlueBikesPresence,
                        PMRAssistance = s.Facilities.PMRAssistance,
                        LiftOnPlatform = s.Facilities.LiftOnPlatform,
                    }
                })
                .ToListAsync();

            return stations;
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
                        var existingStation = await _context.stations.FirstOrDefaultAsync(s => s.Official_Station_id == stationData.Id); // Assuming each station has a unique Id

                        if (existingStation == null)
                        {
                            // If the station doesn't exist, create a new one
                            existingStation = new Station
                            {
                                Official_Station_id = stationData.Id,
                                lon = stationData.locationX,
                                lat = stationData.locationY
                            };
                            _context.stations.Add(existingStation);
                        }

                        // Update the name based on the language
                        switch (lang.ToLower())
                        {
                            case "fr":
                                existingStation.name_fr = stationData.Name;
                                break;
                            case "en":
                                existingStation.name_eng = stationData.Name;
                                break;
                            case "nl":
                                existingStation.name_nl = stationData.Name;
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
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }
            public float locationX { get; set; }
            public float locationY { get; set; }
        }

        private string NormalizeStationName(string name)
        {
            // Normalize the string to remove diacritics and convert to uppercase
            var normalizedString = name.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var upperCaseString = stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToUpperInvariant();

            // Remove all white spaces
            upperCaseString = upperCaseString.Replace(" ", "");

            // Replace opening parenthesis with '-' and remove closing parenthesis
            upperCaseString = upperCaseString.Replace("(", "-").Replace(")", "");

            return upperCaseString;
        }




        public async Task<bool> FetchAndStoreFacilitiesAsync()
        {
            List<string> stationsNotFound = new List<string>(); // List to collect names of stations not found
            try
            {
                var facilitiesPath = "../DAL/Intern Data/facilities.json";
                var facilitiesJson = await File.ReadAllTextAsync(facilitiesPath);
                var facilitiesData = JsonSerializer.Deserialize<List<FacilityData>>(facilitiesJson);

                if (facilitiesData == null) return false;

                foreach (var facility in facilitiesData)
                {
                    var normalizedFacilityName = NormalizeStationName(facility.station);

                    var stations = await _context.stations.ToListAsync();
                    // Attempt to find the station by normalized names
                    var station =   stations
                        .FirstOrDefault(s => NormalizeStationName(s.name_fr) == normalizedFacilityName ||
                                                  NormalizeStationName(s.name_eng) == normalizedFacilityName ||
                                                  NormalizeStationName(s.name_nl) == normalizedFacilityName);

                    if (station == null)
                    {
                        stationsNotFound.Add(facility.station);
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

                // After processing all facilities
                if (stationsNotFound.Any())
                {
                    Console.WriteLine("Stations not found:");
                    foreach (var stationName in stationsNotFound)
                    {
                        Console.WriteLine(stationName);
                    }
                }
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

        //ml
        public IDataView LoadData(string[] filePaths)
        {
            var data = new List<StationDataCSV>();

            foreach (var filePath in filePaths)
            {
                var lines = File.ReadAllLines(filePath).Skip(1); // Skip header
                foreach (var line in lines)
                {
                    var values = line.Split(',');
                    if (values.Length == 3)
                    {
                        data.Add(new StationDataCSV
                        {
                            timestamp = DateTime.ParseExact(values[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                            zone = values[1],
                            total = float.Parse(values[2])
                        });
                    }
                }
            }

            var dataView = _mlContext.Data.LoadFromEnumerable(data);

            // Log the schema
            Console.WriteLine("Schema of the loaded data:");
            foreach (var column in dataView.Schema)
            {
                Console.WriteLine($"{column.Name}: {column.Type}");
            }

            return dataView;
        }

        public ITransformer TrainModel(IDataView data, string modelPath)
        {
            var dataProcessPipeline = _mlContext.Transforms.Categorical.OneHotEncoding("ZoneEncoded", "zone")
                .Append(_mlContext.Transforms.CustomMapping<StationDataCSV, StationDataCSVTransformed>(
                    (input, output) =>
                    {
                        output.Hour = input.timestamp.Hour;
                        output.DayOfWeek = (float)input.timestamp.DayOfWeek;
                        output.Zone = input.zone;
                        output.Total = input.total;
                    }, "CustomMapping"))
                .Append(_mlContext.Transforms.Concatenate("Features", "Hour", "DayOfWeek", "ZoneEncoded"))
                .Append(_mlContext.Transforms.CopyColumns("Label", "Total"));

            var trainer = _mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features");
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            var model = trainingPipeline.Fit(data);

            // Save the model to a file
            _mlContext.Model.Save(model, data.Schema, modelPath);

            return model;
        }

        public ITransformer LoadModel(string modelPath)
        {
            RegisterCustomMapping(); // Ensure registration happens before loading the model
            DataViewSchema modelSchema;
            var model = _mlContext.Model.Load(modelPath, out modelSchema);
            return model;
        }

        private void RegisterCustomMapping()
        {
            try
            {
                // Register the assembly containing the custom mapping class
                _mlContext.ComponentCatalog.RegisterAssembly(typeof(StationDataCSVTransformed).Assembly);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to register custom mapping: {ex.Message}");
                throw;
            }
        }


        public void EvaluateModel(string[] filePaths, string modelPath)
        {
            var data = LoadData(filePaths);
            var model = LoadModel(modelPath);
            var predictions = model.Transform(data);
            var metrics = _mlContext.Regression.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");

            _logger.LogInformation($"R^2: {metrics.RSquared}");
            _logger.LogInformation($"Mean Absolute Error: {metrics.MeanAbsoluteError}");
            _logger.LogInformation($"Mean Squared Error: {metrics.MeanSquaredError}");
            _logger.LogInformation($"Root Mean Squared Error: {metrics.RootMeanSquaredError}");
        }

        public StationDataPrediction MakePrediction(string modelPath, StationDataCSV sampleData)
        {
            var model = LoadModel(modelPath);
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<StationDataCSV, StationDataPrediction>(model);
            var prediction = predictionEngine.Predict(sampleData);
            return prediction;
        }

        public class StationDataCSV
        {
            [LoadColumn(0)]
            public DateTime timestamp { get; set; }

            [LoadColumn(1)]
            public string zone { get; set; }

            [LoadColumn(2)]
            public float total { get; set; }
        }

        public class StationDataCSVTransformed
        {
            public float Hour { get; set; }
            public float DayOfWeek { get; set; }
            public string Zone { get; set; }
            public float Total { get; set; }
        }

        public class StationDataPrediction
        {
            [ColumnName("Score")]
            public float Total { get; set; }
        }
    }
}
