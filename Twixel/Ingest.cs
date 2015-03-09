using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Ingest : TwixelObjectBase
    {
        /// <summary>
        /// v2/v3
        /// </summary>
        public string name;

        /// <summary>
        /// v2/v3
        /// </summary>
        public bool defaultIngest;

        /// <summary>
        /// v2/v3
        /// </summary>
        public long id;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri urlTemplate;

        /// <summary>
        /// v2/v3
        /// </summary>
        public double avalibility;

        public Ingest(string name,
            bool defaultIngest,
            long id,
            string urlTemplate,
            double avalibility,
            Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.name = name;
            this.defaultIngest = defaultIngest;
            this.id = id;
            this.urlTemplate = new Uri(urlTemplate);
            this.avalibility = avalibility;
        }
    }
}
