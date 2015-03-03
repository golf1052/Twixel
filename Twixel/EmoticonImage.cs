using System;

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
