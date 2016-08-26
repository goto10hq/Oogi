using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Oogi
{
    public static class Tools
    {
        /// <summary>
        /// Set Json default settings.
        /// </summary>
        public static void SetJsonDefaultSettings()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.None,
                TypeNameHandling = TypeNameHandling.None,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }
    }
}
