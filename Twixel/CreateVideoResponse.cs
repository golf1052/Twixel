using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwixelAPI
{
    internal class CreateVideoResponse
    {
        [JsonProperty("token")]
        public string Token { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }
    }
}
