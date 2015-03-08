using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class TwixelObjectBase
    {
        public Twixel.APIVersion version;
        public Dictionary<string, Uri> baseLinks;

        public TwixelObjectBase(JObject baseLinksO)
        {
            baseLinks = HelperMethods.LoadLinks(baseLinksO);
        }
    }
}
