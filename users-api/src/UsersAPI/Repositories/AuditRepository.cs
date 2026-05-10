using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Models;

namespace Repositories
{
  public class AuditRepository
  {
    private readonly DynamoDBContext _context;

    public AuditRepository()
    {
      var client = new AmazonDynamoDBClient();
      _context = new DynamoDBContext(client);
    }

    public async Task Log(AuditLog log)
    {
      await _context.SaveAsync(log);
    }
  }
}
