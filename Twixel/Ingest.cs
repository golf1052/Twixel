using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Ingets object
    /// </summary>
    public class Ingest : TwixelObjectBase
    {
        /// <summary>
        /// Name
        /// </summary>
        public string name;

        /// <summary>
        /// Default status
        /// </summary>
        public bool defaultIngest;

        /// <summary>
        /// ID
        /// </summary>
        public long id;

        /// <summary>
        /// Url template.
        /// By directing an RTMP stream with a stream key injected into {stream_key}, you will broadcast content live on Twitch.
        /// </summary>
        public Uri urlTemplate;

        /// <summary>
        /// Availability
        /// </summary>
        public double avalibility;

        /// <summary>
        /// Ingest constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="defaultIngest">Default status</param>
        /// <param name="id">ID</param>
        /// <param name="urlTemplate">Url template</param>
        /// <param name="avalibility">Availability</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
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
