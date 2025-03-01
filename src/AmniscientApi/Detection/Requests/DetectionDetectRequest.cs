using AmniscientApi.Core;

namespace AmniscientApi;

public record DetectionDetectRequest
{
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
