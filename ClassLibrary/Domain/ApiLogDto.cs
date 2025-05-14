using System.Text.Json.Serialization;

public class ApiLogDto
{
    [JsonPropertyName("logId")]
    public int logId { get; set; }
    [JsonPropertyName("userId")]
    public int userId { get; set; }
    [JsonPropertyName("actionType")]
    public string actionType { get; set; }
    [JsonPropertyName("timestamp")]
    public DateTime timestamp { get; set; }
}