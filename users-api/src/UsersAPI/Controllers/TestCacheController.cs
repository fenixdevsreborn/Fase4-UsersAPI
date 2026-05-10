using Microsoft.AspNetCore.Mvc;
using UsersAPI.Cache;

namespace UsersAPI.Controllers;

[ApiController]
[Route("cache")]
public class TestCacheController : ControllerBase
{
  private readonly ICacheService _cacheService;

  public TestCacheController(
      ICacheService cacheService)
  {
    _cacheService = cacheService;
  }

  [HttpGet]
  public async Task<IActionResult> Get()
  {
    var result =
        await _cacheService.GetOrCreateAsync(
            "users-api-cache");

    return Ok(new
    {
      value = result
    });
  }
}