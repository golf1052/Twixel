using System;

namespace TwixelAPI
{
    /// <summary>
    /// EmoticonImage object
    /// </summary>
    public class EmoticonImage
    {
        /// <summary>
        /// Emoticon set.
        /// If null emoticon is part of the default Twitch set.
        /// </summary>
        public int? emoticonSet;

        /// <summary>
        /// Height in pixels
        /// </summary>
        public int height;

        /// <summary>
        /// Width in pixels
        /// </summary>
        public int width;

        /// <summary>
        /// Link to image
        /// </summary>
        public Uri url;

        /// <summary>
        /// EmoticonImage constructor
        /// </summary>
        /// <param name="emoticonSet">Emoticon set</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="url">Link to image</param>
        public EmoticonImage(int? emoticonSet, int height, int width, string url)
        {
            this.emoticonSet = emoticonSet;
            this.height = height;
            this.width = width;
            this.url = new Uri(url);
        }
    }
}
