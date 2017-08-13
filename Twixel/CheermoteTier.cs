using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwixelAPI
{
    public class CheermoteTier
    {
        [JsonProperty("min_bits")]
        public int MinBits { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("color")]
        public string Color { get; private set; }

        [JsonProperty("images")]
        public JObject Images { get; private set; }
    }
}
