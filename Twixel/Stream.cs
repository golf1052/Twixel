using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Stream object
    /// </summary>
    public class Stream : TwixelObjectBase
    {
        /// <summary>
        /// ID
        /// v2/v3
        /// </summary>
        public long? id;

        /// <summary>
        /// Current game, can be null
        /// v2/v3
        /// </summary>
        public string game;

        /// <summary>
        /// Number of viewers
        /// v2/v3
        /// </summary>
        public long? viewers;

        /// <summary>
        /// Creation date
        /// v2/v3
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// Video height
        /// v2/v3
        /// </summary>
        public int? videoHeight;

        /// <summary>
        /// Average FPS
        /// v2/v3
        /// </summary>
        public double? averageFps;

        /// <summary>
        /// Name
        /// v2/v3
        /// </summary>
        public string name;

        /// <summary>
        /// Broadcaster software used
        /// v2/v3
        /// </summary>
        public string broadcaster;

        /// <summary>
        /// Link to preview image
        /// v2
        /// </summary>
        public Uri preview;

        /// <summary>
        /// Link to preview images
        /// Dictionary strings: small, medium, large, template
        /// v3
        /// </summary>
        public Dictionary<string, Uri> previewList;

        /// <summary>
        /// Channel object
        /// v2/v3
        /// </summary>
        public Channel channel;

        /// <summary>
        /// Stream constructor, Twitch API v3
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="game">Current game, can be null</param>
        /// <param name="viewers">Number of viewers</param>
        /// <param name="createdAt">Creation date</param>
        /// <param name="videoHeight">Video height</param>
        /// <param name="averageFps">Average FPS</param>
        /// <param name="name">Name</param>
        /// <param name="broadcaster">Broadcaster softare used</param>
        /// <param name="previewO">Preview JSON object</param>
        /// <param name="channelO">Channel JSON object</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Stream(long? id,
            string game,
            long? viewers,
            string createdAt,
            int videoHeight,
            double averageFps,
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
            this.createdAt = DateTime.Parse(createdAt);
            this.videoHeight = videoHeight;
            this.averageFps = averageFps;
            this.name = name;
            this.broadcaster = broadcaster;
            this.previewList = HelperMethods.LoadLinks(previewO);
            this.channel = HelperMethods.LoadChannel(channelO, version);
        }
    }
}
