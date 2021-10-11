using HLE.HttpRequests;

namespace ApuDoingStuff.API
{
    public class HTTPRequest
    {
        private const string _randomShibaUrl = "http://shibe.online/api/shibes";
        private const string _randomDuckUrl = "https://random-d.uk/api/random";
        private const string _randomCatUrl = "https://aws.random.cat/meow";
        private const string _randomDogUrl = "https://random.dog/woof.json";

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
    }
}
