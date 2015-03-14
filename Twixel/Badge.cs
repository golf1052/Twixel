using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Badge : TwixelObjectBase
    {
        public string name;
        public Dictionary<string, Uri> links;

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
