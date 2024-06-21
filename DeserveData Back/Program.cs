using DAL.AppDbContextFolder;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;
using BLL.Repository;
using DAL.Services;
using DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "DeserveData Api", Version = "v1" });
});

builder.Services.AddScoped<IStationDataRepository, StationDataRepo>();
builder.Services.AddScoped<IStationDataService, StationDataService>();
builder.Services.AddScoped<IBusStopService, BusStopService>();
builder.Services.AddScoped<IBusStopRepository, BusStopRepo>();
builder.Services.AddScoped<IPlatformHeightRepository, PlatformHeightRepo>();
builder.Services.AddScoped<IPlatformHeightService, PlatformHeightService>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
