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
    public class Stream : TwixelObjectBase
    {
        /// <summary>
        /// v2/v3
        /// </summary>
        public long? id;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string game;

        /// <summary>
        /// v2/v3
        /// </summary>
        public long? viewers;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string createdAtString;

        /// <summary>
        /// v2/v3
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// v2/v3
        /// </summary>
        public int? videoHeight;

        /// <summary>
        /// v2/v3
        /// </summary>
        public double? averageFps;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Dictionary<string, Uri> links;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string name;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string broadcaster;

        /// <summary>
        /// v2
        /// </summary>
        public Uri preview;

        /// <summary>
        /// v3
        /// </summary>
        public Dictionary<string, Uri> previewList;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Channel channel;

        public Stream(long? id,
            string game,
            long? viewers,
            string createdAt,
            int? videoHeight,
            double? averageFps,
            JObject linksO,
            string name,
            string broadcaster,
            string preview,
            JObject channelO,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = Twixel.APIVersion.v2;
            this.id = id;
            this.game = game;
            this.viewers = viewers;
            this.createdAtString = createdAt;
            if (!string.IsNullOrEmpty(createdAt))
            {
                this.createdAt = DateTime.Parse(createdAt);
            }
            this.videoHeight = videoHeight;
            this.averageFps = averageFps;
            this.links = HelperMethods.LoadLinks(linksO);
            this.name = name;
            this.broadcaster = broadcaster;
            if (!string.IsNullOrEmpty(preview))
            {
                this.preview = new Uri(preview);
            }
            this.channel = HelperMethods.LoadChannel(channelO, version);
        }

        public Stream(long? id,
            string game,
            long? viewers,
            string createdAt,
            int videoHeight,
            double averageFps,
            JObject linksO,
            string name,
            string broadcaster,
            JObject previewO,
            JObject channelO,
            JObject baseLinksO)
            : base(baseLinksO)
        {
            this.version = Twixel.APIVersion.v3;
            this.id = id;
            this.game = game;
            this.viewers = viewers;
            this.createdAtString = createdAt;
            this.createdAt = DateTime.Parse(createdAt);
            this.videoHeight = videoHeight;
            this.averageFps = averageFps;
            this.links = HelperMethods.LoadLinks(linksO);
            this.name = name;
            this.broadcaster = broadcaster;
            this.previewList = HelperMethods.LoadLinks(previewO);
            this.channel = HelperMethods.LoadChannel(channelO, version);
        }
    }
}
