using System.Text.Json.Serialization;

public class ApiLogDto
{
    [JsonPropertyName("logId")]
    public int _log_id { get; set; }
    [JsonPropertyName("userId")]
    public int _user_id { get; set; }
    [JsonPropertyName("actionType")]
    public string _action_type { get; set; }
    [JsonPropertyName("timestamp")]
    public DateTime _timestamp { get; set; }
}