using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GoogleMapsApi.Configuration
{
    /// <summary>
    /// Configuration options for Google Maps API following Azure best practices
    /// </summary>
    public class GoogleMapsApiOptions
    {
        public const string SectionName = "GoogleMapsApi";

        /// <summary>
        /// Google Maps API Key - should be stored in Azure Key Vault or user secrets
        /// </summary>
        [Required]
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Base URL for Google Maps API
        /// </summary>
        public string BaseUrl { get; set; } = "https://maps.googleapis.com";

        /// <summary>
        /// Default timeout for requests
        /// </summary>
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Maximum number of concurrent requests
        /// </summary>
        public int MaxConcurrentRequests { get; set; } = 10;

        /// <summary>
        /// Enable request performance logging
        /// </summary>
        public bool EnablePerformanceLogging { get; set; } = true;

        /// <summary>
        /// Enable detailed request logging (be careful with sensitive data)
        /// </summary>
        public bool EnableRequestLogging { get; set; } = false;
    }

    /// <summary>
    /// Extension methods for configuring Google Maps API options
    /// </summary>
    public static class GoogleMapsApiConfigurationExtensions
    {
        /// <summary>
        /// Adds and validates Google Maps API configuration
        /// </summary>
        public static IServiceCollection AddGoogleMapsApiConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<GoogleMapsApiOptions>(
                configuration.GetSection(GoogleMapsApiOptions.SectionName));

            // Add validation
            services.AddSingleton<IValidateOptions<GoogleMapsApiOptions>, GoogleMapsApiOptionsValidator>();

            return services;
        }
    }    /// <summary>
         /// Validator for Google Maps API options
         /// </summary>
    public class GoogleMapsApiOptionsValidator : IValidateOptions<GoogleMapsApiOptions>
    {
        /// <summary>
        /// Validates the Google Maps API options configuration
        /// </summary>
        /// <param name="name">The name of the options instance</param>
        /// <param name="options">The options instance to validate</param>
        /// <returns>The validation result</returns>
        public ValidateOptionsResult Validate(string name, GoogleMapsApiOptions options)
        {
            var failures = new List<string>();

            if (string.IsNullOrWhiteSpace(options.ApiKey))
            {
                failures.Add("Google Maps API key is required. Please configure the GOOGLE_API_KEY setting.");
            }
            else if (options.ApiKey.Length < 10)
            {
                failures.Add("Google Maps API key appears to be invalid (too short).");
            }

            if (options.DefaultTimeout <= TimeSpan.Zero)
            {
                failures.Add("Default timeout must be greater than zero.");
            }

            if (options.MaxConcurrentRequests <= 0)
            {
                failures.Add("Maximum concurrent requests must be greater than zero.");
            }

            if (failures.Any())
            {
                return ValidateOptionsResult.Fail(failures);
            }

            return ValidateOptionsResult.Success;
        }
    }
}
