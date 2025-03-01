using AmniscientApi.Core;

namespace AmniscientApi;

public partial class AmniscientApiClient
{
    private readonly RawClient _client;

    public AmniscientApiClient(string apiKey, ClientOptions? clientOptions = null)
    {
        var defaultHeaders = new Headers(
            new Dictionary<string, string>()
            {
                { "x-api-key", apiKey },
                { "X-Fern-Language", "C#" },
                { "X-Fern-SDK-Name", "AmniscientApi" },
                { "X-Fern-SDK-Version", Version.Current },
            }
        );
        clientOptions ??= new ClientOptions();
        foreach (var header in defaultHeaders)
        {
            if (!clientOptions.Headers.ContainsKey(header.Key))
            {
                clientOptions.Headers[header.Key] = header.Value;
            }
        }
        _client = new RawClient(clientOptions);
        Model = new ModelClient(_client);
        Detection = new DetectionClient(_client);
    }

    public ModelClient Model { get; init; }

    public DetectionClient Detection { get; init; }
}
