using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using golf1052.Trexler;
using Newtonsoft.Json.Linq;
using TwixelAPI.Constants;
using System.Net.Http;
using Newtonsoft.Json;

namespace TwixelAPI
{
    /// <summary>
    /// Notification struct
    /// </summary>
    public struct Notification
    {
        /// <summary>
        /// Email status
        /// </summary>
        public readonly bool email;

        /// <summary>
        /// Push status
        /// </summary>
        public readonly bool push;

        /// <summary>
        /// Notification constructor
        /// </summary>
        /// <param name="email">Email status</param>
        /// <param name="push">Push status</param>
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
        /// Name
        /// v2/v3
        /// </summary>
        public string name;

        /// <summary>
        /// Link to logo
        /// v2/v3
        /// </summary>
        public Uri logo;

        /// <summary>
        /// ID
        /// v2/v3
        /// </summary>
        public long id;

        /// <summary>
        /// Display name
        /// v2/v3
        /// </summary>
        public string displayName;

        /// <summary>
        /// Email address.
        /// Requires authentication.
        /// Requires user_read.
        /// v2/v3
        /// </summary>
        public string email;

        /// <summary>
        /// If the user is a staff member of Twitch
        /// v2
        /// </summary>
        public bool staff;

        /// <summary>
        /// Status on Twitch
        /// v3
        /// </summary>
        public string type;

        /// <summary>
        /// Partnership status.
        /// Requires authentication.
        /// Requires user_read.
        /// v2/v3
        /// </summary>
        public bool partnered;

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
        /// Bio
        /// v3
        /// </summary>
        public string bio;

        /// <summary>
        /// Notification settings
        /// v2/v3
        /// </summary>
        public Notification notifications;

        /// <summary>
        /// Cached channel object.
        /// Null until a retrieve or update is done on the user's channel.
        /// </summary>
        public Channel channel;

        /// <summary>
        /// Stream key
        /// Null until a retrieve or update is done on the user's channel.
        /// </summary>
        public string streamKey;

        /// <summary>
        /// User constructor. Unauthorized. Twitch API v3.
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
        /// User constructor. Authorized. Twitch API v3.
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
        /// Gets a list of users that the user has blocked.
        /// Requires authorization.
        /// Requires user_blocks_read.
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">Maximum number of objects in array. Default is 25. Maximum is 100.</param>
        /// <returns>A list of blocked users</returns>
        public async Task<List<Block>> RetrieveBlockedUsers(int offset = 0, int limit = 25)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserBlocksRead;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name, "blocks");
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
                return HelperMethods.LoadBlocks(JObject.Parse(responseString), version);
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
        /// Blocks a user.
        /// Requires authorization.
        /// Requires user_blocks_edit.
        /// </summary>
        /// <param name="username">The name of the user to block</param>
        /// <returns>The blocked user</returns>
        public async Task<Block> BlockUser(string username)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserBlocksEdit;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name, "blocks", username);
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
                return HelperMethods.LoadBlock(JObject.Parse(responseString), version);
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
        /// Unblocks a user.
        /// Requires authorization.
        /// Requires user_blocks_edit.
        /// </summary>
        /// <param name="username">The name of the user to block</param>
        /// <returns>
        /// Returns true if the request succeeded.
        /// Throws an exception if the user was never blocked.
        /// Throws an exception if the user could not be unblocked.
        /// </returns>
        public async Task<bool> UnblockUser(string username)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserBlocksEdit;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name, "blocks", username);
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
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Gets the channel object for this user. Also retrieves their stream key.
        /// Updates channel object.
        /// Requires authorization.
        /// Requires channel_read.
        /// </summary>
        /// <returns>A channel</returns>
        public async Task<Channel> RetrieveChannel()
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelRead;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegment("channel");
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
        /// Gets the list of users that can edit this user's channel.
        /// Requires authorization.
        /// Requires channel_read.
        /// </summary>
        /// <returns>A list of users</returns>
        public async Task<List<User>> RetrieveChannelEditors()
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelRead;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "editors");
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
                return HelperMethods.LoadUsers(JObject.Parse(responseString), version);
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
        /// Updates a channel's status.
        /// Updates channel object.
        /// Requires authorization.
        /// Requires Twitch partnership if setting a delay above 0.
        /// Requires channel_editor.
        /// </summary>
        /// <param name="status">The new status</param>
        /// <param name="game">The new game</param>
        /// <param name="delay">Delay, requires Twitch partnership if above 0</param>
        /// <param name="channelFeedEnabled">If true, the channel's feed is turned on. Default is false.</param>
        /// <returns>
        /// Returns the channel if the request succeeded.
        /// Throws an exception if the user is not allowed to use delay.
        /// </returns>
        public async Task<Channel> UpdateChannel(string status = "", string game = "",
            int delay = 0, bool channelFeedEnabled = false)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelEditor;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name);
                Uri uri = new Uri(url.ToString());
                JObject content = new JObject();
                content["channel"] = new JObject();
                content["channel"]["status"] = status;
                content["channel"]["game"] = game;
                content["channel"]["delay"] = delay;
                content["channel"]["channel_feed_enabled"] = channelFeedEnabled;
                string responseString;
                try
                {
                    responseString = await Twixel.PutWebData(uri, accessToken, content.ToString(), version);
                }
                catch (TwitchException ex)
                {
                    if (ex.Status == 422)
                    {
                        throw new TwixelException(name + " is not allowed to use delay.", ex);
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
        /// Resets this user's stream key.
        /// Updates channel object.
        /// Requires authorization.
        /// Requires channel_stream.
        /// </summary>
        /// <returns>The new stream key</returns>
        public async Task<string> ResetStreamKey()
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelStream;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "stream_key");
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
                JObject responseObject = JObject.Parse(responseString);
                channel = HelperMethods.LoadChannel(responseObject, version);
                streamKey = (string)responseObject["stream_key"];
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
        /// Requires authorization.
        /// Requires Twitch partnership.
        /// Requires channel_commercial.
        /// </summary>
        /// <param name="length">The length of the commercial</param>
        /// <returns>
        /// Returns true if the request succeeded.
        /// Throws an exception if the user is not partnered.
        /// </returns>
        public async Task<bool> StartCommercial(TwitchConstants.CommercialLength length)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelCommercial;
            if (authorized && authorizedScopes.Contains(relevantScope) && partnered)
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "commercial");
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
        /// Gets a Total object containing a list of Follow objects of type User following the specified user
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many users to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="direction">Creation date sorting direction. Default is Descending.</param>
        /// <returns>A Total object containing a list of Follow objects of type User</returns>
        public async Task<Total<List<Follow<User>>>> RetrieveFollowers(int offset = 0, int limit = 25,
            TwitchConstants.Direction direction = TwitchConstants.Direction.Decending)
        {
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "follows");
            if (limit <= 100)
            {
                url.SetQueryParam("limit", limit);
            }
            else
            {
                url.SetQueryParam("limit", 100);
            }
            url.SetQueryParams(new Dictionary<string, object>()
            {
                { "offset", offset },
                { "direction", TwitchConstants.DirectionToString(direction) }
            });
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
            List<Follow<User>> follows = HelperMethods.LoadUserFollows(responseObject, version);
            return HelperMethods.LoadTotal(responseObject, follows, version);
        }

        /// <summary>
        /// Gets a Total object containing a list of Follow objects of type Channel this user is following
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many channels to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="direction">Sorting direction. Default is Decending.</param>
        /// <param name="sortBy">Sort by. Default is CreatedAt.</param>
        /// <returns>A Total object containing a list of Follow objects of type Channel</returns>
        public async Task<Total<List<Follow<Channel>>>> RetrieveFollowing(int offset = 0, int limit = 25,
            TwitchConstants.Direction direction = TwitchConstants.Direction.Decending,
            TwitchConstants.SortBy sortBy = TwitchConstants.SortBy.CreatedAt)
        {
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name, "follows", "channels");
            if (limit <= 100)
            {
                url.SetQueryParam("limit", limit);
            }
            else
            {
                url.SetQueryParam("limit", 100);
            }
            url.SetQueryParams(new Dictionary<string, object>()
            {
                { "offset", offset },
                { "direction", TwitchConstants.DirectionToString(direction) },
                { "sortby", TwitchConstants.SortByToString(sortBy) }
            });
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
        /// Checks to see if this user is following a specified channel
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>
        /// A Follow object of type Channel if the request succeeded.
        /// Throws an exception if the user is not following the specified channel.
        /// </returns>
        public async Task<Follow<Channel>> RetrieveFollowing(string channel)
        {
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name, "follows", "channels", channel);
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await Twixel.GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                if (ex.Status == 404)
                {
                    throw new TwixelException(name + " is not following " + channel, ex);
                }
                else
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
            }
            return HelperMethods.LoadChannelFollow(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Follows a channel.
        /// Requires authorization.
        /// Requires user_follows_edit.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <param name="notifications">
        /// Whether :user should receive email/push notifications
        /// (depending on their notification settings) when the specified channel goes live.
        /// Default is false.
        /// </param>
        /// <returns>
        /// A Follow object of type Channel if the request succeeds.
        /// Throws an exception if the request was not processed.
        /// </returns>
        public async Task<Follow<Channel>> FollowChannel(string channel, bool notifications = false)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserFollowsEdit;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name,
                    "follows", "channels", channel).SetQueryParam("notifications", notifications);
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.PutWebData(uri, accessToken, "", version);
                }
                catch (TwitchException ex)
                {
                    if (ex.Status == 422)
                    {
                        throw new TwixelException("Could not follow " + channel);
                    }
                    else
                    {
                        throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                    }
                }
                return HelperMethods.LoadChannelFollow(JObject.Parse(responseString), version);
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
        /// Unfollows a channel.
        /// Requires authorization.
        /// Requires user_follows_edit.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>
        /// Returns true the request succeeded.
        /// Throws an exception if the channel was not being followed.
        /// Throws an exception if the channel could not be unfollowed.
        /// </returns>
        public async Task<bool> UnfollowChannel(string channel)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserFollowsEdit;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name, "follows", "channels", channel);
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
                        throw new TwixelException(channel + " was not being followed.", ex);
                    }
                    else if (ex.Status == 422)
                    {
                        throw new TwixelException(channel + " could not be unfollowed. Try again.", ex);
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
                else
                {
                    throw new TwixelException(TwitchConstants.unknownErrorString);
                }
            }
        }

        /// <summary>
        /// Get a Total object containing a list of Subscription of type User
        /// Requires authorization.
        /// Requires Twitch partnership.
        /// Requires channel_subscriptions.
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many subscriptions to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="direction">Creation date sorting direction. Default is Ascending.</param>
        /// <returns>A Total object containing a list of Subscription objects of type User</returns>
        public async Task<Total<List<Subscription<User>>>> RetriveSubscribers(int offset = 0, int limit = 25,
            TwitchConstants.Direction direction = TwitchConstants.Direction.Ascending)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelSubscriptions;
            if (authorized && authorizedScopes.Contains(relevantScope) && partnered)
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "subscriptions");
                if (limit <= 100)
                {
                    url.SetQueryParam("limit", limit);
                }
                else
                {
                    url.SetQueryParam("limit", 100);
                }
                url.SetQueryParams(new Dictionary<string, object>()
                {
                    { "offset", offset },
                    { "direction", TwitchConstants.DirectionToString(direction) }
                });
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
                List<Subscription<User>> subs = HelperMethods.LoadUserSubscriptions(JObject.Parse(responseString), version);
                return HelperMethods.LoadTotal(responseObject, subs, version);
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
        /// Checks to see if a specified user is subscribed to this user.
        /// Requires authorization.
        /// Requires Twitch partnership.
        /// Requires channel_check_subscription.
        /// </summary>
        /// <param name="username">The name of the user</param>
        /// <returns>A Subscription object of type User if the user is subscribed</returns>
        public async Task<Subscription<User>> RetrieveSubsciber(string username)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelCheckSubscription;
            if (authorized && authorizedScopes.Contains(relevantScope) && partnered)
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "subcriptions", username);
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.GetWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    if (ex.Status == 404)
                    {
                        throw new TwixelException(username + " is not subscribed.", ex);
                    }
                    else
                    {
                        throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                    }
                }
                return HelperMethods.LoadUserSubscription(JObject.Parse(responseString), version);
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
        /// Checks to see if this user is subcribed to a specified channel.
        /// Requires authorization.
        /// Requires user_subscriptions.
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <returns>
        /// A subscription object of type Channel if the request succeeds.
        /// Throws an exception if the user is not subscribed to the channel.
        /// Throws an exception if the channel is not partnered.</returns>
        public async Task<Subscription<Channel>> RetrieveSubscription(string channel)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserSubcriptions;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name, "subscriptions", channel);
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await Twixel.GetWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    if (ex.Status == 404)
                    {
                        throw new TwixelException("You are not subscribed to " + channel, ex);
                    }
                    else if (ex.Status == 422)
                    {
                        throw new TwixelException(channel + " has no subscription program", ex);
                    }
                    else
                    {
                        throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                    }
                }
                return HelperMethods.LoadChannelSubscription(JObject.Parse(responseString), version);
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
        /// Gets a list of live streams that the user is following.
        /// Requires authorization.
        /// Requires user_read.
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many streams to get at once. Default is 25. Maximum is 100.</param>
        /// <returns>A list of streams</returns>
        public async Task<List<Stream>> RetrieveOnlineFollowedStreams(int offset = 0, int limit = 25)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserRead;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("streams", "followed");
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
                return HelperMethods.LoadStreams(JObject.Parse(responseString), version);
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

        public async Task<List<Video>> RetrieveFollowedVideos(int offset = 0, int limit = 10, params TwitchConstants.BroadcastType[] broadcastType)
        {
            return await RetrieveFollowedVideos(offset, limit, new List<TwitchConstants.BroadcastType>(broadcastType));
        }

        /// <summary>
        /// Get a list of videos from the channels this user follows
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many videos to get at once. Default is 25. Maximum is 100.</param>
        /// <param name="broadcastType">Constrains the type of videos returned. Default is highlight. v5 only.</param>
        /// <returns>A list of videos.</returns>
        public async Task<List<Video>> RetrieveFollowedVideos(int offset = 0, int limit = 10, List<TwitchConstants.BroadcastType> broadcastType = null)
        {
            if (version == Twixel.APIVersion.v3 ||
                version == Twixel.APIVersion.v5)
            {
                TwitchConstants.Scope relevantScope = TwitchConstants.Scope.UserRead;
                if (authorized && authorizedScopes.Contains(relevantScope))
                {
                    string broadcastTypesString = string.Empty;
                    if (broadcastType == null || broadcastType.Count == 0)
                    {
                        broadcastTypesString = "highlight";
                    }
                    else
                    {
                        for (int i = 0; i < broadcastType.Count; i++)
                        {
                            if (i != broadcastType.Count - 1)
                            {
                                broadcastTypesString += $"{TwitchConstants.BroadcastTypeToString(broadcastType[i])},";
                            }
                            else
                            {
                                broadcastTypesString += TwitchConstants.BroadcastTypeToString(broadcastType[i]);
                            }
                        }
                    }
                    TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("videos", "followed")
                        .SetQueryParam("broadcast_type", broadcastTypesString);
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
                    return HelperMethods.LoadVideos(JObject.Parse(responseString), version);
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
            else
            {
                throw new TwixelException(TwitchConstants.v2UnsupportedErrorString);
            }
        }

        public async Task<Community> RetrieveCommunity()
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelEditor;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "community");
                Uri uri = new Uri(url);
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
                return Community.LoadCommunity(responseObject, version);
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

        public async Task UpdateCommunity(string communityId)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelEditor;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "community", communityId);
                Uri uri = new Uri(url);
                try
                {
                    await Twixel.PutWebData(uri, accessToken, null, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
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

        public async Task DeleteCommunity()
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelEditor;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name, "community");
                Uri uri = new Uri(url);
                try
                {
                    await Twixel.DeleteWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
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

        public async Task CreateVideo(string videoTitle, System.IO.Stream videoData,
            string description = null,
            string game = null,
            string language = null,
            string tagList = null,
            TwitchConstants.Viewable viewable = TwitchConstants.Viewable.Public,
            DateTimeOffset? viewableAt = null)
        {
            TwitchConstants.Scope relevantScope = TwitchConstants.Scope.ChannelEditor;
            if (authorized && authorizedScopes.Contains(relevantScope))
            {
                
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegment("videos").SetQueryParams(new Dictionary<string, object>
                {
                    { "channel_id", id },
                    { "title", videoTitle },
                    { "description", description },
                    { "game", game },
                    { "language", language },
                    { "tag_list", tagList },
                    //{ "viewable", TwitchConstants.ViewableToString(viewable) }
                });
                if (viewable == TwitchConstants.Viewable.Private && viewableAt != null)
                {
                    url.SetQueryParam("viewable_at", viewableAt.Value.ToString());
                }
                Uri uri = new Uri(url);
                string responseString;
                try
                {
                    responseString = await Twixel.PostWebData(uri, accessToken, null, Twixel.APIVersion.v5);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                JObject responseObject = JObject.Parse(responseString);
                CreateVideoResponse createVideoResponse = JsonConvert.DeserializeObject<CreateVideoResponse>(responseObject["upload"].ToString());
                try
                {
                    await UploadVideo(videoData, createVideoResponse);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                catch (TwixelException)
                {
                    throw;
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

        private async Task UploadVideo(System.IO.Stream videoData, CreateVideoResponse createVideoResponse)
        {
            const int maxPartSize = 26214400; // 25 MB
            const long maxVideoSize = 10737418240; // 10 GB
            
            if (videoData.Length > maxVideoSize)
            {
                throw new TwixelException("Video data is greater than 10 GB.");
            }

            HttpClient uploadClient = new HttpClient();
            TrexUri url = new TrexUri(createVideoResponse.Url);

            long partNumber = 1;
            for (long i = 0; i < videoData.Length; i += maxPartSize)
            {
                long contentLength = Math.Min(videoData.Length - i, maxPartSize);
                byte[] contentArray = new byte[contentLength];
                // offset is always 0 because Stream.Position has been updated
                // we're doing the offset from Stream.Position not the beginning :|
                videoData.Read(contentArray, 0, (int)contentLength);
                ByteArrayContent content = new ByteArrayContent(contentArray);
                url.SetQueryParams(new Dictionary<string, object>
                {
                    { "part", partNumber },
                    { "upload_token", createVideoResponse.Token }
                });
                //uploadClient.DefaultRequestHeaders.Add("Content-Length", contentLength.ToString());
                HttpResponseMessage response = await uploadClient.PutAsync(url, content);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    throw new TwitchException(responseString, response.ReasonPhrase, (int)response.StatusCode);
                }
                partNumber++;
            }

            try
            {
                await CompleteUpload(uploadClient, createVideoResponse);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
        }

        private async Task CompleteUpload(HttpClient uploadClient, CreateVideoResponse createVideoResponse)
        {
            TrexUri url = new TrexUri(createVideoResponse.Url);
            url.AppendPathSegment("complete").SetQueryParam("upload_token", createVideoResponse.Token);
            HttpResponseMessage response = await uploadClient.PostAsync(url, new StringContent(string.Empty));
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                throw new TwitchException(responseString, response.ReasonPhrase, (int)response.StatusCode);
            }
        }

        Notification LoadNotifications(JObject o)
        {
            return new Notification((bool)o["email"], (bool)o["push"]);
        }
    }
}
