using System.Collections.Generic;
using Newtonsoft.Json;

namespace Oogi
{
    public class Paginator<T>
    {
        [JsonIgnore]
        public string ContinuationToken { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "items")]
        public List<T> Result { get; set; }

        public Paginator(string continuationToken, int total, List<T> result)
        {
            ContinuationToken = continuationToken;
            Total = total;
            Result = result;
        }
    }
}
