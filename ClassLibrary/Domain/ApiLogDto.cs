using System.Text.Json.Serialization;

public class ApiLogDto
{
    [JsonPropertyName("log_id")]
    public int logId { get; set; }
    [JsonPropertyName("user_id")]
    public int userId { get; set; }
    [JsonPropertyName("action_type")]
    public string actionType { get; set; }
    [JsonPropertyName("timestamp")]
    public DateTime timestamp { get; set; }
}