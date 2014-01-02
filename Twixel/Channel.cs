using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace TwixelAPI
{
    public class Channel
    {
        public string mature;
        public WebUrl background;
        public string updatedAt;
        public long id;
        public string status;
        public WebUrl logo;
        public List<Team> teams;
        public WebUrl url;
        public string displayName;
        public string game;
        public WebUrl banner;
        public string name;
        public WebUrl videoBanner;

        public WebUrl chat;
        public WebUrl subscriptions;
        public WebUrl features;
        public WebUrl commercial;
        public WebUrl streamKey;
        public WebUrl editors;
        public WebUrl videos;
        public WebUrl self;
        public WebUrl follows;

        public string createdAt;

        public Channel(string mature,
            string background,
            string updatedAt,
            long id,
            JArray teamsA,
            string status,
            string logo,
            string url,
            string displayName,
            string game,
            string banner,
            string name,
            string videoBanner,
            string chat,
            string subscriptions,
            string features,
            string commercial,
            string streamKey,
            string editors,
            string videos,
            string self,
            string follows,
            string createdAt,
            Twixel twixel)
        {
            teams = new List<Team>();
            if (teamsA != null)
            {
                foreach (JObject team in teamsA)
                {
                    teams.Add(twixel.LoadTeam(team));
                }
            }
            this.mature = mature;
            if (background != null)
            {
                this.background = new WebUrl(background);
            }
            this.updatedAt = updatedAt;
            this.id = id;
            this.status = status;
            if (logo != null)
            {
                this.logo = new WebUrl(logo);
            }
            this.url = new WebUrl(url);
            this.displayName = displayName;
            this.game = game;
            if (banner != null)
            {
                this.banner = new WebUrl(banner);
            }
            this.name = name;
            if (videoBanner != null)
            {
                this.videoBanner = new WebUrl(videoBanner);
            }
            this.chat = new WebUrl(chat);
            this.subscriptions = new WebUrl(subscriptions);
            this.features = new WebUrl(features);
            this.commercial = new WebUrl(commercial);
            this.streamKey = new WebUrl(streamKey);
            this.editors = new WebUrl(editors);
            this.videos = new WebUrl(videos);
            this.self = new WebUrl(self);
            this.follows = new WebUrl(follows);
            this.createdAt = createdAt;
        }
    }
}
