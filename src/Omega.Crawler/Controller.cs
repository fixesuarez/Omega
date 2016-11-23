using Omega.DAL;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Omega.Crawler
{
    public class Controller
    {
        Analyser a;
        CredentialAuth c;
        Requests r;
        GetATrack gt;
        DatabaseCreator dc;
        DeezerConnect de;
        DeezerToSpotify s;
        SpotifyToDeezer std;
        static IConfigurationRoot Configuration;

        public Controller()
        {
            InitiateConfig();
            a = new Analyser();
            c = new CredentialAuth();
            r = new Requests();
            gt = new GetATrack();
            dc = new DatabaseCreator();
            de = new DeezerConnect();
            s = new DeezerToSpotify();
            std = new SpotifyToDeezer();
        }

        public static void InitiateConfig()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfigurationRoot Config()
        {
            return Configuration;
        }
        public SpotifyToDeezer SpotifyToDeezer()
        {
            return std;
        }

        public DeezerToSpotify GetSpotifycation()
        {
            return s;
        }

        public Analyser GetAnalyser()
        {
            return a;
        }

        public CredentialAuth GetCredentialAuth()
        {
            return c;
        }

        public Requests GetRequests()
        {
            return r;
        }

        public GetATrack GetGetATrack()
        {
            return gt;
        }

        public DatabaseCreator GetDatabaseCreator()
        {
            return dc;
        }

        public DeezerConnect GetDeezerConnect()
        {
            return de;
        }
    }
}
