using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;
using Omega.DAL;

namespace Omega.Model
{
    public class CreateMood
    {
        static IConfigurationRoot Configuration;
        Requests r;
        public CreateMood()
        {
            r = new Requests();
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public void CreateAmbiance(string mood)
        {
            JObject rss = JObject.Parse(mood);
            r.AddMood((string)rss["User"], (string)rss["name"], (string)rss["metadonnees"], Configuration["data:azure:ConnectionString"]);
        }
    }
}
