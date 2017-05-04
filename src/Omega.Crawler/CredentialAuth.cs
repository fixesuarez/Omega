using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Omega.DAL;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Omega.Crawler
{
    public class CredentialAuth
    {
        public async Task<MetaDonnees> TrackMetadonnee(string songId)
        {
            string token = await GetAccessToken();
            MetaDonnees information = new MetaDonnees();

            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Bearer {0}", token));
                HttpResponseMessage message = await client.GetAsync("https://api.spotify.com/v1/audio-features/" + songId);

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    information = JsonConvert.DeserializeObject<MetaDonnees>(responseFromServer);
                    return information;

                }
            }
        }

        public async Task<string> GetAccessToken()
        {
            SpotifyToken token = new SpotifyToken();

            string postString = "grant_type=client_credentials";

            string url = "https://accounts.spotify.com/api/token";

            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Basic {0}", "MTQxY2NkYTBmMmQ3NDI2YTgzMzlmODk3ZDg5ZDYzNGI6Yzg4YWZjNWRkOTUyNDZjODkwNzAyMTAyMGJiODRjYjE="));
                HttpResponseMessage message = await client.PostAsync(url, new StringContent(postString, Encoding.UTF8, "application/x-www-form-urlencoded"));

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    token = JsonConvert.DeserializeObject<SpotifyToken>(responseFromServer);

                }
            }
            return token.access_token;
        }
    }

}