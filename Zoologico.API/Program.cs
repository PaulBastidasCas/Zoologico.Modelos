using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zoologico.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //==============================================================
            // Configurar Serilog leyendo desde appsettings.json
            //==============================================================
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            Log.Information("Iniciado el proceso de LOGGER");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ZoologicoAPIContext>(options =>
            
            options.UseNpgsql(builder.Configuration.GetConnectionString("ZoologicoAPIContext.postgresql") ?? throw new InvalidOperationException("Connection string 'ZoologicoAPIContext' not found."))

            );

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure JSON options
            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(
                    options => 
                    options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
