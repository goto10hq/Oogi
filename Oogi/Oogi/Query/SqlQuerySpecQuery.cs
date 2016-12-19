using Microsoft.Azure.Documents;

namespace Oogi.Query
{
    public class SqlQuerySpecQuery<T> : IQuery<T> where T : BaseEntity
    {
        private readonly SqlQuerySpec _sqlQuerySpec;

        public SqlQuerySpecQuery()
        {            
        }

        public SqlQuerySpecQuery(SqlQuerySpec sqlQuerySpec)
        {
            _sqlQuerySpec = sqlQuerySpec;                        
        }
        
        public SqlQuerySpec ToSqlQuerySpec()
        {
            return _sqlQuerySpec;
        }

        public SqlQuerySpec ToGetFirstOrDefault()
        {
            return new SqlQuerySpec("select top 1 * from c where c.entity = @entity",
                new SqlParameterCollection
                {
                    new SqlParameter("@entity", Core.ToEntity<T>())
                });
        }

        public SqlQuerySpec ToGetAll()
        {
            return new SqlQuerySpec("select * from c where c.entity = @entity",
                new SqlParameterCollection
                {
                    new SqlParameter("@entity", Core.ToEntity<T>())
                });
        }
    }
}
