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
        List<User> blockedUsers;

        string errorString = "";
        public string ErrorString
        {
            get
            {
                return errorString;
            }
        }

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
            blockedUsers = new List<User>();
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
            blockedUsers = new List<User>();
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

        User GetBlockedUser(string username)
        {
            foreach (User user in blockedUsers)
            {
                if (user.name == username)
                {
                    return user;
                }
            }

            return null;
        }

        bool ContainsBlockedUser(string username)
        {
            foreach (User user in blockedUsers)
            {
                if (user.name == username)
                {
                    return true;
                }
            }

            return false;
        }

        // Only returns the streams that are currently online
        public async Task<List<Stream>> RetriveFollowedStreams(Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserRead))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/streams/followed");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                followedStreams = twixel.LoadStreams(JObject.Parse(responseString));
                return followedStreams;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<User>> RetrieveBlockedUsers(Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserBlocksRead))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/blocks");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                foreach (JObject user in JObject.Parse(responseString)["blocks"])
                {
                    User temp = twixel.LoadUser((JObject)user["user"]);
                    if (!ContainsBlockedUser(temp.name))
                    {
                        blockedUsers.Add(temp);
                    }
                }
                return blockedUsers;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<User>> BlockUser(string username, Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserBlocksEdit))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/blocks/" + username);
                string responseString = await Twixel.PutWebData(uri, accessToken, "");
                User temp = twixel.LoadUser((JObject)JObject.Parse(responseString)["user"]);
                if (!ContainsBlockedUser(temp.name))
                {
                    blockedUsers.Add(temp);
                }

                return blockedUsers;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<User>> UnblockUser(string username, Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserBlocksEdit))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/blocks/" + username);
                string responseString = await Twixel.DeleteWebData(uri, accessToken);
                if (responseString == "")
                {
                    blockedUsers.Remove(GetBlockedUser(username));
                    return blockedUsers;
                }
                else if (responseString == "404")
                {
                    errorString = username + " was never blocked";
                    return blockedUsers;
                }
                else if (responseString == "422")
                {
                    errorString = username + " could not be deleted. Try again.";
                    return blockedUsers;
                }
            }

            return null;
        }
    }
}
