using System.Text.Json.Serialization;

namespace TodoApi.BuildingBlocks.Core;

public class ErrorResult
{
    public ErrorResult(string requestId, string errorType, string[] errorCodes = null)
    {
        RequestId = requestId;
        ErrorCodes = errorCodes;
        ErrorType = errorType;
    }
    [JsonPropertyName("request_id")]
    public  string RequestId { get; }
    [JsonPropertyName("error_type")]
    public string ErrorType { get; }
    [JsonPropertyName("error_codes")]
    public IEnumerable<string> ErrorCodes { get; }
}