using BackendTask.Common.Middlewares;
using BackendTask.Manager;
using BackendTask.Data;
using Microsoft.AspNetCore.Builder;
using Serilog;
using BackendTask.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog based on environment
var environment = builder.Environment;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

builder.Host.UseSerilog((context, services, configuration) =>
{
    Serilog.Debugging.SelfLog.Enable(Console.Out);

    configuration.ReadFrom.Configuration(context.Configuration)
             .Enrich.FromLogContext();

        configuration.WriteTo.File("Logs/local-log-.txt", rollingInterval: RollingInterval.Day);
});

// Add services to the container.
builder.Services.ConfigureDataModule(configuration);
builder.Services.ConfigureManagerModule(configuration);
builder.Services.ConfigureApiControllers(configuration, "CorsPolicy");
builder.Services.ConfigureApiIdentity(configuration);
builder.Services.AddControllers();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
//Congiguring Health Ckeck
//builder.Services.ConfigureHealthChecks(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCustomExceptionHandler();

app.UseCors("CorsPolicy");

app.UseStaticFiles();

app.UseRequestLocalization();

app.UseResultWrapper();

app.UseRouting();

app.UseAuthentication();
app.UseRolePermissions();
app.UseAuthorization();


app.MapControllers();
app.UseUnitOfWork();

try
{
    Log.Information("Trying to migrate db.");
    await app.MigrateAndSeedDatabaseAsync();
}
catch (Exception exception)
{
    Log.Error(exception, "Stopped program because of exception");
    throw;
}

app.Run();

