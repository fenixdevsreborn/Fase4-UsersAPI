using Amazon.DynamoDBv2.DataModel;

namespace Models
{
  [DynamoDBTable("AuditLogs")]
  public class AuditLog
  {
    [DynamoDBHashKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string TableName { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
  }
}
