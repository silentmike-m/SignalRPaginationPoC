namespace SignalRPoc.Server.Application.Common;

using System.Text.Json.Serialization;

public class BaseResponse<T>
{
    [JsonPropertyName("code")] public string Code { get; init; } = "ok";
    [JsonPropertyName("error")] public string? Error { get; init; } = default;
    [JsonPropertyName("response")] public T? Response { get; set; } = default;

    public BaseResponse()
    {

    }

    public BaseResponse(T response)
    {
        this.Response = response;
    }
}
