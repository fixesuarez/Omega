using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omega.EventCrawler
{
    public class Program
    {
        static Controller ct = new Controller();

        static void Main(string[] args)
        {
            Crawl().Wait();
        }

        static async Task Crawl()
        {
            for (;;)
            {
                
            }
        }
    }
}
