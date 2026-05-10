using Amazon.DynamoDBv2.DataModel;

[DynamoDBTable("Users")]
public class Users
{
  [DynamoDBHashKey]
  public required string Id { get; set; }
  public string Email { get; set; } = string.Empty;     
  public string Nickname { get; set; } = string.Empty;
  public string Name { get; set; } = string.Empty;
  public bool Active { get; set; } = true;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; set; }
}