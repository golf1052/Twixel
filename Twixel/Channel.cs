using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TwixelAPI
{
    /// <summary>
    /// Channel object
    /// </summary>
    public class Channel : TwixelObjectBase
    {
        /// <summary>
        /// Mature status
        /// v2/v3
        /// </summary>
        public bool? mature;

        /// <summary>
        /// Status
        /// v2/v3
        /// </summary>
        public string status;

        /// <summary>
        /// Broadcaster language as a string
        /// v3
        /// </summary>
        public string broadcasterLanguageString;

        /// <summary>
        /// Broadcaster language
        /// v3
        /// </summary>
        public CultureInfo broadcasterLanguage;

        /// <summary>
        /// Display name
        /// v2/v3
        /// </summary>
        public string displayName;

        /// <summary>
        /// Current game, can be null
        /// v2/v3
        /// </summary>
        public string game;

        /// <summary>
        /// Current delay
        /// Partnered channels can have non 0 delay
        /// v3
        /// </summary>
        public int? delay;

        /// <summary>
        /// Language string
        /// v3
        /// </summary>
        public string languageString;

        /// <summary>
        /// Language
        /// v3
        /// </summary>
        public CultureInfo language;

        /// <summary>
        /// ID
        /// v2/v3
        /// </summary>
        public long id;

        /// <summary>
        /// Name
        /// v2/v3
        /// </summary>
        public string name;

        /// <summary>
        /// Creation date
        /// v2/v3
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// Last updated
        /// v2/v3
        /// </summary>
        public DateTime updatedAt;

        /// <summary>
        /// Logo
        /// v2
        /// </summary>
        public Uri logo;

        /// <summary>
        /// Banner
        /// v2/v3
        /// </summary>
        public Uri banner;

        /// <summary>
        /// Video banner
        /// v2/v3
        /// </summary>
        public Uri videoBanner;

        /// <summary>
        /// Background
        /// v2/v3
        /// </summary>
        public Uri background;

        /// <summary>
        /// Profile banner
        /// v3
        /// </summary>
        public Uri profileBanner;

        /// <summary>
        /// Profile banner background color
        /// v3
        /// </summary>
        public string profileBannerBackgroundColor;

        /// <summary>
        /// Twitch partnership status
        /// v3
        /// </summary>
        public bool? partner;

        /// <summary>
        /// Link to channel
        /// v2/v3
        /// </summary>
        public Uri url;

        /// <summary>
        /// Number of views
        /// v3
        /// </summary>
        public long? views;

        /// <summary>
        /// Number of followers
        /// v3
        /// </summary>
        public long? followers;

        /// <summary>
        /// Teams this channel is a part of
        /// v2
        /// </summary>
        public List<Team> teams;

        /// <summary>
        /// Channel constructor, Twitch API v2
        /// </summary>
        /// <param name="mature">Mature status</param>
        /// <param name="status">Status</param>
        /// <param name="displayName">Display name</param>
        /// <param name="game">Current game, can be null</param>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="createdAt">Creation date</param>
        /// <param name="updatedAt">Last updated</param>
        /// <param name="logo">Logo</param>
        /// <param name="banner">Banner</param>
        /// <param name="videoBanner">Video banner</param>
        /// <param name="background">Background</param>
        /// <param name="url">Link to channel</param>
        /// <param name="teamsA">Teams JSON object</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Channel(bool? mature,
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
            JArray teamsA,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = Twixel.APIVersion.v2;
            this.teams = new List<Team>();
            this.mature = mature;
            this.status = status;
            this.displayName = displayName;
            this.game = game;
            this.id = id;
            this.name = name;
            if (!string.IsNullOrEmpty(createdAt))
            {
                this.createdAt = DateTime.Parse(createdAt);
            }
            if (!string.IsNullOrEmpty(updatedAt))
            {
                this.updatedAt = DateTime.Parse(updatedAt);
            }
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
            if (!string.IsNullOrEmpty(url))
            {
                this.url = new Uri(url);
            }
            if (teamsA != null)
            {
                foreach (JObject team in teamsA)
                {
                    teams.Add(HelperMethods.LoadTeam(team, version));
                }
            }
        }

        /// <summary>
        /// Channel constructor, Twitch API v3
        /// </summary>
        /// <param name="mature">Mature status</param>
        /// <param name="status">Status</param>
        /// <param name="broadcasterLanguage">Broadcaster language</param>
        /// <param name="displayName">Display name</param>
        /// <param name="game">Current game, can be null</param>
        /// <param name="delay">Current delay</param>
        /// <param name="language">Language</param>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="createdAt">Creation date</param>
        /// <param name="updatedAt">Last updated</param>
        /// <param name="logo">Logo</param>
        /// <param name="banner">Banner</param>
        /// <param name="videoBanner">Video banner</param>
        /// <param name="background">Background</param>
        /// <param name="profileBanner">Profile banner</param>
        /// <param name="profileBannerBackgroundColor">Profile banner background color</param>
        /// <param name="partner">Twitch partnership status</param>
        /// <param name="url">Link to channel</param>
        /// <param name="views">Number of views</param>
        /// <param name="followers">Number of followers</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Channel(bool? mature,
            string status,
            string broadcasterLanguage,
            string displayName,
            string game,
            int? delay,
            string language,
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
            bool? partner,
            string url,
            long? views,
            long? followers,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = Twixel.APIVersion.v3;
            this.mature = mature;
            this.status = status;
            this.broadcasterLanguageString = broadcasterLanguage;
            if (!string.IsNullOrEmpty(broadcasterLanguage))
            {
                try
                {
                    this.broadcasterLanguage = new CultureInfo(broadcasterLanguage);
                }
                catch (CultureNotFoundException ex)
                {
                    this.broadcasterLanguage = CultureInfo.InvariantCulture;
                }
            }
            this.displayName = displayName;
            this.game = game;
            this.delay = delay;
            this.languageString = language;
            if (!string.IsNullOrEmpty(language))
            {
                try
                {
                    this.language = new CultureInfo(language);
                }
                catch (CultureNotFoundException ex)
                {
                    this.language = CultureInfo.InvariantCulture;
                }
            }
            this.id = id;
            this.name = name;
            if (!string.IsNullOrEmpty(createdAt))
            {
                this.createdAt = DateTime.Parse(createdAt);
            }
            if (!string.IsNullOrEmpty(updatedAt))
            {
                this.updatedAt = DateTime.Parse(updatedAt);
            }
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
            if (!string.IsNullOrEmpty(url))
            {
                this.url = new Uri(url);
            }
            this.views = views;
            this.followers = followers;
        }
    }
}
