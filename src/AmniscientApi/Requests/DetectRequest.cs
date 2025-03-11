using AmniscientApi.Core;

namespace AmniscientApi;

public record DetectRequest
{
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
