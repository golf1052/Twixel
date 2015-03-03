using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Channel : TwixelObjectBase
    {
        /// <summary>
        /// v2/v3
        /// </summary>
        public bool mature;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string status;

        /// <summary>
        /// v3
        /// </summary>
        public string broadcasterLanguage;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string displayName;

        /// <summary>
        /// v2/v3
        /// </summary>
        public string game;

        /// <summary>
        /// v3
        /// </summary>
        public int delay;

        /// <summary>
        /// v3
        /// </summary>
        public string language;

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
        /// v2
        /// </summary>
        public Uri logo;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri banner;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri videoBanner;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri background;

        /// <summary>
        /// v3
        /// </summary>
        public Uri profileBanner;

        /// <summary>
        /// v3
        /// </summary>
        public string profileBannerBackgroundColor;

        /// <summary>
        /// v3
        /// </summary>
        public bool partner;

        /// <summary>
        /// v2/v3
        /// </summary>
        public Uri url;

        /// <summary>
        /// v3
        /// </summary>
        public long? views;

        /// <summary>
        /// v3
        /// </summary>
        public long? followers;

        /// <summary>
        /// v2
        /// </summary>
        public List<Team> teams;

        public Twixel.APIVersion version;

        public Channel(bool mature,
            string status,
            string displayName,
            string game,
            long id,
            string name,
            string createdAt,
            string updatedAt,
            string logo,
            string banner,
            string videoBanner,
            string background,
            string url,
            JObject linksO,
            JArray teamsA) : base(linksO)
        {
            this.version = Twixel.APIVersion.v2;
            this.mature = mature;
            this.status = status;
            this.displayName = displayName;
            this.game = game;
            this.id = id;
            this.name = name;
            this.createdAtString = createdAt;
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAtString = updatedAt;
            this.updatedAt = DateTime.Parse(updatedAt);
            if (!string.IsNullOrEmpty(logo))
            {
                this.logo = new Uri(logo);
            }
            if (!string.IsNullOrEmpty(banner))
            {
                this.banner = new Uri(banner);
            }
            if (!string.IsNullOrEmpty(videoBanner))
            {
                this.videoBanner = new Uri(videoBanner);
            }
            if (!string.IsNullOrEmpty(background))
            {
                this.background = new Uri(background);
            }
            this.url = new Uri(url);
            if (teamsA != null)
            {
                foreach (JObject team in teamsA)
                {
                    teams.Add(HelperMethods.LoadTeam(team));
                }
            }
        }

        public Channel(bool mature,
            string status,
            string broadcasterLanguage,
            string displayName,
            string game,
            int delay,
            long id,
            string name,
            string createdAt,
            string updatedAt,
            string logo,
            string banner,
            string videoBanner,
            string background,
            string profileBanner,
            string profileBannerBackgroundColor,
            bool partner,
            string url,
            long? views,
            long? followers,
            JObject linksO) : base(linksO)
        {
            this.version = Twixel.APIVersion.v3;
            this.mature = mature;
            this.status = status;
            this.broadcasterLanguage = broadcasterLanguage;
            this.displayName = displayName;
            this.game = game;
            this.delay = delay;
            this.id = id;
            this.name = name;
            this.createdAtString = createdAt;
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAtString = updatedAt;
            this.updatedAt = DateTime.Parse(updatedAt);
            if (!string.IsNullOrEmpty(logo))
            {
                this.logo = new Uri(logo);
            }
            if (!string.IsNullOrEmpty(banner))
            {
                this.banner = new Uri(banner);
            }
            if (!string.IsNullOrEmpty(videoBanner))
            {
                this.videoBanner = new Uri(videoBanner);
            }
            if (!string.IsNullOrEmpty(background))
            {
                this.background = new Uri(background);
            }
            if (!string.IsNullOrEmpty(profileBanner))
            {
                this.profileBanner = new Uri(profileBanner);
            }
            this.profileBannerBackgroundColor = profileBannerBackgroundColor;
            this.partner = partner;
            this.url = new Uri(url);
            this.views = views;
            this.followers = followers;
        }
    }
}
