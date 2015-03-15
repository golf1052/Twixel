using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// TwixelObjectBase object.
    /// All Twixel objects (except for Emoticons) derive from this.
    /// </summary>
    public class TwixelObjectBase
    {
        /// <summary>
        /// Twitch API version that was used to create this object
        /// </summary>
        public Twixel.APIVersion version;

        /// <summary>
        /// Base links
        /// </summary>
        public Dictionary<string, Uri> baseLinks;

        /// <summary>
        /// TwixelObjectBase constructor
        /// </summary>
        /// <param name="baseLinksO">Base links JSON object</param>
        public TwixelObjectBase(JObject baseLinksO)
        {
            baseLinks = HelperMethods.LoadLinks(baseLinksO);
        }
    }
}
