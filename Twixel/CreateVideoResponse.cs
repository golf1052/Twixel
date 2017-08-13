using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using TwixelAPI.Constants;

namespace TwixelAPI
{
    internal class CreateVideoResponse
    {
        [JsonProperty("upload")]
        public UploadObject Upload { get; private set; }

        [JsonProperty("video")]
        public VideoObject Video { get; private set; }

        internal class UploadObject
        {
            [JsonProperty("token")]
            public string Token { get; private set; }

            [JsonProperty("url")]
            public string Url { get; private set; }
        }
    }
}
