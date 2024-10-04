using DAL.AppDbContextFolder;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;
using BLL.Repository;
using DAL.Services;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BLL.TokenManager;
using Microsoft.OpenApi.Models;

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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeserveData API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

builder.Services.AddScoped<IStationDataRepository, StationDataRepo>();
builder.Services.AddScoped<IStationDataService, StationDataService>();
builder.Services.AddScoped<IBusStopService, BusStopService>();
builder.Services.AddScoped<IBusStopRepository, BusStopRepo>();
builder.Services.AddScoped<IPlatformHeightRepository, PlatformHeightRepo>();
builder.Services.AddScoped<IPlatformHeightService, PlatformHeightService>();
builder.Services.AddScoped<IMachineLearningService, MachineLearningService>();
builder.Services.AddScoped<IMLRepository, MachineLearningRepo>();
builder.Services.AddScoped<IUserRepository, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IScoreRepository, ScoreRepo>();
builder.Services.AddScoped<IScoreInterface, ScoreService>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepo>();
builder.Services.AddScoped<IFeedBackService, FeedbackService>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepo>();
builder.Services.AddScoped<IFeedBackService, FeedbackService>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GenerateTokenManager.key)),
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("Connected", policy => policy.RequireAuthenticatedUser());
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
