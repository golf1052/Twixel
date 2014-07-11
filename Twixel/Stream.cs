using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace TwixelAPI
{
    public class Stream
    {
        public WebUrl channelUrl;
        public string broadcaster;
        public long? id;
        public WebUrl preview;
        public string game;
        public Channel channel;
        public string name;
        public int? viewers;

        public Stream(string channelUrl, string broadcaster, long? id, string preview, string game, JObject channelO, string name, int? viewers, Twixel twixel)
        {
            if (channelUrl != null)
            {
                this.channelUrl = new WebUrl(channelUrl);
            }
            this.broadcaster = broadcaster;
            if (id == null)
            {
                this.id = -1;
            }
            else
            {
                this.id = id;
            }
            this.preview = new WebUrl(preview);
            this.game = game;
            channel = twixel.LoadChannel(channelO);
            this.name = name;
            if (viewers == null)
            {
                this.viewers = -1;
            }
            else
            {
                this.viewers = viewers;
            }
        }
    }
}
