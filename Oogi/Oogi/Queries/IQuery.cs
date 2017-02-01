using Microsoft.Azure.Documents;

namespace Oogi.Queries
{
    public interface IQuery<T> where T : BaseEntity
    {
        SqlQuerySpec ToSqlQuerySpec();
        SqlQuerySpec ToGetFirstOrDefault();
        SqlQuerySpec ToGetAll();
    }
}
