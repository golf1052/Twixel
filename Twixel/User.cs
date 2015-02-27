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
    /// <summary>
    /// User class
    /// </summary>
    public class User
    {
        Twixel twixel;

        /// <summary>
        /// Authorization status
        /// </summary>
        public bool authorized;

        /// <summary>
        /// User's access token
        /// </summary>
        public string accessToken = "";

        /// <summary>
        /// List of scopes user has given permission to
        /// </summary>
        public List<TwitchConstants.Scope> authorizedScopes;

        /// <summary>
        /// Name of the user
        /// </summary>
        public string name;

        /// <summary>
        /// User's logo
        /// </summary>
        public Uri logo;

        /// <summary>
        /// User's ID
        /// </summary>
        public long id;

        /// <summary>
        /// User's public display name
        /// </summary>
        public string displayName;

        /// <summary>
        /// User's email address
        /// </summary>
        public string email;

        /// <summary>
        /// If the user is a staff member of Twitch
        /// </summary>
        public bool? staff;

        /// <summary>
        /// If the user is partnered with Twitch
        /// </summary>
        public bool? partnered;

        /// <summary>
        /// When the user was created
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// When the user was last updated
        /// </summary>
        public DateTime updatedAt;

        List<Stream> followedStreams;
        List<User> blockedUsers;

        /// <summary>
        /// The number of subscribers this user has
        /// </summary>
        public int totalSubscribers;

        List<Subscription> subscribedUsers;
        Channel channel;
        List<User> channelEditors;

        /// <summary>
        /// User's stream key
        /// </summary>
        public string streamKey;

        /// <summary>
        /// User's bio
        /// </summary>
        public string bio;

        /// <summary>
        /// Next subscribers URL
        /// </summary>
        public Uri nextSubs;

        /// <summary>
        /// Next following URL
        /// </summary>
        public Uri nextFollowing;

        /// <summary>
        /// Next following streams URL
        /// </summary>
        public Uri nextFollowingStreams;

        internal User(Twixel twixel,
            string accessToken,
            List<TwitchConstants.Scope> authorizedScopes,
            string name,
            string logo,
            long id,
            string displayName,
            string email,
            bool? staff,
            bool? partnered,
            string createdAt,
            string updatedAt,
            string bio)
        {
            this.twixel = twixel;
            blockedUsers = new List<User>();
            subscribedUsers = new List<Subscription>();
            channelEditors = new List<User>();
            authorized = true;
            this.accessToken = accessToken;
            this.authorizedScopes = authorizedScopes;
            this.name = name;
            if (logo != null)
            {
                this.logo = new Uri(logo);
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
            if (bio != null)
            {
                this.bio = bio;
            }
        }

        internal User(Twixel twixel,
            string name,
            string logo,
            long id,
            string displayName,
            bool? staff,
            string createdAt,
            string updatedAt,
            string bio)
        {
            blockedUsers = new List<User>();
            subscribedUsers = new List<Subscription>();
            channelEditors = new List<User>();
            authorized = false;
            this.name = name;
            if (logo != null)
            {
                this.logo = new Uri(logo);
            }
            this.id = id;
            this.displayName = displayName;
            if (staff != null)
            {
                this.staff = staff;
            }
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAt = DateTime.Parse(updatedAt);
            if (bio != null)
            {
                this.bio = bio;
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

        /// <summary>
        /// Gets a list of live streams that the user is following. Requires user authorization.
        /// </summary>
        /// <returns>A list of streams</returns>
        public async Task<List<Stream>> RetrieveOnlineFollowedStreams()
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
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserRead))
                {
                    twixel.CreateError(name + " has not given user_read permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Gets a list of users that the user has blocked. Requires user authorization.
        /// </summary>
        /// <returns>A list of users</returns>
        public async Task<List<User>> RetrieveBlockedUsers()
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
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserBlocksRead))
                {
                    twixel.CreateError(name + " has not given user_blocks_read permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Blocks a user. Requires user authorization.
        /// </summary>
        /// <param name="username">The name of the user to block</param>
        /// <returns>The current list of blocked users</returns>
        public async Task<List<User>> BlockUser(string username)
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
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserBlocksEdit))
                {
                    twixel.CreateError(name + " has not given user_blocks_edit permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Unblocks a user. Requires user authorization.
        /// </summary>
        /// <param name="username">The name of the user to block</param>
        /// <returns>The current list of blocked users</returns>
        public async Task<List<User>> UnblockUser(string username)
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
                    twixel.CreateError(username + "was never blocked");
                    return null;
                }
                else if (responseString == "422")
                {
                    twixel.CreateError(username + " could not be unblocked. Try again.");
                    return null;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserBlocksEdit))
                {
                    twixel.CreateError(name + " has not given user_blocks_edit permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Gets a list of users subscribed to this user. Requires user authorization. Requires Twitch partnership.
        /// </summary>
        /// <param name="getNext">If this method was called before then this will get the next page of subscriptions</param>
        /// <returns>A list of subscriptions</returns>
        public async Task<List<Subscription>> RetriveSubscribers(bool getNext)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/subscriptions");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (responseString != "422")
                {
                    totalSubscribers = (int)JObject.Parse(responseString)["_total"];
                    nextSubs = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
                    foreach (JObject o in (JArray)JObject.Parse(responseString)["subscriptions"])
                    {
                        if (!ContainsSubscriber((string)o["user"]["name"]))
                        {
                            subscribedUsers.Add(LoadSubscriber(o));
                        }
                    }
                    return subscribedUsers;
                }
                else
                {
                    twixel.CreateError("You aren't partnered so you cannot have subs");
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
                {
                    twixel.CreateError(name + " has not given channel_subscriptions permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Get a list of users subscribed to this user. Requires user authorization. Requires Twitch partnership.
        /// </summary>
        /// <param name="limit">How many subscriptions to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="direction">Creation date sorting direction</param>
        /// <returns>A list of subscriptions</returns>
        public async Task<List<Subscription>> RetriveSubscribers(int limit, TwitchConstants.Direction direction)
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
                    twixel.CreateError("You cannot fetch more than 100 subs at a time");
                    return null;
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
                    nextSubs = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
                    foreach (JObject o in (JArray)JObject.Parse(responseString)["subscriptions"])
                    {
                        if (!ContainsSubscriber((string)o["user"]["name"]))
                        {
                            subscribedUsers.Add(LoadSubscriber(o));
                        }
                    }
                    return subscribedUsers;
                }
                else
                {
                    twixel.CreateError("You aren't partnered so you cannot have subs");
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
                {
                    twixel.CreateError(name + " has not given channel_subscriptions permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Checks to see if a specified user is subscribed to this user. Requires user authorization. Requires Twitch partnership.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>A subscription object if the user is subscribed</returns>
        public async Task<Subscription> RetrieveSubsciber(string username)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelCheckSubscription))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/subscriptions/" + username);
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (responseString != "422" && responseString != "404")
                {
                    return LoadSubscriber(JObject.Parse(responseString));
                }
                else if (responseString == "404")
                {
                    twixel.CreateError(username + " is not subscribed");
                    return null;
                }
                else if (responseString == "422")
                {
                    twixel.CreateError("You aren't partnered so you cannot have subs");
                    return null;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelCheckSubscription))
                {
                    twixel.CreateError(name + " has not given channel_check_subscription permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Checks to see if this user is subcribed to a specified channel. Requires user authorization.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>A subscription object</returns>
        public async Task<Subscription> RetrieveSubscription(string channel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserSubcriptions))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/subscriptions/" + channel);
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (responseString != "422" && responseString != "404")
                {
                    return LoadSubscription(JObject.Parse(responseString));
                }
                else if (responseString == "404")
                {
                    twixel.CreateError("You are not subscribed to " + channel);
                    return null;
                }
                else if (responseString == "422")
                {
                    twixel.CreateError(channel + " has no subscription program");
                    return null;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserSubcriptions))
                {
                    twixel.CreateError(name + " has not given user_subscriptions permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Gets a list of channels this user is following
        /// </summary>
        /// <param name="getNext">If this method was called before then this will get the next page of channels</param>
        /// <returns>A list of channels</returns>
        public async Task<List<Channel>> RetrieveFollowing(bool getNext)
        {
            Uri uri;
            if (!getNext)
            {
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels");
            }
            else
            {
                if (nextFollowing != null)
                {
                    uri = nextFollowing.url;
                }
                else
                {
                    uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels");
                }
            }
            string responseString = await Twixel.GetWebData(uri);
            if (Twixel.GoodStatusCode(responseString))
            {
                nextFollowing = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
                List<Channel> followedChannels = new List<Channel>();
                foreach (JObject o in (JArray)JObject.Parse(responseString)["follows"])
                {
                    followedChannels.Add(twixel.LoadChannel((JObject)o["channel"]));
                }
                return followedChannels;
            }
            else
            {
                twixel.CreateError(responseString);
                return null;
            }
        }

        /// <summary>
        /// Gets a list of channels this user is following
        /// </summary>
        /// <param name="limit">How many channels to get at one time. Default is 25. Maximum is 100</param>
        /// <returns>A list of channels</returns>
        public async Task<List<Channel>> RetrieveFollowing(int limit)
        {
            Uri uri;
            if (limit <= 100)
            {
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels?limit=" + limit.ToString());
            }
            else
            {
                twixel.CreateError("You cannot get more than 100 channels at a time");
                return null;
            }
            string responseString = await Twixel.GetWebData(uri);
            if (Twixel.GoodStatusCode(responseString))
            {
                nextFollowing = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
                List<Channel> followedChannels = new List<Channel>();
                foreach (JObject o in (JArray)JObject.Parse(responseString)["follows"])
                {
                    followedChannels.Add(twixel.LoadChannel((JObject)o["channel"]));
                }
                return followedChannels;
            }
            else
            {
                twixel.CreateError(responseString);
                return null;
            }
        }

        /// <summary>
        /// Checks to see if this user is following a specified channel
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>A channel object</returns>
        public async Task<Channel> RetrieveFollowing(string channel)
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
                twixel.CreateError(name + " is not following " + channel);
                return null;
            }
            else
            {
                twixel.CreateError(responseString);
                return null;
            }
        }

        /// <summary>
        /// Follows a channel. Requires user authorization.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>A channel object</returns>
        public async Task<Channel> FollowChannel(string channel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels/" + channel);
                string responseString = await Twixel.PutWebData(uri, accessToken, "");
                if (responseString != "422")
                {
                    Channel temp = twixel.LoadChannel((JObject)JObject.Parse(responseString)["channel"]);
                    return temp;
                }
                else if (responseString == "422")
                {
                    twixel.CreateError("Could not follow " + channel);
                    return null;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
                {
                    twixel.CreateError(name + " has not given user_follows_edit permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Unfollows a channel. Requires user authorization.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>If the request succeeded</returns>
        public async Task<bool> UnfollowChannel(string channel)
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels/" + channel);
                string responseString = await Twixel.DeleteWebData(uri, accessToken);
                if (responseString == "")
                {
                    return true;
                }
                else if (responseString == "404")
                {
                    twixel.CreateError(channel + "was not being followed");
                    return false;
                }
                else if (responseString == "422")
                {
                    twixel.CreateError(channel + " could not be unfollowed. Try again.");
                    return false;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return false;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
                {
                    twixel.CreateError(name + " has not given user_follows_edit permissions");
                }
                return false;
            }
        }

        /// <summary>
        /// Gets the channel object for this user. Requires user authorization.
        /// </summary>
        /// <returns>A channel object</returns>
        public async Task<Channel> RetrieveChannel()
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelRead))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channel");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (Twixel.GoodStatusCode(responseString))
                {
                    streamKey = (string)JObject.Parse(responseString)["stream_key"];
                    channel = twixel.LoadChannel(JObject.Parse(responseString));
                    return channel;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelRead))
                {
                    twixel.CreateError(name + " has not given channel_read permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the list of users that can edit this user's channel. Requires user authorization.
        /// </summary>
        /// <returns>A list of users</returns>
        public async Task<List<User>> RetrieveChannelEditors()
        {
            if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelRead))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/editors");
                string responseString = await Twixel.GetWebData(uri, accessToken);
                if (Twixel.GoodStatusCode(responseString))
                {
                    foreach (JObject o in (JArray)JObject.Parse(responseString)["users"])
                    {
                        if (!ContainsEditor((string)o["name"]))
                        {
                            channelEditors.Add(twixel.LoadUser(o));
                        }
                    }
                    return channelEditors;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelRead))
                {
                    twixel.CreateError(name + " has not given channel_read permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Updates a channel's status. Requires user authorization.
        /// </summary>
        /// <param name="status">The new status</param>
        /// <param name="game">The new game</param>
        /// <returns></returns>
        public async Task<Channel> UpdateChannel(string status, string game)
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
                if (Twixel.GoodStatusCode(responseString))
                {
                    return twixel.LoadChannel(JObject.Parse(responseString));
                }
                else
                {
                    twixel.CreateError(responseString);
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelEditor))
                {
                    twixel.CreateError(name + " has not given channel_editor permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Resets this user's stream key. Requires user authorization.
        /// </summary>
        /// <returns>The new stream key</returns>
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
                    twixel.CreateError("Error reseting stream key");
                    return null;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return null;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelStream))
                {
                    twixel.CreateError(name + " has not given channel_stream permissions");
                }
                return null;
            }
        }

        /// <summary>
        /// Starts a commercial on this user's live stream. Requires user authorization. Requires Twitch partnership
        /// </summary>
        /// <param name="length">The length of the commercial</param>
        /// <returns>If the request succeeded</returns>
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
                    twixel.CreateError("You are not partnered so you cannot run commercials");
                    return false;
                }
                else
                {
                    twixel.CreateError(responseString);
                    return false;
                }
            }
            else
            {
                if (!authorized)
                {
                    twixel.CreateError(name + " is not authorized");
                }
                else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelCommercial))
                {
                    twixel.CreateError(name + " has not given channel_commercial permissions");
                }
                return false;
            }
        }

        Subscription LoadSubscriber(JObject o)
        {
            Subscription sub = new Subscription((string)o["_id"], (JObject)o["user"], (string)o["created_at"], twixel);
            return sub;
        }

        Subscription LoadSubscription(JObject o)
        {
            Subscription sub = new Subscription((string)o["_id"], (string)o["created_at"], (JObject)o["channel"], twixel);
            return sub;
        }
    }
}
