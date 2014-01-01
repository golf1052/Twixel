using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using TwixelAPI.Constants;

namespace TwixelAPI
{
    public class User
    {
        public bool authorized;
        public string accessToken = "";
        public List<TwitchConstants.Scope> authorizedScopes;
        public string name;
        public WebUrl logo;
        public long id;
        public string displayName;
        public string email;
        public bool? staff;
        public bool? partnered;
        List<Stream> followedStreams;

        public User(string accessToken,
            List<TwitchConstants.Scope> authorizedScopes,
            string name,
            string logo,
            long id,
            string displayName,
            string email,
            bool? staff,
            bool? partnered)
        {
            authorized = true;
            this.accessToken = accessToken;
            this.authorizedScopes = authorizedScopes;
            this.name = name;
            this.logo = new WebUrl(logo);
            this.id = id;
            this.displayName = displayName;
            if (email != null)
            {
                this.email = email;
            }
            if (staff != null)
            {
                this.staff = staff;
            }
            if (partnered != null)
            {
                this.partnered = partnered;
            }
        }

        public User(string name,
            string logo,
            long id,
            string displayName,
            bool? staff)
        {
            authorized = false;
            this.name = name;
            this.logo = new WebUrl(logo);
            this.id = id;
            this.displayName = displayName;
            if (staff != null)
            {
                this.staff = staff;
            }
        }

        // Only returns the streams that are currently online
        public async Task<List<Stream>> RetriveFollowedStreams(Twixel twixel)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/streams/followed");
            string responseString = await Twixel.GetWebData(uri, accessToken);
            followedStreams = twixel.LoadStreams(JObject.Parse(responseString));
            return followedStreams;
        }
    }
}
