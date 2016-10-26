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
                headers.Add("content-type", "application/json");
                HttpResponseMessage message = await client.GetAsync("https://api.spotify.com/v1/audio-features/" + songId);

                using (Stream responseStream = await message.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string responseFromServer = reader.ReadToEnd();
                    information = JsonConvert.DeserializeObject<MetaDonnees>(responseFromServer);
                    return information;

                }
            }

            //WebRequest request = HttpWebRequest.Create("https://api.spotify.com/v1/audio-features/" + songId);
            //request.Method = "GET";
            //request.Headers.Add("Authorization", string.Format("Bearer {0}", token));
            //request.ContentType = "application/json";
            //using (WebResponse response = await request.GetResponseAsync())
            //using (Stream responseStream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(responseStream))
            //{
            //    string responseFromServer = reader.ReadToEnd();
            //    information = JsonConvert.DeserializeObject<MetaDonnees>(responseFromServer);
            //    return information;
            //}
        }

        public async Task<string> GetAccessToken()
        {
            SpotifyToken token = new SpotifyToken();

            string postString = string.Format("grant_type=client_credentials");
            byte[] byteArray = Encoding.UTF8.GetBytes(postString);

            string url = "https://accounts.spotify.com/api/token";

            using (HttpClient client = new HttpClient())
            {
                HttpRequestHeaders headers = client.DefaultRequestHeaders;
                headers.Add("Authorization", string.Format("Basic {0}", "YTY3MTI4NjA3ZmYyNGIxYTk4NWFiZjU0YWEzOTViY2Y6OTRkZjZhNzU5ZWIwNDU3M2JhMjBlNzUyNDljODI4ODk = "));
                headers.Add("content-type", "application/x-www-form-urlencoded");
                HttpResponseMessage message = await client.PostAsync(url, new ByteArrayContent(byteArray));

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