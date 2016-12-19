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

        /// <summary>
        /// Escape value.
        /// </summary>
        public static string ToEscapedString(this string value)
        {
            value = value ?? string.Empty;
            return value.Replace(@"\", @"\\").Replace("'", @"\'");
        }
    }
}
