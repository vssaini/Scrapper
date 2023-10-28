using System.Data;

namespace Scrapper.Contracts.Data;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}