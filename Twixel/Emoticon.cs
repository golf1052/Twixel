using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Emoticon : TwixelObjectBase
    {
        public string regex;
        public List<EmoticonImage> emoticonImages;

        public Emoticon(string regex, JArray imagesA, Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            emoticonImages = new List<EmoticonImage>();
            this.regex = regex;
            emoticonImages = LoadEmoticonImages(imagesA);
        }

        List<EmoticonImage> LoadEmoticonImages(JArray a)
        {
            List<EmoticonImage> images = new List<EmoticonImage>();
            foreach(JObject o in a)
            {
                images.Add(new EmoticonImage((int?)o["emoticon_set"],
                    (int)o["height"],
                    (int)o["width"],
                    (string)o["url"]));
            }
            return images;
        }
    }
}
