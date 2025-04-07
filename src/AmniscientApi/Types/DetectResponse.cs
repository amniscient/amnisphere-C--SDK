using System.Text.Json;
using System.Text.Json.Serialization;
using AmniscientApi.Core;

namespace AmniscientApi;

public record DetectResponse
{
    /// <summary>
    /// A status code denoting success or failure.
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    /// The detections response includes 3 elements: the bbox array, a confidence value, and a class. The bbox array is an array of numbers describing the bounding box coordinates for your objects listed in order as [x1, y1, x2, y2]. The confidence score is a value between 0 and 1 rating how confident the object detection output is based on your model and the image provided. The class is the class name of the detected object.
    /// </summary>
    [JsonPropertyName("detections")]
    public IEnumerable<object>? Detections { get; set; }

    /// <summary>
    /// Additional properties received from the response, if any.
    /// </summary>
    /// <remarks>
    /// [EXPERIMENTAL] This API is experimental and may change in future releases.
    /// </remarks>
    [JsonExtensionData]
    public IDictionary<string, JsonElement> AdditionalProperties { get; internal set; } =
        new Dictionary<string, JsonElement>();

    /// <inheritdoc />
    public override string ToString()
    {
        return JsonUtils.Serialize(this);
    }
}
