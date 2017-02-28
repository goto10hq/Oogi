﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;

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
        [IsSortable]
        [Analyzer(AnalyzerName.AsString.CsLucene)]
        [JsonProperty("title_cz")]
        public string TitleCz { get; set; }

        [IsSearchable]
        [IsSortable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        [JsonProperty("title_en")]
        public string TitleEn { get; set; }

        [IsSearchable]
        [IsSortable]
        [Analyzer(AnalyzerName.AsString.CsLucene)]
        [JsonProperty("description_cz")]
        public string DescriptionCz { get; set; }

        [IsSearchable]
        [IsSortable]
        [Analyzer(AnalyzerName.AsString.EnLucene)]
        [JsonProperty("description_en")]
        public string DescriptionEn { get; set; }

        [IsSearchable]
        [IsFilterable]
        public string Slug { get; set; }

        [IsSortable]
        [IsFilterable]
        [IsFacetable]
        public DateTimeOffset? Date { get; set; }

        [IsSearchable]
        [IsFilterable]
        [IsFacetable]
        public List<string> Tags { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public int? Number { get; set; }

        [IsFilterable]
        [IsSortable]
        [IsFacetable]
        public double? Money { get; set; }

        [IsFilterable]
        [IsFacetable]
        public bool? Flag { get; set; }

        [IsFilterable]
        public bool IsDeleted { get; set; }
    }
}