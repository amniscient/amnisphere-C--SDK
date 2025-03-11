using System.Net.Http;
using System.Text.Json;
using System.Threading;
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
                { "User-Agent", "Imdb.Net/0.0.1" },
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
    }

    /// <summary>
    /// Initializes a model for inference. This endpoint must be called before running any detections.
    /// </summary>
    /// <example>
    /// <code>
    /// await client.LoadModelAsync(
    ///     "model_id",
    ///     new LoadModelRequest { OrganizationId = "organization_id" }
    /// );
    /// </code>
    /// </example>
    public async Task<LoadModelResponse> LoadModelAsync(
        string modelId,
        LoadModelRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .MakeRequestAsync(
                new RawClient.JsonApiRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Post,
                    Path = $"loadModel/{JsonUtils.SerializeAsString(modelId)}",
                    Body = request,
                    ContentType = "application/json",
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        var responseBody = await response.Raw.Content.ReadAsStringAsync();
        if (response.StatusCode is >= 200 and < 400)
        {
            try
            {
                return JsonUtils.Deserialize<LoadModelResponse>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new AmniscientApiException("Failed to deserialize response", e);
            }
        }

        try
        {
            switch (response.StatusCode)
            {
                case 400:
                    throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
                case 401:
                    throw new UnauthorizedError(
                        JsonUtils.Deserialize<UnauthorizedErrorBody>(responseBody)
                    );
            }
        }
        catch (JsonException)
        {
            // unable to map error response, throwing generic error
        }
        throw new AmniscientApiApiException(
            $"Error with status code {response.StatusCode}",
            response.StatusCode,
            responseBody
        );
    }

    /// <summary>
    /// Detects an object within an uploaded image file. Make sure to load the model you're using for detection first!
    /// </summary>
    public async Task<DetectResponse> DetectAsync(
        DetectRequest request,
        RequestOptions? options = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _client
            .MakeRequestAsync(
                new RawClient.JsonApiRequest
                {
                    BaseUrl = _client.Options.BaseUrl,
                    Method = HttpMethod.Post,
                    Path = "detect",
                    Options = options,
                },
                cancellationToken
            )
            .ConfigureAwait(false);
        var responseBody = await response.Raw.Content.ReadAsStringAsync();
        if (response.StatusCode is >= 200 and < 400)
        {
            try
            {
                return JsonUtils.Deserialize<DetectResponse>(responseBody)!;
            }
            catch (JsonException e)
            {
                throw new AmniscientApiException("Failed to deserialize response", e);
            }
        }

        try
        {
            switch (response.StatusCode)
            {
                case 400:
                    throw new BadRequestError(JsonUtils.Deserialize<object>(responseBody));
            }
        }
        catch (JsonException)
        {
            // unable to map error response, throwing generic error
        }
        throw new AmniscientApiApiException(
            $"Error with status code {response.StatusCode}",
            response.StatusCode,
            responseBody
        );
    }
}
