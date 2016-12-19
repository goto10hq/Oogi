using Microsoft.Azure.Documents;

namespace Oogi.Query
{
    public interface IQuery<T> where T : BaseEntity
    {
        SqlQuerySpec ToSqlQuerySpec();
        SqlQuerySpec ToGetFirstOrDefault();
        SqlQuerySpec ToGetAll();
    }
}
