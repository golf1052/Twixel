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
        public long id;
        public WebUrl preview;
        public string game;
        public Channel channel;
        public string name;
        public int viewers;

        public Stream(string channelUrl, string broadcaster, long id, string preview, string game, JObject channelO, string name, int viewers, Twixel twixel)
        {
            this.channelUrl = new WebUrl(channelUrl);
            this.broadcaster = broadcaster;
            this.id = id;
            this.preview = new WebUrl(preview);
            this.game = game;
            LoadChannel(channelO, twixel);
            this.name = name;
            this.viewers = viewers;
        }

        void LoadChannel(JObject o, Twixel twixel)
        {
            channel = new Channel((string)o["mature"],
                (string)o["background"],
                (string)o["updated_at"],
                (long)o["_id"],
                (JArray)o["teams"],
                (string)o["status"],
                (string)o["logo"],
                (string)o["url"],
                (string)o["display_name"],
                (string)o["game"],
                (string)o["banner"],
                (string)o["name"],
                (string)o["video_banner"],
                (string)o["_links"]["chat"],
                (string)o["_links"]["subscriptions"],
                (string)o["_links"]["features"],
                (string)o["_links"]["commercial"],
                (string)o["_links"]["stream_key"],
                (string)o["_links"]["editors"],
                (string)o["_links"]["videos"],
                (string)o["_links"]["self"],
                (string)o["_links"]["follows"],
                (string)o["created_at"],
                twixel);
        }
    }
}
