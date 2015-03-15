using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Game object
    /// </summary>
    public class Game : TwixelObjectBase
    {
        /// <summary>
        /// Number of viewers.
        /// Null if game was searched.
        /// </summary>
        public long? viewers;

        /// <summary>
        /// Number of channels.
        /// Null if game was searched
        /// </summary>
        public long? channels;

        /// <summary>
        /// Name
        /// </summary>
        public string name;

        /// <summary>
        /// ID
        /// </summary>
        public long? id;

        /// <summary>
        /// GiantBomb ID
        /// </summary>
        public long? giantBombId;

        /// <summary>
        /// Box image links.
        /// Dictionary strings: small, medium, large, template
        /// </summary>
        public Dictionary<string, Uri> box;

        /// <summary>
        /// Logo image links.
        /// Dictionary strings: small, medium, large, template
        /// </summary>
        public Dictionary<string, Uri> logo;

        /// <summary>
        /// Game constructor
        /// </summary>
        /// <param name="viewers">Number of viewers</param>
        /// <param name="channels">Number of channels</param>
        /// <param name="name">Name</param>
        /// <param name="id">ID</param>
        /// <param name="giantBombId">GiantBomb ID</param>
        /// <param name="boxO">Box JSON object</param>
        /// <param name="logoO">Logo JSON object</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Game(long? viewers,
            long? channels,
            string name,
            long? id,
            long? giantBombId,
            JObject boxO,
            JObject logoO,
            Twixel.APIVersion version,
            JObject baseLinksO) : base (baseLinksO)
        {
            this.version = version;
            this.viewers = viewers;
            this.channels = channels;
            this.name = name;
            this.id = id;
            this.giantBombId = giantBombId;
            this.box = HelperMethods.LoadLinks(boxO);
            this.logo = HelperMethods.LoadLinks(logoO);
        }
    }
}
