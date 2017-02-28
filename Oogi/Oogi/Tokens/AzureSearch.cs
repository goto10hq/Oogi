﻿using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Oogi.Tokens
{
    [SerializePropertyNamesAsCamelCase]
    public class AzureSearch : AzureSearchBase
    {        
        [IsFilterable]
        [IsSortable]        
        //public GeographyPoint Point { get; set; }
        public GeoPoint Point { get; set; }
    }
}
