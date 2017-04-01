using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// FeaturedStream object
    /// </summary>
    public class FeaturedStream : TwixelObjectBase
    {
        /// <summary>
        /// Description text.
        /// Contains HTML tags by default.
        /// v2/v3
        /// </summary>
        public string text;

        /// <summary>
        /// Link to image
        /// v2/v3
        /// </summary>
        public Uri image;

        /// <summary>
        /// Title
        /// v3
        /// </summary>
        public string title;

        /// <summary>
        /// Sponsored status
        /// v3
        /// </summary>
        public bool sponsored;

        /// <summary>
        /// Priority
        /// v3
        /// </summary>
        public int priority;

        /// <summary>
        /// Scheduled status
        /// v3
        /// </summary>
        public bool scheduled;

        /// <summary>
        /// Stream object
        /// v2/v3
        /// </summary>
        public Stream stream;

        /// <summary>
        /// FeaturedStream constructor, Twitch API v3
        /// </summary>
        /// <param name="text">Description text</param>
        /// <param name="image">Link to image</param>
        /// <param name="title">Title</param>
        /// <param name="sponsored">Sponsored status</param>
        /// <param name="priority">Priority</param>
        /// <param name="scheduled">Scheduled status</param>
        /// <param name="streamO">Stream JSON object</param>
        /// <param name="baseLinksO">Base links JSON object</param>
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
            this.stream = HelperMethods.LoadStream(streamO, version);
        }

        /// <summary>
        /// Remove HTML tags and HTML decode info string
        /// </summary>
        public void CleanTextString()
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
