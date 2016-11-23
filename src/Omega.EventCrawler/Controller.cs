using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using Omega.DAL;

namespace Omega.EventCrawler
{
    public class Controller
    {
        static IConfigurationRoot _configuration;
        EventGateway _eg;
        public Controller()
        {
            _eg = new EventGateway(_configuration["data:azure:ConnectionString"]);
        }

        public static void InitiateConfig()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public EventGateway GetEventGateway()
        {
            return _eg;
        }

        public IConfigurationRoot Config()
        {
            return _configuration;
        }
    }
}
