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
        public int totalSubscribers;
        List<Subscription> subscribedUsers;

        public WebUrl nextSubs;

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
            subscribedUsers = new List<Subscription>();
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
            subscribedUsers = new List<Subscription>();
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

        Subscription GetSubscriber(string username)
        {
            foreach (Subscription sub in subscribedUsers)
            {
                if (sub.user.name == username)
                {
                    return sub;
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

        bool ContainsSubscriber(string username)
        {
            foreach (Subscription sub in subscribedUsers)
            {
                if (sub.user.name == username)
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

        public async Task<List<Subscription>> RetriveSubscribers(bool getNext, Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/subscriptions");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (responseString != "422")
                {
                    totalSubscribers = (int)JObject.Parse(responseString)["_total"];
                    nextSubs = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                    foreach (JObject o in (JArray)JObject.Parse(responseString)["subscriptions"])
                    {
                        if (!ContainsSubscriber((string)o["user"]["name"]))
                        {
                            subscribedUsers.Add(LoadSubscriber(o, twixel));
                        }
                    }
                }
                else
                {
                    errorString = "You aren't partnered so you cannot have subs";
                }
            }

            return null;
        }

        public async Task<List<Subscription>> RetriveSubscribers(int limit, TwitchConstants.Direction direction, Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
            {
                Uri uri;
                string url = "https://api.twitch.tv/kraken/channels/" + name + "/subscriptions";
                if (limit <= 100)
                {
                    url += "?limit=" + limit.ToString();
                }
                else
                {
                    url += "?limit=100";
                    errorString = "You cannot fetch more than 100 subs at a time";
                }

                if (direction != TwitchConstants.Direction.None)
                {
                    url += "&direction=" + TwitchConstants.DirectionToString(direction);
                }

                uri = new Uri(url);
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (responseString != "422")
                {
                    totalSubscribers = (int)JObject.Parse(responseString)["_total"];
                    nextSubs = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                    foreach (JObject o in (JArray)JObject.Parse(responseString)["subscriptions"])
                    {
                        if (!ContainsSubscriber((string)o["user"]["name"]))
                        {
                            subscribedUsers.Add(LoadSubscriber(o, twixel));
                        }
                    }
                }
                else
                {
                    errorString = "You aren't partnered so you cannot have subs";
                }
            }

            return null;
        }

        public async Task<Subscription> RetrieveSubsciber(string username, Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelCheckSubscription))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/subscriptions/" + username);
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (responseString != "422" && responseString != "404")
                {
                    return LoadSubscriber(JObject.Parse(responseString), twixel);
                }
                else if (responseString == "404")
                {
                    errorString = username + " is not subscribed";
                }
                else if (responseString == "422")
                {
                    errorString = "You aren't partnered so you cannot have subs";
                }
            }

            return null;
        }

        public async Task<Subscription> RetrieveSubscription(string channel, Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserSubcriptions))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/subscriptions/" + channel);
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (responseString != "422" && responseString != "404")
                {
                    return LoadSubscription(JObject.Parse(responseString), twixel);
                }
                else if (responseString == "404")
                {
                    errorString = "You are not subscribed to " + channel;
                }
                else if (responseString == "422")
                {
                    errorString = channel + " has no subscription program";
                }
            }

            return null;
        }

        Subscription LoadSubscriber(JObject o, Twixel twixel)
        {
            Subscription sub = new Subscription((string)o["_id"], (JObject)o["user"], (string)o["created_at"], twixel);
            return sub;
        }

        Subscription LoadSubscription(JObject o, Twixel twixel)
        {
            Subscription sub = new Subscription((string)o["_id"], (string)o["created_at"], (JObject)o["channel"], twixel);
            return sub;
        }
    }
}
