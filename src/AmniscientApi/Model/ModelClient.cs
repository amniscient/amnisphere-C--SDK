using System.Net.Http;
using System.Text.Json;
using System.Threading;
using AmniscientApi.Core;

namespace AmniscientApi;

public partial class ModelClient
{
    private RawClient _client;

    internal ModelClient(RawClient client)
    {
        _client = client;
    }

    /// <summary>
    /// Initializes a model for inference. This endpoint must be called before running any detections.
    /// </summary>
    /// <example>
    /// <code>
    /// await client.Model.LoadModelAsync(
    ///     "model_id",
    ///     new ModelLoadModelRequest { OrganizationId = "organization_id" }
    /// );
    /// </code>
    /// </example>
    public async Task<ModelLoadModelResponse> LoadModelAsync(
        string modelId,
        ModelLoadModelRequest request,
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
                return JsonUtils.Deserialize<ModelLoadModelResponse>(responseBody)!;
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
}
