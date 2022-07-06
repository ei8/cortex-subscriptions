using System.Text.Json;

namespace ei8.Cortex.Subscriptions.IO.Http
{
    internal sealed class Constants
    {
        internal static readonly JsonSerializerOptions CamelCaseSerialization = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
