using Amazon.DynamoDBv2;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Services;
using UsersAPI.Cache;
using UsersAPI.Configurations;

namespace UsersAPI.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplicationServices(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.Configure<RedisConfiguration>(
        configuration.GetSection("Redis"));

    services.Configure<AwsConfiguration>(
        configuration.GetSection("AWS"));

    services.AddScoped<ICacheService, RedisCacheService>();
    services.AddScoped<UserService>();

    services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration =
          configuration["Redis:Connection"];
    });

    services.AddAWSService<IAmazonDynamoDB>();

    return services;
  }

  public static IServiceCollection AddApplicationHealthChecks(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddHealthChecks()
        .AddCheck("self",
            () => HealthCheckResult.Healthy())

        .AddRedis(
            configuration["Redis:Connection"]!,
            name: "redis",
            failureStatus: HealthStatus.Unhealthy);

    return services;
  }
}