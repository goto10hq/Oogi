using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using Oogi.Tokens;
using Sushi2;

namespace Oogi
{
    public class Connection
    {
        public DocumentClient Client { get; private set; }

        public string DatabaseId { get; private set; }
        public string CollectionId { get; private set; }

        static Connection()
        {
            Tools.SetJsonDefaultSettings();    
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        public Connection(ConnectionString connectionString) 
            : this(connectionString.Endpoint, connectionString.AuthorizationKey, connectionString.Database, connectionString.Database)
        {
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        public Connection(string endpoint, string authorizationKey, string database, string collection)
        {
            var connectionPolicy = new ConnectionPolicy
                                   {
                                       UserAgentSuffix = "Oogi",
                                       ConnectionMode = ConnectionMode.Direct,
                                       ConnectionProtocol = Protocol.Tcp
                                   };

            Client = new DocumentClient(new Uri(endpoint), authorizationKey, connectionPolicy);                     
            DatabaseId = database;
            CollectionId = collection;
        }
        
        /// <summary>
        /// Upsert document(s) as pure json.
        /// </summary>
        public List<object> UpsertJson(string jsonString)
        {
            return AsyncTools.RunSync(() => UpsertJsonAsync(jsonString));
        }

        /// <summary>
        /// Upsert document(s) as pure json.
        /// </summary>
        public async Task<List<object>> UpsertJsonAsync(string jsonString)
        {
            if (jsonString == null)
                throw new ArgumentNullException(nameof(jsonString));

            var result = new List<object>();
            var docs = JsonConvert.DeserializeObject<List<object>>(jsonString);

            foreach (var doc in docs)
            {
                result.Add(await Core.ExecuteWithRetriesAsync(() => Client.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), doc)));
            }

            return result;
        }

        /// <summary>
        /// Execute query.
        /// </summary>
        public async Task<object> ExecuteQueryAsync(string query)
        {            
            var q = Client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), query).AsDocumentQuery();            
            var result = await Core.ExecuteWithRetriesAsync(() => QueryMoreDocumentsAsync(q));

            return result;
        }

        /// <summary>
        /// Execute query.
        /// </summary>
        public object ExecuteQuery(string query)
        {                        
            var result = AsyncTools.RunSync(() => ExecuteQueryAsync(query));

            return result;
        }

        /// <summary>
        /// Create store procedure.
        /// </summary>
        public async Task<StoredProcedure> CreateStoredProcedureAsync(StoredProcedure sp)
        {         
            StoredProcedure createdStoredProcedure = await Client.CreateStoredProcedureAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), sp);
            return createdStoredProcedure;
        }

        /// <summary>
        /// Read store procedure.
        /// </summary>
        public async Task<StoredProcedure> ReadStoredProcedureAsync(string storeProcedureId)
        {            
            StoredProcedure storedProcedure = await Client.ReadStoredProcedureAsync(UriFactory.CreateStoredProcedureUri(DatabaseId, CollectionId, storeProcedureId));
            return storedProcedure;
        }

        /// <summary>
        /// Read store procedure.
        /// </summary>
        public StoredProcedure ReadStoredProcedure(string storeProcedureId)
        {
            StoredProcedure storedProcedure = AsyncTools.RunSync(() => Client.ReadStoredProcedureAsync(UriFactory.CreateStoredProcedureUri(DatabaseId, CollectionId, storeProcedureId)));
            return storedProcedure;
        }

        /// <summary>
        /// Delete store procedure.
        /// </summary>
        public async Task<StoredProcedure> DeleteStoredProcedureAsync(string storeProcedureId)
        {
            StoredProcedure storedProcedure = await Client.DeleteStoredProcedureAsync(UriFactory.CreateStoredProcedureUri(DatabaseId, CollectionId, storeProcedureId));
            return storedProcedure;
        }

        /// <summary>
        /// Delete store procedure.
        /// </summary>
        public StoredProcedure DeleteStoredProcedure(string storeProcedureId)
        {
            StoredProcedure storedProcedure = AsyncTools.RunSync(() => Client.DeleteStoredProcedureAsync(UriFactory.CreateStoredProcedureUri(DatabaseId, CollectionId, storeProcedureId)));
            return storedProcedure;
        }

        /// <summary>
        /// Create store procedure.
        /// </summary>
        public StoredProcedure CreateStoredProcedure(StoredProcedure sp)
        {
            return AsyncTools.RunSync(() => CreateStoredProcedureAsync(sp));
        }

        /// <summary>
        /// Query cube.
        /// </summary>
        public async Task<IList<Tuple<string, int>>> GetUniqueValuesAsync(string field, string filterQuery)
        {
            var cubeConfigString = $"{{ cubeConfig: {{ groupBy: '{field}',  field: '{field}', f: 'uniqueValues' }}, filterQuery: '{filterQuery}'}}";

            string continuationToken = null;

            dynamic cubeConfig = JsonConvert.DeserializeObject<dynamic>(cubeConfigString);

            do
            {
                again:
                StoredProcedureResponse<dynamic> result;

                try
                {
                    var token = continuationToken;
                    dynamic config = cubeConfig;

                    result = await Core.ExecuteWithRetriesAsync<StoredProcedureResponse<dynamic>>(() => Client.ExecuteStoredProcedureAsync<dynamic>
                        (
                            UriFactory.CreateStoredProcedureUri(DatabaseId, CollectionId, "cube"), 
                            config, 
                            token)
                        );
                }
                catch (DocumentClientException de)
                {
                    // getting rid of "The script with id 'blablabla' is blocked for execution because it has violated its allowed resource limit several times."
                    if (de.StatusCode != null &&
                        ((int)de.StatusCode == 403))
                    {
                        var sp = await ReadStoredProcedureAsync("cube");
                        await DeleteStoredProcedureAsync("cube");
                        await CreateStoredProcedureAsync(sp);                        
                        goto again;
                    }

                    throw;
                }

                cubeConfig = result.Response;
                continuationToken = cubeConfig.continuation;
            }
            while (continuationToken != null);

            var cube = (CubeConfig)JsonConvert.DeserializeObject<CubeConfig>(cubeConfig.ToString());
            var vals = cube.SavedCube.CellsAsCsvStyleArray.ToList().Skip(1);
            return vals.Select(v => new Tuple<string, int>(v[0].ToString(), v[1].ToInt32() ?? 0)).ToList();
        }

        /// <summary>
        /// Query cube.
        /// </summary>
        public IList<Tuple<string, int>> GetUniqueValues(string field, string filterQuery)
        {
            return AsyncTools.RunSync(() => GetUniqueValuesAsync(field, filterQuery));
        }

        private static async Task<List<dynamic>> QueryMoreDocumentsAsync(IDocumentQuery<dynamic> query)
        {
            var entitiesRetrieved = new List<dynamic>();

            while (query.HasMoreResults)
            {
                var queryResponse = await Core.ExecuteWithRetriesAsync(() => QuerySingleDocumentAsync(query));

                var entities = queryResponse.AsEnumerable();

                if (entities != null)
                    entitiesRetrieved.AddRange(entities);
            }

            return entitiesRetrieved;
        }

        private static async Task<FeedResponse<dynamic>> QuerySingleDocumentAsync(IDocumentQuery<dynamic> query)
        {
            return await query.ExecuteNextAsync<dynamic>();
        }

        /// <summary>
        /// Delete document by id.
        /// </summary>
        public async Task<object> DeleteAsync(string id)
        {
            var response = await Core.ExecuteWithRetriesAsync(() => Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id)));
            return response;
        }
    }
}
