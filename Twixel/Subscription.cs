using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Subscription object
    /// </summary>
    /// <typeparam name="T">Wrapped object, usually either User or Channel object</typeparam>
    public class Subscription<T> : TwixelObjectBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public string id;

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// Wrapped object, usually either User or Channel object
        /// </summary>
        public T wrapped;

        /// <summary>
        /// Subscription constructor
        /// </summary>
        /// <param name="createdAt">Creation date</param>
        /// <param name="id">ID</param>
        /// <param name="t">Wrapped object</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Subscription(string createdAt,
            string id,
            T t,
            Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.createdAt = DateTime.Parse(createdAt);
            this.id = id;
            this.wrapped = t;
        }
    }
}
