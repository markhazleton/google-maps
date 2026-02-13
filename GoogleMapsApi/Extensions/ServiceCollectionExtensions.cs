using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WebSpark.HttpClientUtility.RequestResult;
using WebSpark.HttpClientUtility.StringConverter;
using System;
using System.Net.Http;

namespace GoogleMapsApi.Extensions
{
    /// <summary>
    /// Extension methods for configuring Google Maps API services following Azure best practices
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Google Maps API services with best practices configuration
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddGoogleMapsApi(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure HttpClient with resilience patterns and best practices
            services.AddHttpClient("GoogleMapsApi", client =>
            {
                // Set reasonable timeout
                client.Timeout = TimeSpan.FromSeconds(30);

                // Add user agent for API identification
                client.DefaultRequestHeaders.Add("User-Agent", "GoogleMapsApi.NET/1.0");

                // Enable keep-alive for better performance
                client.DefaultRequestHeaders.Connection.Add("keep-alive");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                // Enable compression for better performance
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,

                // Security: Use system proxy settings
                UseProxy = true,
                UseDefaultCredentials = false
            });

            // Register WebSpark.HttpClientUtility services
            services.AddSingleton<IStringConverter, NewtonsoftJsonStringConverter>();
            services.AddTransient<IHttpRequestResultService, HttpRequestResultService>();

            return services;
        }

        /// <summary>
        /// Validates Google Maps API configuration
        /// </summary>
        /// <param name="configuration">The configuration to validate</param>
        /// <returns>True if configuration is valid</returns>
        public static bool ValidateGoogleMapsConfiguration(this IConfiguration configuration)
        {
            var apiKey = configuration.GetValue<string>("GOOGLE_API_KEY");

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Google Maps API key is not configured. Please set the GOOGLE_API_KEY configuration value.");
            }

            if (apiKey.Length < 10)
            {
                throw new InvalidOperationException(
                    "Google Maps API key appears to be invalid (too short).");
            }

            return true;
        }
    }
}
