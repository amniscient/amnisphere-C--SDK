using System.Text.Json.Serialization;
using AmniscientApi.Core;

namespace AmniscientApi;

public record LoadModelResponse
{
    /// <summary>
    /// A status code denoting success or failure.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    /// The model ID of the model you loaded. This should match the model ID that was input as a path parameter to this API.
    /// </summary>
    [JsonPropertyName("model_id")]
    public string? ModelId { get; set; }

    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
