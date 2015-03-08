using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Team : TwixelObjectBase
    {
        /// <summary>
        /// v2/v3
        /// </summary>
        public long id;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string name;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string info;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string displayName;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string createdAtString;

        /// <summary>
        /// v2/v3
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string updatedAtString;

        /// <summary>
        /// v2/v3
        /// </summary>
        public DateTime updatedAt;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri logo;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri banner;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri background;
        
        public Team(long id,
            string name,
            string info,
            string displayName,
            string createdAt,
            string updatedAt,
            string logo,
            string banner,
            string background,
            Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.id = id;
            this.name = name;
            this.info = info;
            this.displayName = displayName;
            this.createdAtString = createdAt;
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAtString = updatedAt;
            this.updatedAt = DateTime.Parse(updatedAt);
            if (logo != null)
            {
                this.logo = new Uri(logo);
            }
            if (banner != null)
            {
                this.banner = new Uri(banner);
            }
            if (background != null)
            {
                this.background = new Uri(background);
            }
        }
    }
}
