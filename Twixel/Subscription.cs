using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;

namespace TwixelAPI
{
    public class Subscription
    {
        public string id;
        public User user;
        public DateTime createdAt;
        public Channel channel;

        public Subscription(string id, JObject userO, string createdAt, Twixel twixel)
        {
            this.id = id;
            this.user = twixel.LoadUser(userO);
            this.createdAt = DateTime.Parse(createdAt);
        }

        public Subscription(string id, string createdAt, JObject channelO, Twixel twixel)
        {
            this.id = id;
            this.createdAt = DateTime.Parse(createdAt);
            this.channel = twixel.LoadChannel(channelO);
        }
    }
}
