using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Emoticon object
    /// </summary>
    public class Emoticon : TwixelObjectBase
    {
        /// <summary>
        /// Regex
        /// </summary>
        public string regex;

        /// <summary>
        /// List of emoticon images
        /// </summary>
        public List<EmoticonImage> emoticonImages;

        /// <summary>
        /// Emoticon constructor
        /// </summary>
        /// <param name="regex">Regex</param>
        /// <param name="imagesA">Emoticon images JSON object</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Emoticon(string regex, JArray imagesA, Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            emoticonImages = new List<EmoticonImage>();
            this.regex = regex;
            emoticonImages = LoadEmoticonImages(imagesA);
        }

        private List<EmoticonImage> LoadEmoticonImages(JArray a)
        {
            List<EmoticonImage> images = new List<EmoticonImage>();
            foreach(JObject o in a)
            {
                if (!string.IsNullOrEmpty((string)o["height"]) &&
                    !string.IsNullOrEmpty((string)o["width"]) &&
                    !string.IsNullOrEmpty((string)o["url"]))
                {
                    images.Add(new EmoticonImage((int?)o["emoticon_set"],
                    (int)o["height"],
                    (int)o["width"],
                    (string)o["url"]));
                }
                
            }
            return images;
        }
    }
}
