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
        public DateTime createdAt;
        public DateTime updatedAt;
        List<Stream> followedStreams;
        List<Channel> followedChannels;
        List<User> blockedUsers;
        public int totalSubscribers;
        List<Subscription> subscribedUsers;
        Channel channel;
        List<User> channelEditors;
        public string streamKey;

        public WebUrl nextSubs;
        public WebUrl nextFollows;

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
            bool? partnered,
            string createdAt,
            string updatedAt)
        {
            blockedUsers = new List<User>();
            subscribedUsers = new List<Subscription>();
            followedChannels = new List<Channel>();
            channelEditors = new List<User>();
            authorized = true;
            this.accessToken = accessToken;
            this.authorizedScopes = authorizedScopes;
            this.name = name;
            if (logo != null)
            {
                this.logo = new WebUrl(logo);
            }
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
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAt = DateTime.Parse(updatedAt);
        }

        public User(string name,
            string logo,
            long id,
            string displayName,
            bool? staff,
            string createdAt,
            string updatedAt)
        {
            blockedUsers = new List<User>();
            subscribedUsers = new List<Subscription>();
            followedChannels = new List<Channel>();
            channelEditors = new List<User>();
            authorized = false;
            this.name = name;
            if (logo != null)
            {
                this.logo = new WebUrl(logo);
            }
            this.id = id;
            this.displayName = displayName;
            if (staff != null)
            {
                this.staff = staff;
            }
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAt = DateTime.Parse(updatedAt);
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

        bool ContainsEditor(string username)
        {
            foreach (User user in channelEditors)
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

        Channel GetChannel(string name)
        {
            foreach (Channel channel in followedChannels)
            {
                if (channel.name == name)
                {
                    return channel;
                }
            }

            return null;
        }

        bool ContainsChannel(string name)
        {
            foreach (Channel channel in followedChannels)
            {
                if (channel.name == name)
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
                    errorString = username + " could not be unblocked. Try again.";
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

        public async Task<List<Channel>> RetrieveFollowing(bool getNext, Twixel twixel)
        {
            Uri uri;
            if (!getNext)
            {
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels");
            }
            else
            {
                if (nextFollows != null)
                {
                    uri = nextFollows.url;
                }
                else
                {
                    uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels");
                }
            }
            string responseString = await Twixel.GetWebData(uri);
            nextFollows = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
            foreach (JObject o in (JArray)JObject.Parse(responseString)["follows"])
            {
                if (!ContainsChannel((string)o["channel"]["name"]))
                {
                    followedChannels.Add(twixel.LoadChannel((JObject)o["channel"]));
                }
            }
            return followedChannels;
        }

        public async Task<List<Channel>> RetrieveFollowing(int limit, Twixel twixel)
        {
            Uri uri;
            if (limit <= 100)
            {
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels&limit=" + limit.ToString());
            }
            else
            {
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels&limit=100");
                errorString = "You cannot get more than 100 channels at a time";
            }
            string responseString = await Twixel.GetWebData(uri);
            nextFollows = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
            foreach (JObject o in (JArray)JObject.Parse(responseString)["follows"])
            {
                if (!ContainsChannel((string)o["channel"]["name"]))
                {
                    followedChannels.Add(twixel.LoadChannel((JObject)o["channel"]));
                }
            }
            return followedChannels;
        }

        public async Task<Channel> RetrieveFollowing(string channel, Twixel twixel)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels/" + channel);
            string responseString = await Twixel.GetWebData(uri);
            if (responseString != "404")
            {
                return twixel.LoadChannel((JObject)JObject.Parse(responseString)["channel"]);
            }
            else if (responseString == "404")
            {
                errorString = name + " is not following " + channel;
            }

            return null;
        }

        public async Task<Channel> FollowChannel(string channel, Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels/" + channel);
                string responseString = await Twixel.PutWebData(uri, accessToken, "");
                if (responseString != "422")
                {
                    Channel temp = twixel.LoadChannel((JObject)JObject.Parse(responseString)["channel"]);
                    if (!ContainsChannel((string)JObject.Parse(responseString)["channel"]["name"]))
                    {
                        followedChannels.Add(temp);
                    }
                    return temp;
                }
                else if (responseString == "422")
                {
                    errorString = "Could not follow " + channel;
                }
            }

            return null;
        }

        public async Task<List<Channel>> UnfollowChannel(string channel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels/" + channel);
                string responseString = await Twixel.DeleteWebData(uri, accessToken);
                if (responseString == "")
                {
                    followedChannels.Remove(GetChannel(channel));
                    return followedChannels;
                }
                else if (responseString == "404")
                {
                    errorString = channel + " was never followed";
                    return followedChannels;
                }
                else if (responseString == "422")
                {
                    errorString = channel + " could not be unfollowed. Try again.";
                }
            }

            return null;
        }

        public async Task<Channel> RetrieveChannel(Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelRead))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channel");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                streamKey = (string)JObject.Parse(responseString)["stream_key"];
                channel = twixel.LoadChannel(JObject.Parse(responseString));
                return channel;
            }

            return null;
        }

        public async Task<List<User>> RetrieveChannelEditors(Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelRead))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/editors");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                foreach (JObject o in (JArray)JObject.Parse(responseString)["users"])
                {
                    if (!ContainsEditor((string)o["name"]))
                    {
                        channelEditors.Add(twixel.LoadUser(o));
                    }
                }

                return channelEditors;
            }

            return null;
        }

        public async Task<Channel> UpdateChannel(string status, string game, Twixel twixel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelEditor))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + name);
                JObject content = new JObject();
                content["channel"] = new JObject();
                content["channel"]["status"] = status;
                content["channel"]["game"] = game;
                string responseString = await Twixel.PutWebData(uri, accessToken, content.ToString());
                return twixel.LoadChannel(JObject.Parse(responseString));
            }

            return null;
        }

        public async Task<string> ResetStreamKey()
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelStream))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/stream_key");
                string responseString = await Twixel.DeleteWebData(uri, accessToken);
                if (responseString != "422")
                {
                    streamKey = (string)JObject.Parse(responseString)["stream_key"];
                    return streamKey;
                }
                else if (responseString == "422")
                {
                    errorString = "Error reseting stream key";
                }
            }

            return null;
        }

        public async Task<bool> StartCommercial(TwitchConstants.Length length)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelCommercial))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/test_user1/commercial");
                string responseString = await Twixel.PostWebData(uri, accessToken, "length=" + TwitchConstants.LengthToInt(length).ToString());
                if (responseString == "")
                {
                    return true;
                }
                else if (responseString == "422")
                {
                    errorString = "You are not partnered so you cannot run commercials";
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> RetrieveAuthorizationStatus()
        {
            if (authorized)
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                return (bool)JObject.Parse(responseString)["token"]["valid"];
            }
            else
            {
                return false;
            }
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
