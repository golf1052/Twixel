using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Follow : TwixelObjectBase
    {
        public DateTime createdAt;
        public bool notifications;
        public User user;

        public Follow(string createdAt,
            bool notifications,
            JObject userO,
            Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.createdAt = DateTime.Parse(createdAt);
            this.notifications = notifications;
            this.user = HelperMethods.LoadUser(userO, version);
        }
    }
}
