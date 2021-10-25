using HLE.HttpRequests;
using System.Text.Json;

namespace ApuDoingStuff.API
{
    public class HTTPRequest
    {
        private const string _randomShibaUrl = "http://shibe.online/api/shibes";
        private const string _randomDuckUrl = "https://random-d.uk/api/random";
        private const string _randomCatUrl = "https://aws.random.cat/meow";
        private const string _randomCatFact = "https://catfact.ninja/fact";
        private const string _randomDogUrl = "https://random.dog/woof.json";
        private const string _randomAxolotlUrl = "https://axoltlapi.herokuapp.com/";

        public static string RandomShibaUrl()
        {
            HttpGet request = new(_randomShibaUrl);
            if (request.ValidJsonData)
            {
                return request.Data[0].GetString();
            }
            else
            {
                return request.Result;
            }
        }

        public static string RandomDuckUrl()
        {
            HttpGet request = new(_randomDuckUrl);
            if (request.ValidJsonData)
            {
                return request.Data.GetProperty("url").GetString();
            }
            else
            {
                return request.Result;
            }
        }

        public static string RandomCatUrl()
        {
            HttpGet request = new(_randomCatUrl);
            if (request.ValidJsonData)
            {
                return request.Data.GetProperty("file").GetString();
            }
            else
            {
                return request.Result;
            }
        }

        public static string RandomDogUrl()
        {
            HttpGet request = new(_randomDogUrl);
            if (request.ValidJsonData)
            {
                return request.Data.GetProperty("url").GetString();
            }
            else
            {
                return request.Result;
            }
        }

        public static string RandomAxolotlUrl()
        {
            HttpGet request = new(_randomAxolotlUrl);
            if (request.ValidJsonData)
            {
                return request.Data.GetProperty("url").GetString();
            }
            else
            {
                return request.Result;
            }
        }

        public static string RandomAxolotFact()
        {
            HttpGet request = new(_randomAxolotlUrl);
            if (request.ValidJsonData)
            {
                return request.Data.GetProperty("facts").GetString();
            }
            else
            {
                return request.Result;
            }
        }

        public static string RandomCatFact()
        {
            HttpGet request = new(_randomCatFact);
            if (request.ValidJsonData)
            {
                return request.Data.GetProperty("fact").GetString();
            }
            else
            {
                return request.Result;
            }
        }

        public static string LyricsUrl(string title)
        {
            HttpGet request = new($"https://some-random-api.ml/lyrics?title={title}");
            JsonElement lyricsLink = request.Data.GetProperty("links").GetProperty("genius");
            if (request.ValidJsonData)
            {
                return $"{lyricsLink}";
            }
            else
            {
                return request.Result;
            }
        }
    }
}
