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
        public WebUrl url;
        public string id;
        public WebUrl channel;
        public string embed;
        public int views;
        public string description;
        public int length;
        public string game;
        public WebUrl preview;

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
            this.url = new WebUrl(url);
            this.id = id;
            this.channel = new WebUrl(channel);
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
            this.preview = new WebUrl(preview);
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
            this.title = title;
            this.url = new WebUrl(url);
            this.id = id;
            this.channel = new WebUrl(channel);
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
            this.preview = new WebUrl(preview);
            this.name = name;
        }
    }
}
