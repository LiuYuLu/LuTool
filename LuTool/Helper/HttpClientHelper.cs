using System.Net.Http;

namespace LuTool
{
    public sealed class HttpClientHelper
    {
        private HttpClientHelper()
        {
        }

        public static readonly HttpClient Client;

        static HttpClientHelper()
        {
            Client = new HttpClient();
        }
    }
}