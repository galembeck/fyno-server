using System.Text.Json;

namespace Domain.Data.Models.Util;

public class ErrorModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Data { get; set; }
    public int? Status { get; set; }
    public ValidationErrorModel Error { get; set; }

    public string ToJson()
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };

        return JsonSerializer.Serialize(this, options);
    }
}