using AmniscientApi.Core;

namespace AmniscientApi;

public record DetectRequest
{
    /// <summary>
    /// Your organization identifier
    /// </summary>
    public required string OrganizationId { get; set; }

    public required object File { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
