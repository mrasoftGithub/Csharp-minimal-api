namespace MinimalAPI;

public class Fout
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
}
