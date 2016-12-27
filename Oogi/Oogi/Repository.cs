using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Oogi.Query;
using Sushi;

namespace Oogi
{
    public class Repository<T> where T : BaseEntity
    {
        private readonly Connection _connection;

        public Repository()
        {            
            _connection = new Connection();
        }

        public Repository(string endpoint, string authorizationKey, string database, string collection)
        {
            _connection = new Connection(endpoint, authorizationKey, database, collection);
        }

        private async Task<T> GetFirstOrDefaultHelperAsync(IQuery<T> query = null)
        {
            SqlQuerySpec sqlq;

            if (query == null)
            {
                var qq = new SqlQuerySpecQuery<T>();
                sqlq = qq.ToGetFirstOrDefault();
            }
            else
            {
                sqlq = query.ToGetFirstOrDefault();
            }

            var q = _connection.Client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_connection.DatabaseId, _connection.CollectionId), sqlq).AsDocumentQuery();
            var response = await Core.ExecuteWithRetriesAsync(() => QuerySingleDocumentAsync(q));
            return response.AsEnumerable().FirstOrDefault();
        }

        /// <summary>
        /// Get first or default.
        /// </summary>
        public async Task<T> GetFirstOrDefaultAsync(SqlQuerySpec query = null)
        {
            return await GetFirstOrDefaultHelperAsync(new SqlQuerySpecQuery<T>(query));
        }

        /// <summary>
        /// Get first or default.
        /// </summary>
        public async Task<T> GetFirstOrDefaultAsync(DynamicQuery<T> query)
        {
            return await GetFirstOrDefaultHelperAsync(query);
        }

        /// <summary>
        /// Get first or default.
        /// </summary>
        public async Task<T> GetFirstOrDefaultAsync(string query, object parameters)
        {
            return await GetFirstOrDefaultHelperAsync(new DynamicQuery<T>(query, parameters));
        }

        /// <summary>
        /// Get first or default.
        /// </summary>
        public T GetFirstOrDefault(SqlQuerySpec query = null)
        {
            return AsyncTools.RunSync(() => GetFirstOrDefaultAsync(query));
        }

        /// <summary>
        /// Get first or default.
        /// </summary>
        public T GetFirstOrDefault(DynamicQuery<T> query)
        {
            return AsyncTools.RunSync(() => GetFirstOrDefaultAsync(query));
        }

        /// <summary>
        /// Get first or default.
        /// </summary>
        public T GetFirstOrDefault(string sql, object parameters)
        {
            return AsyncTools.RunSync(() => GetFirstOrDefaultAsync(new DynamicQuery<T>(sql, parameters)));
        }

        /// <summary>
        /// Get first or default.
        /// </summary>
        public async Task<T> GetFirstOrDefaultAsync(string id)
        {            
            return await GetFirstOrDefaultHelperAsync(new IdQuery<T>(id));
        }

        /// <summary>
        /// Get first or default.
        /// </summary>
        public T GetFirstOrDefault(string id)
        {
            return AsyncTools.RunSync(() => GetFirstOrDefaultAsync(id));
        }

        /// <summary>
        /// Upsert entity.
        /// </summary>
        public async Task<T> UpsertAsync(T entity)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => UpsertDocumentAsync(entity));
            return response;
        }

        /// <summary>
        /// Upsert entity.
        /// </summary>
        public T Upsert(T entity)
        {
            var ret = AsyncTools.RunSync(() => UpsertAsync(entity));
            return ret;
        }
		
        /// <summary>
        /// Create entity.
        /// </summary>
        public async Task<T> CreateAsync(T entity)
        {
            var ro = new RequestOptions();
            
            var response = await Core.ExecuteWithRetriesAsync(() => CreateDocumentAsync(entity, ro));
            return response;            
        }

        /// <summary>
        /// Create entity.
        /// </summary>
        public T Create(T entity)
        {
            var ret = AsyncTools.RunSync(() => CreateAsync(entity));
            return ret;
        }

        /// <summary>
        /// Replace entity.
        /// </summary>
        public async Task<T> ReplaceAsync(T entity)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => ReplaceDocumentAsync(entity));
            return response;
        }

        /// <summary>
        /// Replace entity.
        /// </summary>
        public T Replace(T entity)
        {
            var ret = AsyncTools.RunSync(() => ReplaceAsync(entity));
            return ret;
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        public async Task<T> DeleteAsync(string id)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => DeleteDocumentAsync(id));            
            return response;            
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        public T Delete(string id)
        {
            var en = AsyncTools.RunSync(() => DeleteAsync(id));
            return en;
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        public async Task<T> DeleteAsync(T entity)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => DeleteDocumentAsync(entity));
            return response;
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        public T Delete(T entity)
        {
            var en = AsyncTools.RunSync(() => DeleteAsync(entity.Id));
            return en;
        }

		/// <summary>
        /// Get list of all entities from query.
        /// </summary>        
        public async Task<List<T>> GetAllAsync()
        {            
            var query = new SqlQuerySpecQuery<T>();
		    var sq = query.ToGetAll();
            var q = _connection.Client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_connection.DatabaseId, _connection.CollectionId), sq).AsDocumentQuery();

            var response = await Core.ExecuteWithRetriesAsync(() => QueryMoreDocumentsAsync(q));
            return response;
        }

        /// <summary>
        /// Get list of all entities from query.
        /// </summary>        
        public List<T> GetAll()
        {
            var all = AsyncTools.RunSync(GetAllAsync);
            return all;
        }

        private async Task<List<T>> GetListHelperAsync(IQuery<T> query)
        {
            var sq = query.ToSqlQuerySpec();
            var q = _connection.Client.CreateDocumentQuery<T>(UriFactory.CreateDocumentCollectionUri(_connection.DatabaseId, _connection.CollectionId), sq).AsDocumentQuery();

            var response = await Core.ExecuteWithRetriesAsync(() => QueryMoreDocumentsAsync(q));
            return response;
        }

        /// <summary>
        /// Get list from query.
        /// </summary>        
        public async Task<List<T>> GetListAsync(SqlQuerySpec query)
        {
            return await GetListHelperAsync(new SqlQuerySpecQuery<T>(query));
        }

        /// <summary>
        /// Get list from query.
        /// </summary>        
        public async Task<List<T>> GetListAsync(DynamicQuery<T> query)
        {
            return await GetListHelperAsync(query);
        }

        /// <summary>
        /// Get list from query.
        /// </summary>        
        public async Task<List<T>> GetListAsync(string query, object parameters)
        {
            return await GetListHelperAsync(new DynamicQuery<T>(query, parameters));
        }

        /// <summary>
        /// Get list from query.
        /// </summary>        
        public List<T> GetList(SqlQuerySpec query)
        {
            return AsyncTools.RunSync(() => GetListAsync(query));
        }

        /// <summary>
        /// Get list from query.
        /// </summary>        
        public List<T> GetList(DynamicQuery<T> query)
        {
            return AsyncTools.RunSync(() => GetListAsync(query));
        }

        /// <summary>
        /// Get list from query.
        /// </summary>        
        public List<T> GetList(string query, object parameters)
        {
            return AsyncTools.RunSync(() => GetListAsync(new DynamicQuery<T>(query, parameters)));
        }

        private async Task<Paginator<T>> GetListHelperAsync(IQuery<T> query, int pageNumber, int pageSize)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (pageNumber == 0 ||
                pageSize == 0)
            {
                var res = await GetListAsync(query.ToSqlQuerySpec());
                return new Paginator<T>(null, res.Count, res);
            }

            string continuationToken = null;
            var total = 0;
            var result = new List<T>();
            var qq = query.ToSqlQuerySpec().ToSqlQuery();

            do
            {
                again:
                StoredProcedureResponse<Paginator<T>> response;

                try
                {
                    response = await Core.ExecuteWithRetriesAsync(() => _connection.Client.ExecuteStoredProcedureAsync<Paginator<T>>
                        (
                         UriFactory.CreateStoredProcedureUri(_connection.DatabaseId, _connection.CollectionId, "paginator"),
                            qq,
                            pageNumber,
                            pageSize,
                            continuationToken)
                        );
                }
                catch (DocumentClientException de)
                {
                    // getting rid of "The script with id 'blablabla' is blocked for execution because it has violated its allowed resource limit several times."
                    if (de.StatusCode != null &&
                        ((int)de.StatusCode == 403))
                    {
                        var sp = await _connection.ReadStoredProcedureAsync("paginator");
                        await _connection.DeleteStoredProcedureAsync("paginator");
                        await _connection.CreateStoredProcedureAsync(sp);
                        goto again;
                    }

                    throw;
                }

                continuationToken = response.Response.ContinuationToken;
                var col = response.Response.Result;

                result.AddRange(col);

                total += response.Response.Total;
            }
            while (continuationToken != null);

            return new Paginator<T>(null, total, result);
        }

        /// <summary>
        /// Get list with paging.
        /// </summary>        
        public async Task<Paginator<T>> GetListAsync(SqlQuerySpec query, int pageNumber, int pageSize)
        {
            return await GetListHelperAsync(new SqlQuerySpecQuery<T>(query), pageNumber, pageSize);
        }

        /// <summary>
        /// Get list with paging.
        /// </summary>        
        public async Task<Paginator<T>> GetListAsync(DynamicQuery<T> query, int pageNumber, int pageSize)
        {
            return await GetListHelperAsync(query, pageNumber, pageSize);
        }

        /// <summary>
        /// Get list with paging.
        /// </summary>        
        public async Task<Paginator<T>> GetListAsync(string query, object parameters, int pageNumber, int pageSize)
        {
            return await GetListHelperAsync(new DynamicQuery<T>(query, parameters), pageNumber, pageSize);
        }

        /// <summary>
        /// Get list with paging.
        /// </summary>        
        public Paginator<T> GetList(SqlQuerySpec query, int pageNumber, int pageSize)
        {
            return AsyncTools.RunSync(() => GetListAsync(query, pageNumber, pageSize));
        }

        /// <summary>
        /// Get list with paging.
        /// </summary>        
        public Paginator<T> GetList(DynamicQuery<T> query, int pageNumber, int pageSize)
        {
            return AsyncTools.RunSync(() => GetListAsync(query, pageNumber, pageSize));
        }

        /// <summary>
        /// Get list with paging.
        /// </summary>        
        public Paginator<T> GetList(string query, object parameters, int pageNumber, int pageSize)
        {
            return AsyncTools.RunSync(() => GetListAsync(new DynamicQuery<T>(query, parameters), pageNumber, pageSize));
        }

        private static async Task<FeedResponse<T>> QuerySingleDocumentAsync(IDocumentQuery<T> query)
        {
            return await query.ExecuteNextAsync<T>();
        }
        
        private async Task<T> CreateDocumentAsync(T entity, RequestOptions requestOptions)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => _connection.Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_connection.DatabaseId, _connection.CollectionId), entity, requestOptions));
            var ret = (T)(dynamic)response.Resource;
            return ret;
        }

        private async Task<T> ReplaceDocumentAsync(T entity)
        {            
            var response = await Core.ExecuteWithRetriesAsync(() => _connection.Client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_connection.DatabaseId, _connection.CollectionId, entity.Id), entity));
            var ret = (T)(dynamic)response.Resource;
            return ret;
        }
		
		private async Task<T> UpsertDocumentAsync(T entity)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => _connection.Client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(_connection.DatabaseId, _connection.CollectionId), entity));
            var ret = (T)(dynamic)response.Resource;
            return ret;
        }

        private async Task<T> DeleteDocumentAsync(T entity)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => _connection.Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_connection.DatabaseId, _connection.CollectionId, entity.Id)));
            var ret = (T)(dynamic)response.Resource;
            return ret;
        }

        private async Task<T> DeleteDocumentAsync(string id)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => _connection.Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_connection.DatabaseId, _connection.CollectionId, id)));
            var ret = (T)(dynamic)response.Resource;
            return ret;
        }

        private static async Task<List<T>> QueryMoreDocumentsAsync(IDocumentQuery<T> query)
        {
            var entitiesRetrieved = new List<T>();
            
            while (query.HasMoreResults)
            {
                var queryResponse = await Core.ExecuteWithRetriesAsync(() => QuerySingleDocumentAsync(query));                
                
                var entities = queryResponse.AsEnumerable();

                if (entities != null)
                    entitiesRetrieved.AddRange(entities);
            }

            return entitiesRetrieved;
        }
    }
}
