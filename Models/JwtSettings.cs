using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JwtAuthDemo.Models
{
    public partial class JwtSettings
    {
        [JsonProperty("Issuer")]
        public string Issuer { get; set; }

        [JsonProperty("SignKey")]
        public string SignKey { get; set; }
    }
}
