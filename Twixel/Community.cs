using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Community : TwixelObjectBase
    {
        public string id;
        public Uri avatarImageUrl;
        public Uri coverImageUrl;
        public string description;
        public string descriptionHtml;
        public string language;
        public string name;
        public long ownerId;
        public string rules;
        public string rulesHtml;
        public string summary;

        public Community(string id,
            string avatarImageUrl,
            string coverImageUrl,
            string description,
            string descriptionHtml,
            string language,
            string name,
            long ownerId,
            string rules,
            string rulesHtml,
            string summary,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.id = id;
            if (!string.IsNullOrEmpty(avatarImageUrl))
            {
                this.avatarImageUrl = new Uri(avatarImageUrl);
            }
            if (!string.IsNullOrEmpty(coverImageUrl))
            {
                this.coverImageUrl = new Uri(coverImageUrl);
            }
            this.description = description;
            this.descriptionHtml = descriptionHtml;
            this.language = language;
            this.name = name;
            this.ownerId = ownerId;
            this.rules = rules;
            this.rulesHtml = rulesHtml;
            this.summary = summary;
        }

        public static Community LoadCommunity(JObject o, Twixel.APIVersion version)
        {
            return new Community((string)o["_id"],
                (string)o["avatar_image_url"],
                (string)o["cover_image_url"],
                (string)o["description"],
                (string)o["description_html"],
                (string)o["language"],
                (string)o["name"],
                (long)o["owner_id"],
                (string)o["rules"],
                (string)o["rules_html"],
                (string)o["summary"],
                null);
        }
    }
}
