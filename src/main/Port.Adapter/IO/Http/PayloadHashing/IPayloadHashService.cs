using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.IO.Http.PayloadHashing
{
    public interface IPayloadHashService
    {
        Task<string> GetPayloadHashAsync(string url);
    }
}
