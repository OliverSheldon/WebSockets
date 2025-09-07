namespace WebSockets.Models
{
    public class ApiClient : IApiClient
    {
        private readonly IConfiguration _config;
        private HttpClient _client;
        public ApiClient(IConfiguration config)
        {
            _config = config;

            _client = new HttpClient();

            _client.BaseAddress = new Uri(_config.GetSection("TimingApi").GetValue<string>("Endpoint"));
        }

        public async Task<string> GetSessions()
        {
            return await _client.GetStringAsync("sessions");
        }
    }
}
