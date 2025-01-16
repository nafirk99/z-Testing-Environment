
using Serilog;
using Serilog.Events;
/**
 * @author:     Nafiz Imtiaz Khan
 * @since:      14/1/2025
 * @description:
 *      Implements Bootstrap Logging for immediate log capturing,
 *      Bootstrap Logger used When normal loggers failing to capture errors
 *      during application startup.
 *      Reads appsettings configuration from a directory manually
 *      before `appsettings.json` is fully loaded.
 */
#region Bootstrap(Start) Logger Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateBootstrapLogger();
#endregion
/*  
 **Encapsulate Subsequent Code in a Try-Catch Block for Robust Error Handling**
 This try-catch block safeguards the application's startup process by capturing any exceptions that might arise during initialization.
 By integrating DateTime.Now within the logging mechanism, we can meticulously track the time of exception occurrence.
*/
try
{
    Log.Information($"Application Starting.... Start_Time: {DateTime.Now}");
    var builder = WebApplication.CreateBuilder(args);

    /**
    * @author:     Nafiz Imtiaz Khan
    * @since:      14/1/2025
    * @description:
    *      Implements a file logger to capture errors and information 
    *      within controllers. 
    *      This logger leverages pre-configured Serilog settings, 
    *      eliminating the need for additional configuration.
    *      All logs will be written to the file after the application starts.
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


