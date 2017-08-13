using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace TwixelAPI
{
    public class VideoObject
    {
        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("description_html")]
        public string DescriptionHtml { get; private set; }

        [JsonProperty("broadcast_id")]
        public long BroadcastId { get; private set; }

        [JsonProperty("broadcast_type")]
        public string BroadcastType { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        [JsonProperty("tag_list")]
        public string TagList { get; private set; }

        [JsonProperty("views")]
        public long Views { get; private set; }

        [JsonProperty("url")]
        public Uri url { get; private set; }

        [JsonProperty("language")]
        public string Language { get; private set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; private set; }

        [JsonProperty("viewable")]
        public string Viewable { get; private set; }

        [JsonProperty("viewable_at")]
        public DateTimeOffset? ViewableAt { get; private set; }

        [JsonProperty("published_at")]
        public DateTimeOffset? PublishedAt { get; private set; }

        [JsonProperty("_id")]
        public string Id { get; private set; }

        [JsonProperty("recorded_at")]
        public DateTimeOffset RecordedAt { get; private set; }

        [JsonProperty("game")]
        public string Game { get; private set; }

        [JsonProperty("length")]
        public long Length { get; private set; }

        [JsonProperty("preview")]
        public Dictionary<string, Uri> Preview { get; private set; }

        [JsonProperty("animated_preview_url")]
        public Uri AnimatedPreviewUrl { get; private set; }

        [JsonProperty("thumbnails")]
        public JObject Thumbnails { get; private set; }

        [JsonProperty("fps")]
        public Dictionary<string, double> Fps { get; private set; }

        [JsonProperty("resolutions")]
        public JObject Resolutions { get; private set; }

        [JsonProperty("channel")]
        public JObject Channel { get; private set; }
    }
}
