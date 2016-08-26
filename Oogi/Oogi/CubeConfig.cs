using System.Collections.Generic;
using Newtonsoft.Json;

namespace Oogi
{    
    public class SavedCube
    {
        [JsonProperty(PropertyName = "cellsAsCSVStyleArray")]
        public List<List<object>> CellsAsCsvStyleArray { get; set; }
    }

    public class CubeConfig
    {
        [JsonProperty(PropertyName = "savedCube")]
        public SavedCube SavedCube { get; set; }
    }
}
