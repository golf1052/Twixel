using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Links
    {
        public Uri self;
        public Uri follows;
        public Uri commercial;
        public Uri streamKey;
        public Uri chat;
        public Uri features;
        public Uri subscriptions;
        public Uri editors;
        public Uri videos;

        public Links(string self,
            string follows,
            string commercial,
            string streamKey,
            string chat,
            string features,
            string subscriptions,
            string editors,
            string videos)
        {
            Init(self,
                follows,
                commercial,
                streamKey,
                chat,
                features,
                subscriptions,
                editors,
                videos);
        }

        public Links(JObject linksO)
        {
            Init((string)linksO["self"],
                (string)linksO["follows"],
                (string)linksO["commercial"],
                (string)linksO["stream_key"],
                (string)linksO["chat"],
                (string)linksO["features"],
                (string)linksO["subscriptions"],
                (string)linksO["editors"],
                (string)linksO["videos"]);
        }

        private void Init(string self,
            string follows,
            string commercial,
            string streamKey,
            string chat,
            string features,
            string subscriptions,
            string editors,
            string videos)
        {
            if (!string.IsNullOrEmpty(self))
                this.self = new Uri(self);
            if (!string.IsNullOrEmpty(follows))
                this.follows = new Uri(follows);
            if (!string.IsNullOrEmpty(commercial))
                this.commercial = new Uri(commercial);
            if (!string.IsNullOrEmpty(streamKey))
                this.streamKey = new Uri(streamKey);
            if (!string.IsNullOrEmpty(chat))
                this.chat = new Uri(chat);
            if (!string.IsNullOrEmpty(features))
                this.features = new Uri(features);
            if (!string.IsNullOrEmpty(subscriptions))
                this.subscriptions = new Uri(subscriptions);
            if (!string.IsNullOrEmpty(editors))
                this.editors = new Uri(editors);
            if (!string.IsNullOrEmpty(videos))
                this.videos = new Uri(videos);
        }
    }
}
