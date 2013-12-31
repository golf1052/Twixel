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
    public class Emoticon
    {
        public string regex;
        public List<EmoticonImage> emoticonImages;

        public Emoticon(string regex, JArray imagesA)
        {
            emoticonImages = new List<EmoticonImage>();
            this.regex = regex;
            LoadEmoticonImages(imagesA);
        }

        void LoadEmoticonImages(JArray a)
        {
            foreach(JObject o in a)
            {
                emoticonImages.Add(new EmoticonImage((int?)o["emoticon_set"],
                    (int)o["height"],
                    (int)o["width"],
                    (string)o["url"]));
            }
        }
    }
}
