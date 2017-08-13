using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwixelAPI
{
    public class Cheermote
    {
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }

        [JsonProperty("scales")]
        public List<string> Scales { get; private set; }

        [JsonProperty("tiers")]
        public List<CheermoteTier> Tiers { get; private set; }

        [JsonProperty("backgrounds")]
        public List<string> Backgrounds { get; private set; }

        [JsonProperty("states")]
        public List<string> States { get; private set; }

        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; private set; }

        [JsonProperty("priority")]
        public int Priority { get; private set; }
    }
}
