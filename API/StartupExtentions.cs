using Microsoft.OpenApi.Models;
using TvMazeScraper.ApiClients;
using TvMazeScraper.Services;
using TvMazeScraper.Persistence;
using TvMazeScraper.Application;
using System.Text.Json.Serialization;

namespace TvMazeScraper
{
    public static class StartupExtentions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers()
             .AddJsonOptions(options =>
              {
                  options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
              });


            // Adding services from layers
            builder.Services.AddPersistenceServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddApiServices(); 
            

            builder.Services.AddScoped<TvMazeDataIngestionService>();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Open", policy =>
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TvMazeScraper API",
                    Version = "v1"
                });
            });

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TvMazeScraper API");
                });
            }

            app.UseHttpsRedirection();
            app.UseCors("Open");
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
