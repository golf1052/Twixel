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
        public Uri image;
        public string text;
        public Stream stream;

        public FeaturedStream(string channelUrl, string image, string text, JObject streamO, Twixel twixel)
        {
            this.image = new Uri(image);
            this.text = text;
            this.stream = twixel.LoadStream(streamO, channelUrl);
        }
    }
}
