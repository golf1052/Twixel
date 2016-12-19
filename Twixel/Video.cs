using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Resolution struct
    /// </summary>
    public struct Resolution
    {
        /// <summary>
        /// Width in pixels
        /// </summary>
        public readonly int width;

        /// <summary>
        /// Height in pixels
        /// </summary>
        public readonly int height;

        /// <summary>
        /// Resolution constructor
        /// </summary>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        public Resolution(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }

    /// <summary>
    /// Video object
    /// </summary>
    public class Video : TwixelObjectBase
    {
        /// <summary>
        /// Title
        /// v2/v3
        /// </summary>
        public string title;

        /// <summary>
        /// Description
        /// v2/v3
        /// </summary>
        public string description;

        /// <summary>
        /// Broadcast ID
        /// v2/v3
        /// </summary>
        public long broadcastId;

        /// <summary>
        /// Status
        /// v2/v3
        /// </summary>
        public string status;

        /// <summary>
        /// Comma seperated string of tags
        /// v3
        /// </summary>
        public string tagList;

        /// <summary>
        /// ID
        /// v2/v3
        /// </summary>
        public string id;

        /// <summary>
        /// Recording date
        /// v2/v3
        /// </summary>
        public DateTime recordedAt;

        /// <summary>
        /// Game, can be null
        /// v2/v3
        /// </summary>
        public string game;

        /// <summary>
        /// Length in seconds
        /// v2/v3
        /// </summary>
        public long length;

        /// <summary>
        /// Link to preview image
        /// v2/v3
        /// </summary>
        public Uri preview;
        
        /// <summary>
        /// Link to video
        /// v2/v3
        /// </summary>
        public Uri url;

        /// <summary>
        /// Embed string
        /// v2
        /// </summary>
        public string embed;

        /// <summary>
        /// Number of views
        /// v2/v3
        /// </summary>
        public long views;

        /// <summary>
        /// Quality FPS's
        /// Dictionary strings: chunked, high, medium, low, mobile, audio_only
        /// v3
        /// </summary>
        public Dictionary<string, double> fps;

        /// <summary>
        /// Quality resolutions.
        /// Dictionary strings: chunked, high, medium, low, mobile
        /// v3
        /// </summary>
        public Dictionary<string, Resolution> resolutions;

        /// <summary>
        /// Broadcast type
        /// v2/v3
        /// </summary>
        public string broadcastType;

        /// <summary>
        /// Channel info
        /// Dictionary strings: name, display_name
        /// v2/v3
        /// </summary>
        public Dictionary<string, string> channel;

        /// <summary>
        /// Video constructor, Twitch API v2
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="broadcastId">Broadcast ID</param>
        /// <param name="status">Status</param>
        /// <param name="id">ID</param>
        /// <param name="recordedAt">Recording date</param>
        /// <param name="game">Game, can be null</param>
        /// <param name="length">Length in seconds</param>
        /// <param name="preview">Link to preview image</param>
        /// <param name="url">Link to video</param>
        /// <param name="embed">Embed string</param>
        /// <param name="views">Number of views</param>
        /// <param name="broadcastType">Broadcast type</param>
        /// <param name="miniChannelO">Mini channel JSON object</param>
        /// <param name="baseLinksO">Base links JSON object</param>
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

        /// <summary>
        /// Video constructor, Twitch API v3
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="description">Description</param>
        /// <param name="broadcastId">Broadcast ID</param>
        /// <param name="status">Status</param>
        /// <param name="tagList">Comma seperated string of tags</param>
        /// <param name="id">ID</param>
        /// <param name="recordedAt">Recording date</param>
        /// <param name="game">Game, can be null</param>
        /// <param name="length">Length in seconds</param>
        /// <param name="preview">Link to preview image</param>
        /// <param name="url">Link to video</param>
        /// <param name="views">Number of views</param>
        /// <param name="fps">FPS JSON object</param>
        /// <param name="resolutions">Resolutions JSON object</param>
        /// <param name="broadcastType">Broadcast type</param>
        /// <param name="miniChannelO">Mini channel JSON object</param>
        /// <param name="baseLinksO">Base links JSON object</param>
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
                if (res.Value.Contains("x"))
                {
                    string[] splitRes = res.Value.Split('x');
                    resolutions.Add(res.Key,
                        new Resolution(int.Parse(splitRes[0]),
                        int.Parse(splitRes[1])));
                }
            }
            return resolutions;
        }
        
        private Dictionary<string, string> LoadChannel(JObject o)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(o.ToString());
        }
    }
}
