using Microsoft.Azure.Documents;

namespace Oogi.Queries
{
    public class IdQuery<T> : IQuery where T : BaseEntity
    {
        private readonly string _id;                       

        public IdQuery(string id)
        {            
            _id = id;
        }

        public SqlQuerySpec ToSqlQuerySpec()
        {
            var entity = Core.ToEntity<T>();
            return new SqlQuerySpec("select * from c where c.entity = @entity and c.id = @id", new SqlParameterCollection { new SqlParameter("@entity", entity), new SqlParameter("@id", _id) });
        }

        public SqlQuerySpec ToGetFirstOrDefault()
        {
            var entity = Core.ToEntity<T>();
            return new SqlQuerySpec("select top 1 * from c where c.entity = @entity and c.id = @id", new SqlParameterCollection { new SqlParameter("@entity", entity), new SqlParameter("@id", _id) });
        }

        public SqlQuerySpec ToGetAll()
        {
            throw new System.NotImplementedException();
        }
    }    
}
