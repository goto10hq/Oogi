using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;

namespace Oogi.Tokens
{
    [SerializePropertyNamesAsCamelCase]
    public class AzureSearchIndex : AzureSearchBase
    {        
        [IsFilterable]
        [IsSortable]        
        public GeographyPoint Point { get; set; }        
    }
}
