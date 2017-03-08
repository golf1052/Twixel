using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using golf1052.Trexler;
using Newtonsoft.Json.Linq;
using TwixelAPI.Constants;

namespace TwixelAPI
{
    /// <summary>
    /// Twixel class
    /// </summary>
    public class Twixel
    {
        /// <summary>
        /// The enum of available API versions
        /// </summary>
        public enum APIVersion
        {
            /// <summary>
            /// Version 2 of the API
            /// </summary>
            v2,
            /// <summary>
            /// Version 3 of the API
            /// </summary>
            v3,
            /// <summary>
            /// Version 5 of the API
            /// </summary>
            v5,
            /// <summary>
            /// Not a valid version
            /// </summary>
            None
        }

        /// <summary>
        /// Request types
        /// </summary>
        public enum RequestType
        {
            Get,
            Put,
            Post,
            Delete
        }

        /// <summary>
        /// Your client ID
        /// </summary>
        public static string clientID = "";

        /// <summary>
        /// Your redirect URL
        /// </summary>
        public string redirectUrl = "";

        private APIVersion defaultVersion;

        /// <summary>
        /// The default version that gets sent to the API
        /// </summary>
        public APIVersion DefaultVersion
        {
            get
            {
                return defaultVersion;
            }
            set
            {
                if (value == APIVersion.None)
                {
                    defaultVersion = APIVersion.v3;
                }
                else
                {
                    defaultVersion = value;
                }
            }
        }

        /// <summary>
        /// Default Twixel constructor, sets API version to v3
        /// </summary>
        /// <param name="id">Your client ID</param>
        /// <param name="secret">Your client secret</param>
        /// <param name="url">Your redirect URL, should not have / at end</param>
        public Twixel(string id,
            string url) : this(id, url, APIVersion.v3)
        {
        }

        /// <summary>
        /// Twixel constructor
        /// </summary>
        /// <param name="id">Your client ID</param>
        /// <param name="url">Your redirect URL, should not have / at end</param>
        /// <param name="defaultVersion">The API version you want to use</param>
        public Twixel(string id, string url, APIVersion defaultVersion)
        {
            clientID = id;
            redirectUrl = url;
            this.DefaultVersion = defaultVersion;
        }

        /// <summary>
        /// Converts from a Twitch API v2/v3 username to a Twitch API v5 user ID
        /// </summary>
        /// <param name="username">The Twitch API v2/v3 username</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>The Twitch API v5 user ID of the specified username</returns>
        public async Task<long> GetUserId(string username,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            if (version == APIVersion.v5)
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegment("users")
                    .SetQueryParam("login", username);
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await GetWebData(uri, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                JObject responseObject = JObject.Parse(responseString);
                return (long)responseObject["users"][0]["_id"];
            }
            else
            {
                throw new TwixelException(TwitchConstants.v5OnlyErrorString);
            }
        }

        /// <summary>
        /// Gets the channel of the specified user.
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A channel</returns>
        public async Task<Channel> RetrieveChannel(string name,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", name);
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadChannel(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Retrieve teams the specified user is a member of.
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>List of teams</returns>
        public async Task<List<Team>> RetrieveTeams(string user,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            if (version == APIVersion.v3 || version == APIVersion.v5)
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", user, "teams");
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await GetWebData(uri, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                return HelperMethods.LoadTeams(JObject.Parse(responseString), version);
            }
            else
            {
                throw new TwixelException(TwitchConstants.v2UnsupportedErrorString);
            }
        }

        /// <summary>
        /// Gets the chat URL's for the specified user
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A dictionary of links</returns>
        public async Task<Dictionary<string, Uri>> RetrieveChat(string user,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            if (version == APIVersion.v2 || version == APIVersion.v3)
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("chat", user);
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await GetWebData(uri, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                return HelperMethods.LoadLinks((JObject)JObject.Parse(responseString)["_links"]);
            }
            else
            {
                throw new TwixelException(TwitchConstants.v5UnsupportedErrorString);
            }
        }

        /// <summary>
        /// Gets the list of emoticons on Twitch
        /// </summary>
        /// <param name="version">Twitch API version</param>
        /// <returns>Returns a list of emoticons</returns>
        public async Task<List<Emoticon>> RetrieveEmoticons(APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("chat", "emoticons");
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadEmoticons(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Gets the list of badges that can be used in the specified user's channel
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>List of badges</returns>
        public async Task<List<Badge>> RetrieveBadges(string user,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("chat", user, "badges");
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadBadges(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Gets top games on Twitch.
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many streams to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A Total object containing a list of games</returns>
        public async Task<Total<List<Game>>> RetrieveTopGames(int offset = 0, int limit = 25,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("games", "top");

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
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            JObject responseObject = JObject.Parse(responseString);
            List<Game> games = HelperMethods.LoadGames(responseObject, version);
            return HelperMethods.LoadTotal(responseObject, games, version);
        }

        /// <summary>
        /// Gets the list of RTMP ingest points
        /// </summary>
        /// <param name="version">Twitch API version</param>
        /// <returns>A list of ingests</returns>
        public async Task<List<Ingest>> RetrieveIngests(APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegment("ingests");
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadIngests(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Search channels on Twitch. Twitch API v3 only.
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many channels to get at one time. Default is 25. Maximum is 100.</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A Total object containing a list of channels matching the search query</returns>
        public async Task<Total<List<Channel>>> SearchChannels(string query,
            int offset = 0, int limit = 25,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            if (version == APIVersion.v3 || version == APIVersion.v5)
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("search", "channels").SetQueryParam("query", query);
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
                    responseString = await GetWebData(uri, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                JObject responseObject = JObject.Parse(responseString);
                List<Channel> channels = HelperMethods.LoadChannels(responseObject, version);
                return HelperMethods.LoadTotal(responseObject, channels, version);
            }
            else
            {
                throw new TwixelException(TwitchConstants.v2UnsupportedErrorString);
            }
        }

        /// <summary>
        /// Search streams on Twitch
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many streams to get at one time. Default is 25. Maximum is 100.</param>
        /// <param name="hls">If set to true, only returns streams using HLS. If set to false, only returns streams that are non-HLS.</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A Total object containing a list of streams</returns>
        public async Task<Total<List<Stream>>> SearchStreams(string query,
            int offset = 0, int limit = 25, bool? hls = null,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("search", "streams").SetQueryParam("query", query);
            if (limit <= 100)
            {
                url.SetQueryParam("limit", limit);
            }
            else
            {
                url.SetQueryParam("limit", 100);
            }
            url.SetQueryParam("offset", offset);
            if (version == APIVersion.v3 || version == APIVersion.v5)
            {
                if (hls == null)
                {
                    hls = false;
                }
                url.SetQueryParam("hls", hls);
            }
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            JObject responseObject = JObject.Parse(responseString);
            List<Stream> streams = HelperMethods.LoadStreams(responseObject, version);
            return HelperMethods.LoadTotal(responseObject, streams, version);
        }

        /// <summary>
        /// Search games on Twitch
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="live">If true, only returns games that are live on at least one channel. Default is false.</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>Returns a list of searched games</returns>
        public async Task<List<SearchedGame>> SearchGames(string query,
            bool live = false,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("search", "games").SetQueryParams(new Dictionary<string, object>()
            {
                { "query", query },
                { "type", "suggest" },
                { "live", live }
            });
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadSearchedGames(JObject.Parse(responseString), version);
        }


        /// <summary>
        /// Gets a live stream.
        /// If the stream is offline this method will throw an exception.
        /// </summary>
        /// <param name="channelName">The channel stream to get.</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>
        /// Returns a stream object.
        /// If the stream is offline or an error occurs this will throw an exception.
        /// </returns>
        public async Task<Stream> RetrieveStream(string channelName,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("streams", channelName);
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }

            JObject stream = JObject.Parse(responseString);
            if (stream["stream"].ToString() != "")
            {
                return HelperMethods.LoadStream((JObject)stream["stream"],
                    version);
            }
            else
            {
                throw new TwixelException(channelName + " is offline",
                    (JObject)stream["_links"]);
            }
        }

        /// <summary>
        /// Gets the top live streams on Twitch.
        /// </summary>
        /// <param name="game">The game you want streams for.</param>
        /// <param name="channels">Streams from a list of channels.</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many streams to get at one time. Default is 25. Maximum is 100.</param>
        /// <param name="clientId">Only show stream with this client ID. Version 3 only.</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A Total object containing a list of streams</returns>
        public async Task<Total<List<Stream>>> RetrieveStreams(string game = null,
            List<string> channels = null,
            int offset = 0, int limit = 25,
            string clientId = null,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }

            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegment("streams");
            url.SetQueryParam("game", game);
            if (channels != null && channels.Count > 0)
            {
                string channelsString = "";
                for (int i = 0; i < channels.Count; i++)
                {
                    if (i != channels.Count - 1)
                    {
                        channelsString += channels[i] + ",";
                    }
                    else
                    {
                        channelsString += channels[i];
                    }
                }
                url.SetQueryParam("channel", channelsString);
            }
            if (limit <= 100)
            {
                url.SetQueryParam("limit", limit);
            }
            else
            {
                url.SetQueryParam("limit", 100);
            }
            url.SetQueryParam("offset", offset);
            if (version == APIVersion.v3)
            {
                url.SetQueryParam("client_id", clientId);
            }

            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            JObject responseObject = JObject.Parse(responseString);
            List<Stream> streams = HelperMethods.LoadStreams(responseObject, version);
            return HelperMethods.LoadTotal(responseObject, streams, version);
        }

        /// <summary>
        /// Gets the featured live streams on Twitch
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many featured streams to get at one time. Default is 25. Maximum is 100.</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>Returns a list of featured streams</returns>
        public async Task<List<FeaturedStream>> RetrieveFeaturedStreams(int offset = 0, int limit = 25,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }

            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("streams", "featured");
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
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadFeaturedStreams(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Gets a summary of live streams on Twitch
        /// </summary>
        /// <param name="game">Only show stats for this game</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A Dictionary with the first key of Viewers and the second key of Channels.</returns>
        public async Task<Dictionary<string, int>> RetrieveStreamsSummary(string game = null,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("streams", "summary");
            if (!string.IsNullOrEmpty(game))
            {
                url.SetQueryParam("game", game);
            }
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            JObject summary = JObject.Parse(responseString);
            Dictionary<string, int> summaryDict = new Dictionary<string, int>();
            summaryDict.Add("Viewers", (int)summary["viewers"]);
            summaryDict.Add("Channels", (int)summary["channels"]);
            return summaryDict;
        }

        /// <summary>
        /// Gets the list of teams from Twitch
        /// </summary>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many teams to get at one time. Default is 25. Maximum is 100.</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A list of teams</returns>
        public async Task<List<Team>> RetrieveTeams(int offset = 0, int limit = 25,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegment("teams");
            if (limit <= 100)
            {
                url.SetQueryParam("limit", limit);
            }
            else
            {
                url.SetQueryParam("limit", 100);
            }
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadTeams(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Gets a team by name
        /// </summary>
        /// <param name="name">The name of the team</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A team</returns>
        public async Task<Team> RetrieveTeam(string name,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("teams", name);
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadTeam(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Gets a user by their name
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>Returns a user</returns>
        public async Task<User> RetrieveUser(string name,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("users", name);
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadUser(JObject.Parse(responseString), version);
        }


        private async Task<User> RetrieveAuthenticatedUser(string accessToken,
            List<TwitchConstants.Scope> authorizedScopes,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            if (authorizedScopes.Contains(TwitchConstants.Scope.UserRead))
            {
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegment("user");
                Uri uri = new Uri(url.ToString());
                string responseString;
                try
                {
                    responseString = await GetWebData(uri, accessToken, version);
                }
                catch (TwitchException ex)
                {
                    throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
                }
                return HelperMethods.LoadAuthedUser(JObject.Parse(responseString),
                    accessToken, authorizedScopes, version);
            }
            else
            {
                throw new TwixelException("This user has not given user_read permissions");
            }
        }

        /// <summary>
        /// Gets the status of an access token, if the token is valid this returns an
        /// authorized user object
        /// </summary>
        /// <param name="accessToken">The access token</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>
        /// An authorized user if the request succeeds.
        /// Throws an exception if the token is not valid.</returns>
        public async Task<User> RetrieveUserWithAccessToken(string accessToken,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl);
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
            JObject token = (JObject)responseObject["token"];
            if ((bool)token["valid"])
            {
                JArray userScopesA = (JArray)token["authorization"]["scopes"];
                List<TwitchConstants.Scope> userScopes = new List<TwitchConstants.Scope>();
                foreach (string scope in userScopesA)
                {
                    userScopes.Add(TwitchConstants.StringToScope(scope));
                }
                return await RetrieveAuthenticatedUser(accessToken, userScopes, version);
            }
            else
            {
                throw new TwixelException(accessToken + " is not a valid access token",
                    (JObject)responseObject["_links"]);
            }
        }

        /// <summary>
        /// Creates a URL that can be used to authenticate a user
        /// </summary>
        /// <param name="scopes">The permissions you are requesting. Must contain at least one permission.</param>
        /// <returns>
        /// Returns a URL to be used for authenticating a user.
        /// Throws an exception if the scopes list contained no scopes.
        /// </returns>
        public Uri Login(List<TwitchConstants.Scope> scopes)
        {
            if (scopes == null)
            {
                throw new TwixelException("The list of scopes cannot be null.");
            }

            if (scopes.Count > 0)
            {
                List<TwitchConstants.Scope> cleanScopes = new List<TwitchConstants.Scope>();
                for (int i = 0; i < scopes.Count; i++)
                {
                    if (!cleanScopes.Contains(scopes[i]))
                    {
                        cleanScopes.Add(scopes[i]);
                    }
                    else
                    {
                        scopes.RemoveAt(i);
                        i--;
                    }
                }
                TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("oauth2", "authorize").SetQueryParams(new Dictionary<string, object>
                {
                    { "response_type", "token" },
                    { "client_id", clientID },
                    { "redirect_uri", redirectUrl },
                    { "scope", TwitchConstants.ListOfScopesToStringOfScopes(scopes) }
                });
                Uri uri = new Uri(url.ToString());
                return uri;
            }
            else
            {
                throw new TwixelException("You must have at least 1 scope.");
            }
        }

        /// <summary>
        /// Gets a video by ID
        /// </summary>
        /// <param name="id">The video ID</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A video</returns>
        public async Task<Video> RetrieveVideo(string id,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("videos", id);
            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadVideo(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Gets the top videos on Twitch
        /// </summary>
        /// <param name="game">The name of the game to get videos for</param>
        /// <param name="period">The time period you want to look in</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many videos to get at one time. Default is 10. Maximum is 100</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A list of videos</returns>
        public async Task<List<Video>> RetrieveTopVideos(string game = null,
            TwitchConstants.Period period = TwitchConstants.Period.Week,
            int offset = 0, int limit = 25,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("videos", "top");
            url.SetQueryParam("game", game);
            if (limit <= 100)
            {
                url.SetQueryParam("limit", limit);
            }
            else
            {
                url.SetQueryParam("limit", 100);
            }
            url.SetQueryParam("period", TwitchConstants.PeriodToString(period));

            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            return HelperMethods.LoadVideos(JObject.Parse(responseString), version);
        }

        /// <summary>
        /// Gets a Total object containing a list of videos for a specified channel
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <param name="offset">Object offset for pagination. Default is 0.</param>
        /// <param name="limit">How many videos to get at one time. Default is 10. Maximum is 100.</param>
        /// <param name="broadcasts">Returns only broadcasts when true. Otherwise only highlights are returned. Default is false.</param>
        /// <param name="hls">Returns only HLS VoDs when true. Otherwise only non-HLS VoDs are returned. Default is false.</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A Total object containing list of videos</returns>
        public async Task<Total<List<Video>>> RetrieveVideos(string channel = null,
            int offset = 0, int limit = 25,
            bool broadcasts = false,
            bool? hls = null,
            APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = DefaultVersion;
            }
            TrexUri url = new TrexUri(TwitchConstants.baseUrl).AppendPathSegments("channels", channel, "videos");
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
                { "broadcasts", broadcasts },
                { "offset", offset }
            });
            if (version == APIVersion.v3)
            {
                if (hls == null)
                {
                    hls = false;
                }
                url.SetQueryParam("hls", hls);
            }

            Uri uri = new Uri(url.ToString());
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException(TwitchConstants.twitchAPIErrorString, ex);
            }
            JObject responseObject = JObject.Parse(responseString);
            List<Video> videos = HelperMethods.LoadVideos(responseObject, version);
            return HelperMethods.LoadTotal(responseObject, videos, version);
        }

        /// <summary>
        /// Get web data.
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A response string containing a JSON object</returns>
        public static async Task<string> GetWebData(Uri uri, APIVersion version = APIVersion.None)
        {
            return await DoWebData(uri, RequestType.Get, null, null, version);
        }

        /// <summary>
        /// Get web data with an access token.
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A response string containing a JSON object</returns>
        public static async Task<string> GetWebData(Uri uri, string accessToken, APIVersion version = APIVersion.None)
        {
            return await DoWebData(uri, RequestType.Get, accessToken, null, version);
        }

        /// <summary>
        /// Put web data
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="content">JSON object as a string</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A response string containing a JSON object</returns>
        public static async Task<string> PutWebData(Uri uri, string accessToken, string content, APIVersion version = APIVersion.None)
        {
            return await DoWebData(uri, RequestType.Put, accessToken, content, version);
        }

        /// <summary>
        /// Post web data
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="content">JSON object as a string</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A response string containing a JSON object</returns>
        public static async Task<string> PostWebData(Uri uri, string accessToken, string content, APIVersion version = APIVersion.None)
        {
            return await DoWebData(uri, RequestType.Post, accessToken, content, version);
        }

        /// <summary>
        /// Delete web data
        /// </summary>
        /// <param name="uri">URL</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="version">Twitch API version</param>
        /// <returns>A response string containing a JSON object</returns>
        public static async Task<string> DeleteWebData(Uri uri, string accessToken, APIVersion version = APIVersion.None)
        {
            return await DoWebData(uri, RequestType.Delete, accessToken, null, version);
        }

        private static async Task<string> DoWebData(Uri uri, RequestType requestType, string accessToken = null, string content = null, APIVersion version = APIVersion.None)
        {
            HttpClient client = new HttpClient();
            if (version == APIVersion.None)
            {
                version = APIVersion.v3;
            }

            if (version == APIVersion.v2)
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
            }
            else if (version == APIVersion.v3)
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v3+json");
            }
            else if (version == APIVersion.v5)
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v5+json");
            }

            client.DefaultRequestHeaders.Add("Client-ID", clientID);

            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", "OAuth " + accessToken);
            }

            StringContent stringContent = new StringContent("", Encoding.UTF8, "application/json");
            if (requestType == RequestType.Put || requestType == RequestType.Post)
            {
                stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            }

            HttpResponseMessage response = null;
            if (requestType == RequestType.Get)
            {
                response = await client.GetAsync(uri);
            }
            else if (requestType == RequestType.Put)
            {
                response = await client.PutAsync(uri, stringContent);
            }
            else if (requestType == RequestType.Post)
            {
                response = await client.PostAsync(uri, stringContent);
            }
            else if (requestType == RequestType.Delete)
            {
                response = await client.DeleteAsync(uri);
            }

            string responseString;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                responseString = await response.Content.ReadAsStringAsync();
                if (responseString.StartsWith("<"))
                {
                    string[] initialErrorSplit = responseString.Split(' ');
                    string potentialErrorCode = initialErrorSplit[0].Substring(initialErrorSplit[0].Length - 3, 3);
                    int errorCodeNum = -1;
                    try
                    {
                        errorCodeNum = int.Parse(potentialErrorCode);
                    }
                    catch
                    {
                    }
                    throw new TwitchException(responseString, responseString, errorCodeNum);
                }
                return responseString;
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                responseString = string.Empty;
                return responseString;
            }
            else
            {
                responseString = await response.Content.ReadAsStringAsync();
                JObject twitchErrorObject = null;
                try
                {
                    twitchErrorObject = JObject.Parse(responseString);
                    throw new TwitchException(twitchErrorObject);
                }
                catch
                {
                    throw new TwitchException(responseString, response.ReasonPhrase, (int)response.StatusCode);
                }
            }
        }
    }
}
