using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class FeaturedStream : TwixelObjectBase
    {
        /// <summary>
        /// v2/v3
        /// </summary>
        public string text;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri image;

        /// <summary>
        /// v3
        /// </summary>
        public string title;

        /// <summary>
        /// v3
        /// </summary>
        public bool sponsored;

        /// <summary>
        /// v3
        /// </summary>
        public int priority;

        /// <summary>
        /// v3
        /// </summary>
        public bool scheduled;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Stream stream;

        public FeaturedStream(string text,
            string image,
            JObject streamO,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = Twixel.APIVersion.v2;
            this.text = text;
            if (!string.IsNullOrEmpty(image))
            {
                this.image = new Uri(image);
            }
            this.stream = HelperMethods.LoadStream(streamO, baseLinksO, version);
        }

        public FeaturedStream(string text,
            string image,
            string title,
            bool sponsored,
            int priority,
            bool scheduled,
            JObject streamO,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = Twixel.APIVersion.v3;
            this.text = text;
            if (!string.IsNullOrEmpty(image))
            {
                this.image = new Uri(image);
            }
            this.title = title;
            this.sponsored = sponsored;
            this.priority = priority;
            this.scheduled = scheduled;
            this.stream = HelperMethods.LoadStream(streamO, baseLinksO, version);
        }

        public void CleanInfoString()
        {
            text = HelperMethods.ConvertAmp(HelperMethods.RemoveHtmlTags(text)).Trim();
            char lastChar = '\0';
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    if (lastChar == '\n')
                    {
                        i -= 1;
                        text = text.Remove(i);
                        break;
                    }
                }
                lastChar = text[i];
            }
        }
    }
}
