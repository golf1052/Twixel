using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using TwixelAPI.Constants;
using Flurl;

namespace TwixelAPI
{
    public struct Notification
    {
        public readonly bool email;
        public readonly bool push;

        public Notification(bool email, bool push)
        {
            this.email = email;
            this.push = push;
        }
    }

    /// <summary>
    /// User class
    /// </summary>
    public class User : TwixelObjectBase
    {
        /// <summary>
        /// Authorization status
        /// </summary>
        public readonly bool authorized;

        /// <summary>
        /// User's access token
        /// </summary>
        public string accessToken;

        /// <summary>
        /// List of scopes user has given permission to
        /// </summary>
        public List<TwitchConstants.Scope> authorizedScopes;

        /// <summary>
        /// v2/v3
        /// Name of the user
        /// </summary>
        public string name;

        /// <summary>
        /// v2/v3
        /// User's logo
        /// </summary>
        public Uri logo;

        /// <summary>
        /// v2/v3
        /// User's ID
        /// </summary>
        public long id;

        /// <summary>
        /// v2/v3
        /// User's public display name
        /// </summary>
        public string displayName;

        /// <summary>
        /// v2/v3
        /// User's email address
        /// </summary>
        public string email;

        /// <summary>
        /// v2
        /// If the user is a staff member of Twitch
        /// </summary>
        public bool staff;

        /// <summary>
        /// v3
        /// User's status on Twitch
        /// </summary>
        public string type;

        /// <summary>
        /// v2/v3
        /// If the user is partnered with Twitch
        /// </summary>
        public bool partnered;

        /// <summary>
        /// v2/v3
        /// When the user was created
        /// </summary>
        public DateTime createdAt;

        /// <summary>
        /// v2/v3
        /// When the user was last updated
        /// </summary>
        public DateTime updatedAt;

        /// <summary>
        /// v3
        /// User's bio
        /// </summary>
        public string bio;

        /// <summary>
        /// v2/v3
        /// Notification settings for user
        /// </summary>
        public Notification notifications;

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
        /// Load an unauthed v2 user
        /// </summary>
        /// <param name="displayName">Display name</param>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="staff">Are they staff</param>
        /// <param name="createdAt">Time created at</param>
        /// <param name="updatedAt">Time updated at</param>
        /// <param name="logo">Logo link</param>
        /// <param name="baseLinksO">Base links</param>
        public User(string displayName,
            long id,
            string name,
            bool staff,
            string createdAt,
            string updatedAt,
            string logo,
            JObject baseLinksO) : base(baseLinksO)
        {
            Initv2(displayName, id, name, staff, createdAt, updatedAt, logo);
            this.authorized = false;
            this.accessToken = "";
            this.authorizedScopes = new List<TwitchConstants.Scope>();
        }

        /// <summary>
        /// Load an authed v2 user
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="authorizedScopes">List of authorized scopes</param>
        /// <param name="displayName">Display name</param>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="staff">Are they staff</param>
        /// <param name="createdAt">Time created at</param>
        /// <param name="updatedAt">Time updated at</param>
        /// <param name="logo">Logo link</param>
        /// <param name="email">Email</param>
        /// <param name="partnered">Are they partnered</param>
        /// <param name="notificationsO">Notification status</param>
        /// <param name="baseLinksO">Base links</param>
        public User(string accessToken,
            List<TwitchConstants.Scope> authorizedScopes,
            string displayName,
            long id,
            string name,
            bool staff,
            string createdAt,
            string updatedAt,
            string logo,
            string email,
            bool partnered,
            JObject notificationsO,
            JObject baseLinksO) : base(baseLinksO)
        {
            InitAuth(accessToken, authorizedScopes, email, partnered, notificationsO);
            Initv2(displayName, id, name, staff, createdAt, updatedAt, logo);
            this.authorized = true;
        }

        private void Initv2(string displayName,
            long id,
            string name,
            bool staff,
            string createdAt,
            string updatedAt,
            string logo)
        {
            this.version = Twixel.APIVersion.v2;
            this.displayName = displayName;
            this.id = id;
            this.name = name;
            this.staff = staff;
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAt = DateTime.Parse(updatedAt);
            if (!string.IsNullOrEmpty(logo))
            {
                this.logo = new Uri(logo);
            }
            BaseInit();
        }

        /// <summary>
        /// Load an unauthed v3 user
        /// </summary>
        /// <param name="displayName">Display name</param>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="type">Type of user</param>
        /// <param name="bio">Bio</param>
        /// <param name="createdAt">Time created at</param>
        /// <param name="updatedAt">Time updated at</param>
        /// <param name="logo">Logo link</param>
        /// <param name="baseLinksO">Base links</param>
        public User(string displayName,
            long id,
            string name,
            string type,
            string bio,
            string createdAt,
            string updatedAt,
            string logo,
            JObject baseLinksO) : base(baseLinksO)
        {
            Initv3(displayName, id, name, type, bio, createdAt, updatedAt, logo);
            this.authorized = false;
            this.accessToken = "";
            this.authorizedScopes = new List<TwitchConstants.Scope>();
        }

        /// <summary>
        /// Load an authed v3 user
        /// </summary>
        /// <param name="accessToken">Access token</param>
        /// <param name="authorizedScopes">List of authorized scopes</param>
        /// <param name="displayName">Display name</param>
        /// <param name="id">ID</param>
        /// <param name="name">Name</param>
        /// <param name="type">Type of user</param>
        /// <param name="bio">Bio</param>
        /// <param name="createdAt">Time created at</param>
        /// <param name="updatedAt">Time updated at</param>
        /// <param name="logo">Logo link</param>
        /// <param name="email">Email</param>
        /// <param name="partnered">Are they partnered</param>
        /// <param name="notificationsO">Notification status</param>
        /// <param name="baseLinksO">Base links</param>
        public User(string accessToken,
            List<TwitchConstants.Scope> authorizedScopes,
            string displayName,
            long id,
            string name,
            string type,
            string bio,
            string createdAt,
            string updatedAt,
            string logo,
            string email,
            bool partnered,
            JObject notificationsO,
            JObject baseLinksO) : base(baseLinksO)
        {
            InitAuth(accessToken, authorizedScopes, email, partnered, notificationsO);
            Initv3(displayName, id, name, type, bio, createdAt, updatedAt, logo);
            this.authorized = true;
        }

        private void Initv3(string displayName,
            long id,
            string name,
            string type,
            string bio,
            string createdAt,
            string updatedAt,
            string logo)
        {
            this.version = Twixel.APIVersion.v3;
            this.displayName = displayName;
            this.id = id;
            this.name = name;
            this.type = type;
            this.bio = bio;
            this.createdAt = DateTime.Parse(createdAt);
            this.updatedAt = DateTime.Parse(updatedAt);
            if (!string.IsNullOrEmpty(logo))
            {
                this.logo = new Uri(logo);
            }
            BaseInit();
        }

        private void InitAuth(string accessToken,
            List<TwitchConstants.Scope> authorizedScopes,
            string email,
            bool partnered,
            JObject notificationsO)
        {
            this.email = email;
            this.partnered = partnered;
            this.notifications = LoadNotifications(notificationsO);
            this.accessToken = accessToken;
            this.authorizedScopes = authorizedScopes;
        }

        private void BaseInit()
        {
            followedStreams = new List<Stream>();
            blockedUsers = new List<User>();
            subscribedUsers = new List<Subscription>();
            channelEditors = new List<User>();
        }

        User GetBlockedUser(string username)
        {
            return blockedUsers.FirstOrDefault((user) => user.name == username);
        }

        Subscription GetSubscriber(string username)
        {
            return subscribedUsers.FirstOrDefault((sub) => sub.user.name == username);
        }

        bool ContainsBlockedUser(string username)
        {
            return blockedUsers.Any((user) => user.name == username);
        }

        bool ContainsEditor(string username)
        {
            return channelEditors.Any((user) => user.name == username);
        }

        bool ContainsSubscriber(string username)
        {
            return subscribedUsers.Any((sub) => sub.user.name == username);
        }

        private string NotAuthedError()
        {
            return name + " is not authorized.";
        }

        private string MissingPermissionError(TwitchConstants.Scope scope)
        {
            return name + " has not given " + TwitchConstants.ScopeToString(scope) + " permissions.";
        }

        private string NotPartneredError()
        {
            return name + " is not partnered with Twitch.";
        }

        /// <summary>
        /// Gets a list of users that the user has blocked. Requires user authorization.
        /// </summary>
        /// <returns>A list of users</returns>
        public async Task<List<User>> RetrieveBlockedUsers(int offset = 0, int limit = 25)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserBlocksRead;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                Url url = new Url(TwitchConstants.baseUrl).AppendPathSegments("users", name, "blocks");
                if (limit <= 100)
                {
                    url.SetQueryParam("limit", limit);
                }
                else
                {
                    url.SetQueryParam("limit", 100);
                }
                url.SetQueryParam("offset", offset);
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.GetWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                foreach (JObject user in JObject.Parse(responseString)["blocks"])
                {
                    User temp = HelperMethods.LoadUser((JObject)user["user"], version);
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
                    throw new TwixelException(NotAuthedError());
                }
                else if (!authorizedScopes.Contains(relevantScope))
                {
                    throw new TwixelException(MissingPermissionError(relevantScope));
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Blocks a user. Requires user authorization.
        /// </summary>
        /// <param name="username">The name of the user to block</param>
        /// <returns>The current list of blocked users</returns>
        public async Task<List<User>> BlockUser(string username)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserBlocksEdit;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                Url url = new Url(TwitchConstants.baseUrl).AppendPathSegments("users", name, "blocks", username);
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.PutWebData(uri, accessToken, "", version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                User temp = HelperMethods.LoadUser((JObject)JObject.Parse(responseString)["user"], version);
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
                    throw new TwixelException(NotAuthedError());
                }
                else if (!authorizedScopes.Contains(relevantScope))
                {
                    throw new TwixelException(MissingPermissionError(relevantScope));
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Unblocks a user. Requires user authorization.
        /// </summary>
        /// <param name="username">The name of the user to block</param>
        /// <returns>The current list of blocked users</returns>
        public async Task<List<User>> UnblockUser(string username)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserBlocksEdit;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                Url url = new Url(TwitchConstants.baseUrl).AppendPathSegments("users", name, "blocks", username);
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.DeleteWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    if (ex.Status == 404)
                    {
                        throw new TwixelException(username + " was never blocked", ex);
                    }
                    else if (ex.Status == 422)
                    {
                        throw new TwixelException(username + " could not be unblocked. Try again.", ex);
                    }
                    else
                    {
                        throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                    }
                }
                if (string.IsNullOrEmpty(responseString))
                {
                    blockedUsers.Remove(GetBlockedUser(username));
                    return blockedUsers;
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
            else
            {
                if (!authorized)
                {
                    throw new TwixelException(NotAuthedError());
                }
                else if (!authorizedScopes.Contains(relevantScope))
                {
                    throw new TwixelException(MissingPermissionError(relevantScope));
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Gets the channel object for this user. Also retrieves their stream key.
        /// Requires user authorization.
        /// </summary>
        /// <returns>A channel object</returns>
        public async Task<Channel> RetrieveChannel()
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelRead;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                Url url = new Url(TwitchConstants.baseUrl).AppendPathSegment("channel");
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.GetWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                JObject responseObject = JObject.Parse(responseString);
                streamKey = (string)responseObject["stream_key"];
                channel = HelperMethods.LoadChannel(responseObject, version);
                return channel;
            }
            else
            {
                if (!authorized)
                {
                    throw new TwixelException(NotAuthedError());
                }
                else if (!authorizedScopes.Contains(relevantScope))
                {
                    throw new TwixelException(MissingPermissionError(relevantScope));
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Gets the list of users that can edit this user's channel. Requires user authorization.
        /// </summary>
        /// <returns>A list of users</returns>
        public async Task<List<User>> RetrieveChannelEditors()
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelRead;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                Url url = new Url(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "editors");
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.GetWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                foreach (JObject o in (JArray)JObject.Parse(responseString)["users"])
                {
                    if (!ContainsEditor((string)o["name"]))
                    {
                        channelEditors.Add(HelperMethods.LoadUser(o, version));
                    }
                }
                return channelEditors;
            }
            else
            {
                if (!authorized)
                {
                    throw new TwixelException(NotAuthedError());
                }
                else if (!authorizedScopes.Contains(relevantScope))
                {
                    throw new TwixelException(MissingPermissionError(relevantScope));
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Updates a channel's status. Requires user authorization.
        /// </summary>
        /// <param name="status">The new status</param>
        /// <param name="game">The new game</param>
        /// <returns></returns>
        public async Task<Channel> UpdateChannel(string status = "", string game = "",
            int delay = 0)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelEditor;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                Url url = new Url(TwitchConstants.baseUrl).AppendPathSegments("channels", name);
                Uri uri = new Uri(url.ToString());
                JObject content = new JObject();
                content["channel"] = new JObject();
                content["channel"]["status"] = status;
                content["channel"]["game"] = game;
                if (version == Twixel.APIVersion.v3)
                {
                    content["channel"]["delay"] = delay;
                }
                string responseString;
                try
                {
                    responseString = await Twixel.PutWebData(uri, accessToken, content.ToString(), version);
                }
                catch (TwitchException ex)
                {
                    if (ex.Status == 422)
                    {
                        throw new TwixelException(name + " isn't allowed to use delay.", ex);
                    }
                    else
                    {
                        throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                    }
                }
                channel = HelperMethods.LoadChannel(JObject.Parse(responseString), version);
                return channel;
            }
            else
            {
                if (!authorized)
                {
                    throw new TwixelException(NotAuthedError());
                }
                else if (!authorizedScopes.Contains(relevantScope))
                {
                    throw new TwixelException(MissingPermissionError(relevantScope));
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Resets this user's stream key. Requires user authorization.
        /// </summary>
        /// <returns>The new stream key</returns>
        public async Task<string> ResetStreamKey()
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelStream;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                Url url = new Url(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "stream_key");
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.DeleteWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    if (ex.Status == 422)
                    {
                        throw new TwixelException("Error resetting stream key.", ex);
                    }
                    else
                    {
                        throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                    }
                }
                streamKey = (string)JObject.Parse(responseString)["stream_key"];
                return streamKey;
            }
            else
            {
                if (!authorized)
                {
                    throw new TwixelException(NotAuthedError());
                }
                else if (!authorizedScopes.Contains(relevantScope))
                {
                    throw new TwixelException(MissingPermissionError(relevantScope));
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Starts a commercial on this user's live stream.
        /// Requires user authorization. Requires Twitch partnership.
        /// </summary>
        /// <param name="length">The length of the commercial</param>
        /// <returns>If the request succeeded</returns>
        public async Task<bool> StartCommercial(TwitchConstants.Length length)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelCommercial;
            if (authorized && authorizedScopes.Contains(relevantScope) && partnered)
            {
                Url url = new Url(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "commercial");
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.PostWebData(uri, accessToken,
                        "length=" + TwitchConstants.LengthToInt(length).ToString(), version);
                }
                catch (TwitchException ex)
                {
                    if (ex.Status == 422)
                    {
                        throw new TwixelException(ex.Message, ex);
                    }
                    else
                    {
                        throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                    }
                }
                if (string.IsNullOrEmpty(responseString))
                {
                    return true;
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
            else
            {
                if (!authorized)
                {
                    throw new TwixelException(NotAuthedError());
                }
                else if (!authorizedScopes.Contains(relevantScope))
                {
                    throw new TwixelException(MissingPermissionError(relevantScope));
                }
                else if (!partnered)
                {
                    throw new TwixelException(NotPartneredError());
                }
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Gets a list of channels this user is following
        /// </summary>
        /// <param name="limit">How many channels to get at one time. Default is 25. Maximum is 100</param>
        /// <returns>A list of channels</returns>
        public async Task<Total<List<Follow<Channel>>>> RetrieveFollowing(int offset = 0, int limit = 25)
        {
            Url url = new Url(TwitchConstants.baseUrl).AppendPathSegments("users", name, "follows", "channels");
            if (limit <= 100)
            {
                url.SetQueryParam("limit", limit);
            }
            else
            {
                url.SetQueryParam("limit", 100);
            }
            url.SetQueryParam("offset", offset);
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await Twixel.GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            JObject responseObject = JObject.Parse(responseString);
            List<Follow<Channel>> channels = HelperMethods.LoadChannelFollows(responseObject, version);
            return HelperMethods.LoadTotal(responseObject, channels, version);
        }

        /// <summary>
        /// Gets a list of live streams that the user is following. Requires user authorization.
        /// </summary>
        /// <returns>A list of streams</returns>
        //public async Task<List<Stream>> RetrieveOnlineFollowedStreams()
        //{
        //    if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserRead))
        //    {
        //        Uri uri;
        //        uri = new Uri("https://api.twitch.tv/kraken/streams/followed");
        //        string responseString = await Twixel.GetWebData(uri, accessToken);
        //        followedStreams = twixel.LoadStreams(JObject.Parse(responseString));
        //        return followedStreams;
        //    }
        //    else
        //    {
        //        if (!authorized)
        //        {
        //            twixel.CreateError(name + " is not authorized");
        //        }
        //        else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserRead))
        //        {
        //            twixel.CreateError(name + " has not given user_read permissions");
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a list of users subscribed to this user. Requires user authorization. Requires Twitch partnership.
        /// </summary>
        /// <param name="getNext">If this method was called before then this will get the next page of subscriptions</param>
        /// <returns>A list of subscriptions</returns>
        //public async Task<List<Subscription>> RetriveSubscribers(bool getNext)
        //{
        //    if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
        //    {
        //        Uri uri;
        //        uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/subscriptions");
        //        string responseString = await Twixel.GetWebData(uri, accessToken);
        //        if (responseString != "422")
        //        {
        //            totalSubscribers = (int)JObject.Parse(responseString)["_total"];
        //            nextSubs = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //            foreach (JObject o in (JArray)JObject.Parse(responseString)["subscriptions"])
        //            {
        //                if (!ContainsSubscriber((string)o["user"]["name"]))
        //                {
        //                    subscribedUsers.Add(LoadSubscriber(o));
        //                }
        //            }
        //            return subscribedUsers;
        //        }
        //        else
        //        {
        //            twixel.CreateError("You aren't partnered so you cannot have subs");
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        if (!authorized)
        //        {
        //            twixel.CreateError(name + " is not authorized");
        //        }
        //        else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
        //        {
        //            twixel.CreateError(name + " has not given channel_subscriptions permissions");
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Get a list of users subscribed to this user. Requires user authorization. Requires Twitch partnership.
        /// </summary>
        /// <param name="limit">How many subscriptions to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="direction">Creation date sorting direction</param>
        /// <returns>A list of subscriptions</returns>
        //public async Task<List<Subscription>> RetriveSubscribers(int limit, TwitchConstants.Direction direction)
        //{
        //    if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
        //    {
        //        Uri uri;
        //        string url = "https://api.twitch.tv/kraken/channels/" + name + "/subscriptions";
        //        if (limit <= 100)
        //        {
        //            url += "?limit=" + limit.ToString();
        //        }
        //        else
        //        {
        //            twixel.CreateError("You cannot fetch more than 100 subs at a time");
        //            return null;
        //        }

        //        if (direction != TwitchConstants.Direction.None)
        //        {
        //            url += "&direction=" + TwitchConstants.DirectionToString(direction);
        //        }

        //        uri = new Uri(url);
        //        string responseString = await Twixel.GetWebData(uri, accessToken);
        //        if (responseString != "422")
        //        {
        //            totalSubscribers = (int)JObject.Parse(responseString)["_total"];
        //            nextSubs = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //            foreach (JObject o in (JArray)JObject.Parse(responseString)["subscriptions"])
        //            {
        //                if (!ContainsSubscriber((string)o["user"]["name"]))
        //                {
        //                    subscribedUsers.Add(LoadSubscriber(o));
        //                }
        //            }
        //            return subscribedUsers;
        //        }
        //        else
        //        {
        //            twixel.CreateError("You aren't partnered so you cannot have subs");
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        if (!authorized)
        //        {
        //            twixel.CreateError(name + " is not authorized");
        //        }
        //        else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelSubscriptions))
        //        {
        //            twixel.CreateError(name + " has not given channel_subscriptions permissions");
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Checks to see if a specified user is subscribed to this user. Requires user authorization. Requires Twitch partnership.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>A subscription object if the user is subscribed</returns>
        //public async Task<Subscription> RetrieveSubsciber(string username)
        //{
        //    if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.ChannelCheckSubscription))
        //    {
        //        Uri uri;
        //        uri = new Uri("https://api.twitch.tv/kraken/channels/" + name + "/subscriptions/" + username);
        //        string responseString = await Twixel.GetWebData(uri, accessToken);
        //        if (responseString != "422" && responseString != "404")
        //        {
        //            return LoadSubscriber(JObject.Parse(responseString));
        //        }
        //        else if (responseString == "404")
        //        {
        //            twixel.CreateError(username + " is not subscribed");
        //            return null;
        //        }
        //        else if (responseString == "422")
        //        {
        //            twixel.CreateError("You aren't partnered so you cannot have subs");
        //            return null;
        //        }
        //        else
        //        {
        //            twixel.CreateError(responseString);
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        if (!authorized)
        //        {
        //            twixel.CreateError(name + " is not authorized");
        //        }
        //        else if (!authorizedScopes.Contains(TwitchConstants.Scope.ChannelCheckSubscription))
        //        {
        //            twixel.CreateError(name + " has not given channel_check_subscription permissions");
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Checks to see if this user is subcribed to a specified channel. Requires user authorization.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>A subscription object</returns>
        //public async Task<Subscription> RetrieveSubscription(string channel)
        //{
        //    if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserSubcriptions))
        //    {
        //        Uri uri;
        //        uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/subscriptions/" + channel);
        //        string responseString = await Twixel.GetWebData(uri, accessToken);
        //        if (responseString != "422" && responseString != "404")
        //        {
        //            return LoadSubscription(JObject.Parse(responseString));
        //        }
        //        else if (responseString == "404")
        //        {
        //            twixel.CreateError("You are not subscribed to " + channel);
        //            return null;
        //        }
        //        else if (responseString == "422")
        //        {
        //            twixel.CreateError(channel + " has no subscription program");
        //            return null;
        //        }
        //        else
        //        {
        //            twixel.CreateError(responseString);
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        if (!authorized)
        //        {
        //            twixel.CreateError(name + " is not authorized");
        //        }
        //        else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserSubcriptions))
        //        {
        //            twixel.CreateError(name + " has not given user_subscriptions permissions");
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Checks to see if this user is following a specified channel
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>A channel object</returns>
        //public async Task<Channel> RetrieveFollowing(string channel)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels/" + channel);
        //    string responseString = await Twixel.GetWebData(uri);
        //    if (responseString != "404")
        //    {
        //        return twixel.LoadChannel((JObject)JObject.Parse(responseString)["channel"]);
        //    }
        //    else if (responseString == "404")
        //    {
        //        twixel.CreateError(name + " is not following " + channel);
        //        return null;
        //    }
        //    else
        //    {
        //        twixel.CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Follows a channel. Requires user authorization.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>A channel object</returns>
        //public async Task<Channel> FollowChannel(string channel)
        //{
        //    if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
        //    {
        //        Uri uri;
        //        uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels/" + channel);
        //        string responseString = await Twixel.PutWebData(uri, accessToken, "");
        //        if (responseString != "422")
        //        {
        //            Channel temp = twixel.LoadChannel((JObject)JObject.Parse(responseString)["channel"]);
        //            return temp;
        //        }
        //        else if (responseString == "422")
        //        {
        //            twixel.CreateError("Could not follow " + channel);
        //            return null;
        //        }
        //        else
        //        {
        //            twixel.CreateError(responseString);
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        if (!authorized)
        //        {
        //            twixel.CreateError(name + " is not authorized");
        //        }
        //        else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
        //        {
        //            twixel.CreateError(name + " has not given user_follows_edit permissions");
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Unfollows a channel. Requires user authorization.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>If the request succeeded</returns>
        //public async Task<bool> UnfollowChannel(string channel)
        //{
        //    if (authorized && authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
        //    {
        //        Uri uri;
        //        uri = new Uri("https://api.twitch.tv/kraken/users/" + name + "/follows/channels/" + channel);
        //        string responseString = await Twixel.DeleteWebData(uri, accessToken);
        //        if (responseString == "")
        //        {
        //            return true;
        //        }
        //        else if (responseString == "404")
        //        {
        //            twixel.CreateError(channel + "was not being followed");
        //            return false;
        //        }
        //        else if (responseString == "422")
        //        {
        //            twixel.CreateError(channel + " could not be unfollowed. Try again.");
        //            return false;
        //        }
        //        else
        //        {
        //            twixel.CreateError(responseString);
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        if (!authorized)
        //        {
        //            twixel.CreateError(name + " is not authorized");
        //        }
        //        else if (!authorizedScopes.Contains(TwitchConstants.Scope.UserFollowsEdit))
        //        {
        //            twixel.CreateError(name + " has not given user_follows_edit permissions");
        //        }
        //        return false;
        //    }
        //}

        //Subscription LoadSubscriber(JObject o)
        //{
        //    Subscription sub = new Subscription((string)o["_id"], (JObject)o["user"], (string)o["created_at"]);
        //    return sub;
        //}

        //Subscription LoadSubscription(JObject o)
        //{
        //    Subscription sub = new Subscription((string)o["_id"], (string)o["created_at"], (JObject)o["channel"]);
        //    return sub;
        //}

        Notification LoadNotifications(JObject o)
        {
            return new Notification((bool)o["email"], (bool)o["push"]);
        }
    }
}
