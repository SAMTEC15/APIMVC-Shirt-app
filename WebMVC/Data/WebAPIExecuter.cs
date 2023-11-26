namespace WebMVC.Data
{
    public class WebAPIExecuter : IWebAPIExecuter
    {
        private const string apiName = "ShirtsAPI";
        private readonly IHttpClientFactory _httpClientFactory;

        public WebAPIExecuter(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = _httpClientFactory.CreateClient(apiName);
            return await httpClient.GetFromJsonAsync<T>(relativeUrl);
        }
    }
}
