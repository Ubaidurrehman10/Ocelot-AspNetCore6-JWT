using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using Poc.Middleware.Auth;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config
            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("ocelot.json", false, true);
        //.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        //.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: false, reloadOnChange: true)
        //.AddEnvironmentVariables();

        //if (hostingContext.HostingEnvironment.EnvironmentName == "Development")
        //{
        //    config.AddJsonFile("appsettings.Local.json", true, true);
        //}
        config.AddJsonFile("appsettings.json", true, true);
    })
    .UseSerilog((_, config) =>
    {
        config
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
        .WriteTo.Console();
    });

//builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration); // JWT Configuration

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();


app.UseMiddleware<RequestResponseLogging>();

app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();
