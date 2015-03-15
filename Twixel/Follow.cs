using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Follow object
    /// </summary>
    /// <typeparam name="T">Wrapped object, usually either User or Channel object</typeparam>
    public class Follow<T> : TwixelObjectBase
    {
        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// Notification status
        /// </summary>
        public bool notifications;

        /// <summary>
        /// Wrapped object, usually either User or Channel object
        /// </summary>
        public T wrapped;

        /// <summary>
        /// Follow constructor
        /// </summary>
        /// <param name="createdAt">Creation date</param>
        /// <param name="notifications">Notification status</param>
        /// <param name="t">Wrapped object</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Follow(string createdAt,
            bool notifications,
            T t,
            Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.createdAt = DateTime.Parse(createdAt);
            this.notifications = notifications;
            this.wrapped = t;
        }
    }
}
