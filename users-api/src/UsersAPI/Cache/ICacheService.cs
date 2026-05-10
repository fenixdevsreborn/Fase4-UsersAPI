namespace UsersAPI.Cache;

public interface ICacheService
{
  Task<string> GetOrCreateAsync(string key);
}