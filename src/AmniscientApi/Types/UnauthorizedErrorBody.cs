using System.Text.Json.Serialization;
using AmniscientApi.Core;

namespace AmniscientApi;

public record UnauthorizedErrorBody
{
    /// <summary>
    /// A status code denoting success or failure.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// A message describing your error.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
