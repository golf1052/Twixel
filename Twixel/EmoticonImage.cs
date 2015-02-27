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
    public class EmoticonImage
    {
        public int? emoticonSet;
        public int height;
        public int width;
        public Uri url;

        public EmoticonImage(int? emoticonSet, int height, int width, string url)
        {
            this.emoticonSet = emoticonSet;
            this.height = height;
            this.width = width;
            this.url = new Uri(url);
        }
    }
}
