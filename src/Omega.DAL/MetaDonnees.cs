namespace Omega.DAL
{
    public class MetaDonnees
    {
        public MetaDonnees(string pdanceability, string penergy, string ploudness, string pspeechiness, string pacousticness, string pinstrumentalness, string pliveness, string pvalence, string ptempo, string ppopularity)
        {
            danceability = pdanceability;
            energy = penergy;
            loudness = ploudness;
            speechiness = pspeechiness;
            accousticness = pacousticness;
            instrumentalness = pinstrumentalness;
            liveness = pliveness;
            valence = pvalence;
            tempo = ptempo;
            popularity = ppopularity;
        }

        public MetaDonnees()
        {

        }
        public string danceability { get; set; }
        public string energy { get; set; }
        public string key { get; set; }
        public string loudness { get; set; }
        public string mode { get; set; }
        public string speechiness { get; set; }
        public string accousticness { get; set; }
        public string instrumentalness { get; set; }
        public string liveness { get; set; }
        public string valence { get; set; }
        public string tempo { get; set; }
        public string type { get; set; }
        public string id { get; set; }
        public string uri { get; set; }
        public string track_href { get; set; }
        public string analysis_url { get; set; }
        public string duration_ms { get; set; }
        public string time_signature { get; set; }
        public string popularity { get; set; }
    }
}