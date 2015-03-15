using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Badge object
    /// </summary>
    public class Badge : TwixelObjectBase
    {
        /// <summary>
        /// Badge name
        /// </summary>
        public string name;

        /// <summary>
        /// Badge image links
        /// </summary>
        public Dictionary<string, Uri> links;

        /// <summary>
        /// Badge constructor
        /// </summary>
        /// <param name="name">Badge name</param>
        /// <param name="linksO">Badge image links</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Badge(string name,
            JObject linksO,
            Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.name = name;
            if (linksO != null)
            {
                this.links = HelperMethods.LoadLinks(linksO);
            }
        }
    }
}
