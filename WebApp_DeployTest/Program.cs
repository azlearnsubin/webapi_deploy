using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure AppSettings
//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddOptions<AppSettings>()
    .BindConfiguration(nameof(AppSettings));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.MapGet("/config", (IOptions<AppSettings> options) =>
    {
        var appSettings = options.Value;
        return new
        {
            AppName = appSettings.AppName,
            Version = appSettings.Version,
            Environment = appSettings.Environment,
            DatabaseConnection = appSettings.DatabaseConnection,
            ApiBaseUrl = appSettings.ApiBaseUrl,
            ConfigurationType = "IOptions (Singleton)",
            Timestamp = DateTime.Now
        };
    })
    .WithName("GetConfiguration")
    .WithOpenApi();

app.MapGet("/config-snapshot", (IOptionsSnapshot<AppSettings> snapshot) =>
    {
        var appSettings = snapshot.Value;
        return new
        {
            AppName = appSettings.AppName,
            Version = appSettings.Version,
            Environment = appSettings.Environment,
            DatabaseConnection = appSettings.DatabaseConnection,
            ApiBaseUrl = appSettings.ApiBaseUrl,
            ConfigurationType = "IOptionsSnapshot (Scoped)",
            Timestamp = DateTime.Now
        };
    })
    .WithName("GetConfigurationSnapshot")
    .WithOpenApi();

app.MapGet("/config-snapshot123", (IOptionsSnapshot<AppSettings> snapshot) =>
    {
        var appSettings = snapshot.Value;
        return new
        {
            AppName = appSettings.AppName,
            Version = appSettings.Version,
            Environment = appSettings.Environment,
            DatabaseConnection = appSettings.DatabaseConnection,
            ApiBaseUrl = appSettings.ApiBaseUrl,
            ConfigurationType = "IOptionsSnapshot (Scoped)",
            Timestamp = DateTime.Now
        };
    })
    .WithName("GetConfigurationSnapshot123")
    .WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class AppSettings
{
    public string AppName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
    public string DatabaseConnection { get; set; } = string.Empty;
    public string ApiBaseUrl { get; set; } = string.Empty;
}