namespace WebSockets.Models
{
    public interface IApiClient
    {
        public Task<string> GetSessions();
    }
}
