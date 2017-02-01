using Microsoft.Azure.Documents;

namespace Oogi.Queries
{
    public interface IQuery
    {
        SqlQuerySpec ToSqlQuerySpec();
        SqlQuerySpec ToGetFirstOrDefault();
        SqlQuerySpec ToGetAll();
    }
}
