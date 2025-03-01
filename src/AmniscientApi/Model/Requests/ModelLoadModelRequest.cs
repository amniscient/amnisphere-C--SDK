using System.Text.Json.Serialization;
using AmniscientApi.Core;

namespace AmniscientApi;

public record ModelLoadModelRequest
{
    /// <summary>
    /// Your organization identifier
    /// </summary>
    [JsonPropertyName("organization_id")]
    public required string OrganizationId { get; set; }

    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
