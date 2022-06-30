using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.IO.Http
{
    public interface IPayloadHashService
    {
        Task<string> GetPayloadHashAsync(string url);
    }
}
