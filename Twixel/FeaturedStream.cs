using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace TwixelAPI
{
    public class FeaturedStream
    {
        public WebUrl image;
        public string text;
        public Stream stream;

        public FeaturedStream(string image, string text, JObject streamO)
        {
            this.image = new WebUrl(image);
            this.text = text;
            this.stream = LoadStream(streamO);
        }

        Stream LoadStream(JObject o)
        {
            return new Stream((string)o["broadcaster"],
                    (long)o["_id"],
                    (string)o["preview"],
                    (string)o["game"],
                    (JObject)o["channel"],
                    (string)o["name"],
                    (int)o["viewers"]);
        }
    }
}
