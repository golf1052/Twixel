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
    public class Video
    {
        public DateTime recordedAt;
        public string title;
        public Uri url;
        public string id;
        public Uri channel;
        public string embed;
        public int views;
        public string description;
        public int length;
        public string game;
        public Uri preview;

        public string name;

        public Video(string recordedAt,
            string title,
            string url,
            string id,
            string channel,
            string embed,
            int views,
            string description,
            int length,
            string game,
            string preview)
        {
            this.recordedAt = DateTime.Parse(recordedAt);
            this.title = title;
            this.url = new Uri(url);
            this.id = id;
            this.channel = new Uri(channel);
            this.embed = embed;
            this.views = views;
            if (description != null)
            {
                this.description = description;
            }
            this.length = length;
            if (game != null)
            {
                this.game = game;
            }
            this.preview = new Uri(preview);
        }

        public Video(string recordedAt,
            string title,
            string url,
            string id,
            string channel,
            int views,
            string description,
            int length,
            string game,
            string preview,
            string name)
        {
            this.recordedAt = DateTime.Parse(recordedAt);
            if (title != null)
            {
                this.title = title;
            }
            if (url != null)
            {
                this.url = new Uri(url);
            }
            this.id = id;
            if (url != null)
            {
                this.channel = new Uri(channel);
            }
            this.views = views;
            if (description != null)
            {
                this.description = description;
            }
            this.length = length;
            if (game != null)
            {
                this.game = game;
            }
            if (preview != null)
            {
                this.preview = new Uri(preview);
            }
            if (preview != null)
            {
                this.name = name;
            }
        }
    }
}
