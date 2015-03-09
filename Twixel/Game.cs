using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Game : TwixelObjectBase
    {
        /// <summary>
        /// v2/v3
        /// </summary>
        public long? viewers;

        /// <summary>
        /// v2/v3
        /// </summary>
        public long? channels;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string name;

        /// <summary>
        /// v2/v3
        /// </summary>
        public long? id;

        /// <summary>
        /// v2/v3
        /// </summary>
        public long? giantBombId;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Dictionary<string, Uri> box;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Dictionary<string, Uri> logo;

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
