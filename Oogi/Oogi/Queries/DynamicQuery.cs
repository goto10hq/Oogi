using Microsoft.Azure.Documents;

namespace Oogi.Queries
{
    public class DynamicQuery<T> : IQuery<T> where T : BaseEntity
    {
        private readonly object _parameters;
        private readonly string _sql;
        private SqlQuerySpec _sqlQuerySpec;

        private SqlQuerySpec SqlQuerySpec => _sqlQuerySpec ?? (_sqlQuerySpec = ConvertToSqlQuerySpec(_sql, _parameters));

        public DynamicQuery()
        {
        }

        public DynamicQuery(string sql, object parameters = null)
        {
            _sql = sql;
            _parameters = parameters;                      
        }

        private static SqlQuerySpec ConvertToSqlQuerySpec(string sql, object parameters)
        {            
            if (string.IsNullOrEmpty(sql))
                return null;

            var sqlqs = new SqlQuerySpec(sql);

            if (parameters == null)
                return sqlqs;
                        
            var sqlParameters = Tools.AnonymousObjectToSqlParameters(parameters);

            if (sqlParameters == null)
                return sqlqs;

            return new SqlQuerySpec(sql, sqlParameters);            
        }       

        public SqlQuerySpec ToSqlQuerySpec()
        {
            return SqlQuerySpec;
        }

        public SqlQuerySpec ToGetFirstOrDefault()
        {
            if (SqlQuerySpec == null)
            {
                return new SqlQuerySpec("select top 1 * from c where c.entity = @entity",
                    new SqlParameterCollection
                    {
                        new SqlParameter("@entity", Core.ToEntity<T>())
                    });
            }

            return SqlQuerySpec;
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
