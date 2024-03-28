using Microsoft.Extensions.Configuration;
using System.IO;

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

        public BaseTestIntegration()
        {
            // Update to use UserSecrets

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddUserSecrets<BaseTestIntegration>()
                .AddEnvironmentVariables()
                .Build();
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
