using Newtonsoft.Json;

namespace Oogi.Tokens
{
    public class ConnectionString
    {
        [JsonProperty("endPoint")]
        public string Endpoint { get; set; }

        [JsonProperty("authorizationKey")]
        public string AuthorizationKey { get; set; }

        [JsonProperty("database")]
        public string Database { get; set; }

        [JsonProperty("collection")]
        public string Collection { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public ConnectionString()
        {            
        }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="authorizationKey">Authorization key.</param>
        /// <param name="database">Database.</param>
        /// <param name="collection">Collection.</param>
        public ConnectionString(string endpoint, string authorizationKey, string database, string collection)
        {
            Endpoint = endpoint;
            AuthorizationKey = authorizationKey;
            Database = database;
            Collection = collection;
        }
    }
}
