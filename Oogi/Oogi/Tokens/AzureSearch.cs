using System;
using System.Collections.Generic;
using Microsoft.Spatial;

namespace Oogi.Tokens
{
    public class AzureSearch
    {
        public string Index { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; }
        public int Number { get; set; }
        public decimal Money { get; set; }
        public bool Flag { get; set; }
        public GeographyPoint Point { get; set; }
    }
}
