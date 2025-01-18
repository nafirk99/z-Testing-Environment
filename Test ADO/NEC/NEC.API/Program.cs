
using NEC.API.DAL;
using Serilog;
using Serilog.Events;
/**
* Bootstrap Logging Implementation
* Implements Bootstrap Logging for immediate log capturing.
* Bootstrap Logger used When normal loggers failing to capture errors
* during application startup.
* Reads appsettings configuration from a directory manually
* before `appsettings.json` is fully loaded.
* @author: Nafiz Imtiaz Khan
* @since: 14/1/2025
*/
#region Bootstrap(Start) Logger Configuration
/**
* AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
* 
* This method adds a JSON configuration file named "appsettings.json" to the configuration system.
* optional: false means that the file is required. If the file is not found, the application will fail to start.
* reloadOnChange: true enables automatic reloading of the configuration whenever the "appsettings.json" file is modified. 
* This is useful for dynamically updating settings without restarting the application.
*
* @author: Nafiz Imtiaz Khan
* @since: 16/1/2025 
*/

/**
* Adds another optional JSON configuration file to the system. 
* 
* Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") retrieves the current environment variable (e.g., "Development", "Staging", "Production").
* The file name is dynamically constructed based on the environment variable. 
* For example, in the "Development" environment, it will look for "appsettings.Development.json".
* 
* optional: true means that this file is not required. If the file is not found, 
* the configuration system will continue without it.
* 
* reloadOnChange: true enables automatic reloading of this file as well.
*
* @author: Nafiz Imtiaz Khan
* @since: 16/01/2025
*/

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)   
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
#endregion

/// <summary>
/// Encapsulates subsequent code in a try-catch block for robust error handling.
/// 
/// This try-catch block safeguards the application's startup process by capturing any exceptions that might arise during initialization.
/// By integrating DateTime.Now within the logging mechanism, we can meticulously track the time of exception occurrence.
/// <author>Nafiz Imtiaz Khan</author>
/// <since>16/01/2025</since>
/// </summary>
try
{
    Log.Information($"Application Starting.... Start_Time: {DateTime.Now}");
    var builder = WebApplication.CreateBuilder(args);

    /**
    * File Logger Implementation for Controllers
    * Implements a file logger to capture errors and information
    * within controllers.
    * This logger leverages pre-configured Serilog settings,
    * eliminating the need for additional configuration.
    * All logs will be written to the file after the application starts.
    * @author: Nafiz Imtiaz Khan
    * @since: 14/1/2025
    */

    #region Serilog General Configuration
    builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(builder.Configuration));
    #endregion

    // For Test
    var connectionString = builder.Configuration.GetConnectionString("NZWalksConnectionString");
    //Log.Information($"Connection: {connectionString}");



    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    // Register EmployeeDAL*******
    builder.Services.AddScoped<EmployeeDAL>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Failed to start application.");
}
finally
{
    // Clears Log Cache. This Code Always Executes
    Log.CloseAndFlush(); 
}


