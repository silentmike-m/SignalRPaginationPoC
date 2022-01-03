namespace SignalRPoc.Client;

public sealed record SignalROptions
{
    public static readonly string SectionName = "SignalROptions";
    public string Url { get; set; } = string.Empty;
}
