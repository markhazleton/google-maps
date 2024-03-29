using GoogleMapsApi.HttpClientUtility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http;

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
        protected readonly IHttpClientFactory _httpClientFactory;
        protected readonly IHttpClientService _httpClientService;

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
            var serviceProvider = services.BuildServiceProvider();
            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            _httpClientService = new HttpClientService(_httpClientFactory, new NewtonsoftJsonStringConverter());

        }
        // Add check for null api and throw exception
        protected string ApiKey
        {
            get
            {
                var apiKey = Configuration.GetValue<string>("GOOGLE_API_KEY");
                return apiKey;
            }
        }
    }
}
