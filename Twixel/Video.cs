using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public struct Resolution
    {
        public readonly int x;
        public readonly int y;

        public Resolution(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public class Video : TwixelObjectBase
    {
        /// <summary>
        /// v2/v3
        /// </summary>
        public string title;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string description;

        /// <summary>
        /// v2/v3
        /// </summary>
        public long broadcastId;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string status;

        /// <summary>
        /// v3
        /// </summary>
        public string tagList;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string id;

        /// <summary>
        /// v2/v3
        /// </summary>
        public DateTime recordedAt;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string game;

        /// <summary>
        /// v2/v3
        /// </summary>
        public long length;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri preview;
        
        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri url;

        /// <summary>
        /// v2
        /// </summary>
        public string embed;

        /// <summary>
        /// v2/v3
        /// </summary>
        public long views;

        /// <summary>
        /// v3
        /// </summary>
        public Dictionary<string, double> fps;

        /// <summary>
        /// v3
        /// </summary>
        public Dictionary<string, Resolution> resolutions;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string broadcastType;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Dictionary<string, string> channel;

        public Video(string title,
            string description,
            long broadcastId,
            string status,
            string id,
            string recordedAt,
            string game,
            long length,
            string preview,
            string url,
            string embed,
            long views,
            string broadcastType,
            JObject miniChannelO,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = Twixel.APIVersion.v2;
            this.title = title;
            this.description = description;
            this.broadcastId = broadcastId;
            this.status = status;
            this.id = id;
            this.recordedAt = DateTime.Parse(recordedAt);
            this.game = game;
            this.length = length;
            if (!string.IsNullOrEmpty(preview))
            {
                this.preview = new Uri(preview);
            }
            this.url = new Uri(url);
            this.embed = embed;
            this.views = views;
            this.broadcastType = broadcastType;
            this.channel = LoadChannel(miniChannelO);
        }

        public Video(string title,
            string description,
            long broadcastId,
            string status,
            string tagList,
            string id,
            string recordedAt,
            string game,
            long length,
            string preview,
            string url,
            long views,
            string fps,
            string resolutions,
            string broadcastType,
            JObject miniChannelO,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = Twixel.APIVersion.v3;
            this.title = title;
            this.description = description;
            this.broadcastId = broadcastId;
            this.status = status;
            this.tagList = tagList;
            this.id = id;
            this.recordedAt = DateTime.Parse(recordedAt);
            this.game = game;
            this.length = length;
            if (!string.IsNullOrEmpty(preview))
            {
                this.preview = new Uri(preview);
            }
            this.url = new Uri(url);
            this.views = views;
            if (!string.IsNullOrEmpty(fps))
            {
                this.fps = LoadFps(fps);
            }
            if (!string.IsNullOrEmpty(resolutions))
            {
                this.resolutions = LoadResolutions(resolutions);
            }
            this.broadcastType = broadcastType;
            this.channel = LoadChannel(miniChannelO);
        }

        private Dictionary<string, double> LoadFps(string s)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, double>>(s);
        }

        private Dictionary<string, Resolution> LoadResolutions(string s)
        {
            Dictionary<string, string> tmp = JsonConvert.DeserializeObject<Dictionary<string, string>>(s);
            Dictionary<string, Resolution> resolutions = new Dictionary<string, Resolution>();
            foreach (var res in tmp)
            {
                string[] splitRes = res.Value.Split('x');
                resolutions.Add(res.Key,
                    new Resolution(int.Parse(splitRes[0]),
                    int.Parse(splitRes[1])));
            }
            return resolutions;
        }
        
        private Dictionary<string, string> LoadChannel(JObject o)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(o.ToString());
        }
    }
}
