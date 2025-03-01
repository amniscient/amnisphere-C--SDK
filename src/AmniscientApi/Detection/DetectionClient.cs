using System.Net.Http;
using System.Text.Json;
using System.Threading;
using AmniscientApi.Core;

namespace AmniscientApi;

public partial class DetectionClient
{
    private RawClient _client;

    internal DetectionClient(RawClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Detects an object within an uploaded image file. Make sure to load the model you're using for detection first!
    /// </summary>
    public async Task<DetectionDetectResponse> DetectAsync(
        DetectionDetectRequest request,
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
                return JsonUtils.Deserialize<DetectionDetectResponse>(responseBody)!;
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
