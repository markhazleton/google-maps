using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using WebSpark.HttpClientUtility.RequestResult;
using WebSpark.HttpClientUtility.StringConverter;

namespace GoogleMapsApi.Test.IntegrationTests
{
    //  Note:	The integration tests run against the real Google API web
    //			servers and count towards your query limit. Also, the tests
    //			require a working internet connection in order to pass.
    //			Their run time may vary depending on your connection,
    //			network congestion and the current load on Google's servers.

    public class BaseTestIntegration
    {
        private readonly IConfigurationRoot Configuration;
        protected readonly IHttpClientFactory? _httpClientFactory;
        protected readonly IHttpRequestResultService _httpClientService;

        public BaseTestIntegration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets<BaseTestIntegration>()
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IStringConverter, NewtonsoftJsonStringConverter>();
            services.AddTransient<IHttpRequestResultService, HttpRequestResultService>();

            var serviceProvider = services.BuildServiceProvider();
            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            _httpClientService = serviceProvider.GetRequiredService<IHttpRequestResultService>();
        }

        protected string ApiKey
        {
            get
            {
                var apiKey = Configuration.GetValue<string>("GOOGLE_API_KEY");
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new InvalidOperationException("Google API Key is not configured. Please set GOOGLE_API_KEY in appsettings.json, user secrets, or environment variables.");
                }
                return apiKey;
            }
        }
    }
}
