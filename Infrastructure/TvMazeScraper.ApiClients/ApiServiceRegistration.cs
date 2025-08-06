using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using TvMazeScraper.Services;

namespace TvMazeScraper.ApiClients
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Define a Polly retry policy  
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(response => response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)  
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        var statusCode = outcome.Result?.StatusCode;
                        Console.WriteLine($"Retry {retryAttempt} after {timespan.TotalSeconds}s due to {outcome.Exception?.Message ?? statusCode.ToString()}");
                    });

            services.AddHttpClient<ITvMazeApiClientService, TvMazeApiClientService>()
                .AddPolicyHandler(retryPolicy);

            return services;
        }
    }
}
