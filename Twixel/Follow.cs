using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Follow<T> : TwixelObjectBase
    {
        public DateTime createdAt;
        public bool notifications;
        public T wrapped;

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
