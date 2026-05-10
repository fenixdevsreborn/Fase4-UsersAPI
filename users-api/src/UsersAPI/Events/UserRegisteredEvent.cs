namespace Events;

public class UserRegisteredEvent
{
  public string EventType => "USER_REGISTERED";

  public string UserId { get; set; } = string.Empty;

  public string Email { get; set; } = string.Empty;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
