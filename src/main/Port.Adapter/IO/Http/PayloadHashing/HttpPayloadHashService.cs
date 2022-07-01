using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Cortex.Subscriptions.IO.Http.PayloadHashing
{
    public class HttpPayloadHashService : IPayloadHashService
    {
        private readonly AsyncRetryPolicy exponentialRetry;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<HttpPayloadHashService> logger;

        public HttpPayloadHashService(IHttpClientFactory httpClientFactory, ILogger<HttpPayloadHashService> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.logger = logger;
            exponentialRetry = Policy.Handle<Exception>()
                                          .WaitAndRetryAsync(
                                              3,
                                              attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)),
                                              (ex, _) => this.logger.LogError(ex, "Error occurred while communicating with ei8 Cortex Subscriptions: {Message}", ex.InnerException?.Message)
                                          );
        }

        public async Task<string> GetPayloadHashAsync(string url)
        {
            var payload = await GetPayloadAsync(url);
            return HashPayload(payload);
        }

        private async Task<string> GetPayloadAsync(string url)
        {
            using (var client = httpClientFactory.CreateClient())
            {
                var response = await exponentialRetry.ExecuteAsync(async () =>
                {
                    return await client.GetAsync(url);
                });

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();

                else
                    throw new Exception($"HTTP error when querying ${url}: ${response.StatusCode}");
            }
        }

        private string HashPayload(string payload)
        {
            var algorithm = new SHA256Managed();
            var hashString = new StringBuilder();

            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(payload));

            // Get hex representation of each byte from the hash value
            foreach (var b in hash)
            {
                hashString.Append(b.ToString("x2"));
            }

            return hashString.ToString();
        }
    }
}