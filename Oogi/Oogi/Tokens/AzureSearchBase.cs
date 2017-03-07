using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace Oogi.Tokens
{
    public class AzureSearchBase
    {
        [Key]
        [IsFilterable]
        public string Id { get; set; }

        [IsFilterable]
        public string Entity { get; set; }

        [IsFilterable]
        public string Index { get; set; }

        [IsSearchable]        
        [Analyzer("standardasciifolding.lucene")]        
        public string Title { get; set; }
        
        [IsSearchable]
        [IsSortable]
        [Analyzer("standardasciifolding.lucene")]
        [IsFilterable]        
        public string Text { get; set; }

        [IsSearchable]
        [IsSortable]
        [Analyzer("standardasciifolding.lucene")]
        [IsFilterable]        
        public string Text2 { get; set; }

        [IsSearchable]
        [IsSortable]
        [Analyzer("standardasciifolding.lucene")]
        [IsFilterable]        
        public string Text3 { get; set; }

        [IsSearchable]
        [IsSortable]
        [Analyzer("standardasciifolding.lucene")]
        [IsFilterable]        
        public string Text4 { get; set; }

        [IsSearchable]
        [IsSortable]
        [Analyzer("standardasciifolding.lucene")]        
        [IsFilterable]        
        public string Text5 { get; set; }

        [IsSearchable]
        [IsFilterable]
        public string Slug { get; set; }

        [IsSortable]
        [IsFilterable]
        [IsFacetable]
        public DateTimeOffset? Date { get; set; }

        [IsSortable]
        [IsFilterable]
        [IsFacetable]
        public DateTimeOffset? Date2 { get; set; }

        [IsSortable]
        [IsFilterable]
        [IsFacetable]
        public DateTimeOffset? Date3 { get; set; }

        [IsSortable]
        [IsFilterable]
        [IsFacetable]
        public DateTimeOffset? Date4 { get; set; }

        [IsSortable]
        [IsFilterable]
        [IsFacetable]
        public DateTimeOffset? Date5 { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public List<string> Tags { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public List<string> Tags2 { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public List<string> Tags3 { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public List<string> Tags4 { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public List<string> Tags5 { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public int? Number { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public int? Number2 { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public int? Number3 { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public int? Number4 { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public int? Number5 { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public double? Money { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public double? Money2 { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public double? Money3 { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public double? Money4 { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public double? Money5 { get; set; }

        [IsFilterable]
        [IsFacetable]
        public bool? Flag { get; set; }

        [IsFilterable]
        [IsFacetable]
        public bool? Flag2 { get; set; }

        [IsFilterable]
        [IsFacetable]
        public bool? Flag3 { get; set; }

        [IsFilterable]
        [IsFacetable]
        public bool? Flag4 { get; set; }

        [IsFilterable]
        [IsFacetable]
        public bool? Flag5 { get; set; }

        [IsFilterable]
        public bool IsDeleted { get; set; }

        [IsSortable]
        public string Order { get; set; }        
    }
}
