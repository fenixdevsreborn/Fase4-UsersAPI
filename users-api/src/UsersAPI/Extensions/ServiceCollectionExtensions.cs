using Amazon.DynamoDBv2;
using Amazon.SQS;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Messaging;
using Repositories;
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

    services.AddControllers();

    services.AddHttpContextAccessor();

    services.AddScoped<ICacheService, RedisCacheService>();

    services.AddScoped<UserRepository>();

    services.AddScoped<EventPublisher>();

    services.AddScoped<UserService>();

    services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration =
          configuration["Redis:Connection"];
    });

    services.AddAWSService<IAmazonDynamoDB>();

    services.AddAWSService<IAmazonSQS>();

    return services;
  }

  public static IServiceCollection AddApplicationHealthChecks(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddHealthChecks()

        .AddCheck(
            "self",
            () => HealthCheckResult.Healthy(),
            tags: new[] { "live" })

        .AddRedis(
            configuration["Redis:Connection"]!,
            name: "redis",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "ready" });

    return services;
  }
}