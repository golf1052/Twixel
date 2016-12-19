using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Team object
    /// </summary>
    public class Team : TwixelObjectBase
    {
        /// <summary>
        /// ID
        /// </summary>
        public long id;

        /// <summary>
        /// Name
        /// </summary>
        public string name;

        /// <summary>
        /// Info.
        /// Contains HTML tags by default.
        /// </summary>
        public string info;

        /// <summary>
        /// Display name
        /// </summary>
        public string displayName;

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// Last updated
        /// </summary>
        public DateTime updatedAt;

        /// <summary>
        /// Link to logo
        /// </summary>
        public Uri logo;

        /// <summary>
        /// Link to banner
        /// </summary>
        public Uri banner;

        /// <summary>
        /// Link to background
        /// </summary>
        public Uri background;
        
        /// <summary>
        /// Team constructor
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="info">Info</param>
        /// <param name="displayName">Display name</param>
        /// <param name="createdAt">Creation date</param>
        /// <param name="updatedAt">Last updated</param>
        /// <param name="logo">Link to logo</param>
        /// <param name="banner">Link to banner</param>
        /// <param name="background">Link to background</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
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
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAt = DateTime.Parse(updatedAt);
            if (!string.IsNullOrEmpty(logo))
            {
                this.logo = new Uri(logo);
            }
            if (!string.IsNullOrEmpty(banner))
            {
                this.banner = new Uri(banner);
            }
            if (!string.IsNullOrEmpty(background))
            {
                this.background = new Uri(background);
            }
        }

        /// <summary>
        /// Remove HTML tags and HTML decode info string
        /// </summary>
        public void CleanInfoString()
        {
            info = HelperMethods.ConvertAmp(HelperMethods.RemoveHtmlTags(info)).Trim();
            char lastChar = '\0';
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i] == '\n')
                {
                    if (lastChar == '\n')
                    {
                        i -= 1;
                        info = info.Remove(i);
                        break;
                    }
                }
                lastChar = info[i];
            }
        }
    }
}
