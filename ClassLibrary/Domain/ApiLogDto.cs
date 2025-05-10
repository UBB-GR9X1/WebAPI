using System.Text.Json.Serialization;

public class ApiLogDto
{
    [JsonPropertyName("log_id")]
    public int log_id { get; set; }
    [JsonPropertyName("user_id")]
    public int user_id { get; set; }
    [JsonPropertyName("action_type")]
    public string action_type { get; set; }
    [JsonPropertyName("timestamp")]
    public DateTime timestamp { get; set; }
}