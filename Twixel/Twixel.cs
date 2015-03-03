using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
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
            /// Not a valid version
            /// </summary>
            None
        }

        /// <summary>
        /// Your client ID
        /// </summary>
        public static string clientID = "";

        /// <summary>
        /// Your client secret
        /// </summary>
        public static string clientSecret = "";

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
        /// The next games URL
        /// </summary>
        public Uri nextGames;

        /// <summary>
        /// How many games are live on Twitch at the moment
        /// </summary>
        public int? maxGames;

        /// <summary>
        /// The next streams URL
        /// </summary>
        public Uri nextStreams;

        /// <summary>
        /// The next teams URL
        /// </summary>
        public Uri nextTeams;

        /// <summary>
        /// The next videos URL
        /// </summary>
        public Uri nextVideos;

        /// <summary>
        /// The next follows URL
        /// </summary>
        public Uri nextFollows;

        /// <summary>
        /// Default Twixel constructor, sets API version to v3
        /// </summary>
        /// <param name="id">Your client ID</param>
        /// <param name="secret">Your client secret</param>
        /// <param name="url">Your redirect URL, should not have / at end</param>
        public Twixel(string id,
            string secret,
            string url) : this(id, secret, url, APIVersion.v3)
        {
        }

        /// <summary>
        /// Twixel constructor
        /// </summary>
        /// <param name="id">Your client ID</param>
        /// <param name="secret">Your client secret</param>
        /// <param name="url">Your redirect URL, should not have / at end</param>
        /// <param name="defaultVersion">The API version you want to use</param>
        public Twixel(string id, string secret, string url, APIVersion defaultVersion)
        {
            clientID = id;
            clientSecret = secret;
            redirectUrl = url;
            this.DefaultVersion = defaultVersion;
        }

        /// <summary>
        /// Gets games by number of viewers
        /// </summary>
        /// <param name="getNext">If this method was called before then this will get the next page of games</param>
        /// <returns>Returns a list of games (default length 25).
        /// If the page of games contains no games this will return an empty list.
        /// If an error occurs this will return null.</returns>
        //public async Task<List<Game>> RetrieveTopGames(bool getNext)
        //{
        //    Uri uri;

        //    if (!getNext)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/games/top");
        //    }
        //    else
        //    {
        //        if (nextGames != null)
        //        {
        //            uri = nextGames.url;
        //        }
        //        else
        //        {
        //            uri = new Uri("https://api.twitch.tv/kraken/games/top");
        //        }
        //    }

        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadGames(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets games by number of viewers, can specify how many games to get
        /// </summary>
        /// <param name="limit">How many streams to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="hls">If set to true, only returns streams using HLS</param>
        /// <returns>Returns a list of games.
        /// If an error occurs this returns null.</returns>
        //public async Task<List<Game>> RetrieveTopGames(int limit, bool hls)
        //{
        //    Uri uri;

        //    if (limit <= 100)
        //    {
        //        if (!hls)
        //        {
        //            uri = new Uri("https://api.twitch.tv/kraken/games/top?limit=" + limit.ToString());
        //        }
        //        else
        //        {
        //            uri = new Uri("https://api.twitch.tv/kraken/games/top?limit=" + limit.ToString() + "&hls=true");
        //        }
        //    }
        //    else
        //    {
        //        CreateError("The max number of top games you can get at one time is 100");
        //        return null;
        //    }

        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadGames(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a live stream.
        /// If the stream is offline this method will throw an exception.
        /// </summary>
        /// <param name="channelName">The channel stream to get</param>
        /// <returns>Returns a stream object.
        /// If the stream is offline or an error occurs this will throw an exception.</returns>
        public async Task<Stream> RetrieveStream(string channelName, APIVersion version = APIVersion.None)
        {
            if (version == APIVersion.None)
            {
                version = defaultVersion;
            }

            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/streams/" + channelName);
            string responseString;
            try
            {
                responseString = await GetWebData(uri, version);
            }
            catch (TwitchException ex)
            {
                throw new TwixelException("There was a Twitch API error", ex);
            }

            if (GoodStatusCode(responseString))
            {
                JObject stream = JObject.Parse(responseString);
                if (stream["stream"].ToString() != "")
                {
                    return HelperMethods.LoadStream(stream, version);
                }
                else
                {
                    throw new TwixelException(channelName + " is offline",
                        (JObject)stream["_links"]);
                }
            }
            else
            {
                throw new TwixelException(responseString);
            }
        }

        /// <summary>
        /// Gets the top live streams on Twitch
        /// </summary>
        /// <param name="getNext">If this method was called before then this will get the next page of streams</param>
        /// <returns>Returns a list of streams.
        /// If the page of streams contains no streams this will return an empty list.
        /// If an error occurs this will return null.</returns>
        //public async Task<List<Stream>> RetrieveStreams(bool getNext)
        //{
        //    Uri uri;
        //    if (!getNext)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/streams");
        //    }
        //    else
        //    {
        //        if (nextStreams != null)
        //        {
        //            uri = nextStreams.url;
        //        }
        //        else
        //        {
        //            uri = new Uri("https://api.twitch.tv/kraken/streams");
        //        }
        //    }

        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadStreams(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the top live streams for the game specified
        /// </summary>
        /// <param name="game">The game you want streams for</param>
        /// <returns>Returns a list of streams.
        /// If the page of streams contains no streams this will return an empty list.
        /// If an error occurs this will return null.</returns>
        //public async Task<List<Stream>> RetrieveStreams(string game)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/streams?game=" + game);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadStreams(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the top live streams on Twitch
        /// </summary>
        /// <param name="game">The game you want streams for. Can be an empty string. May not be null.</param>
        /// <param name="channels">Streams from a list of channels. Can be an empty list. May not be null.</param>
        /// <param name="limit">How many streams to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="embeddable">If set to true, only returns streams that can be embedded</param>
        /// <param name="hls">If set to true, only returns streams using HLS</param>
        /// <returns>Returns a list of streams.
        /// If the page of streams contains no streams this will return an empty list.
        /// If an error occurs this returns null.</returns>
        //public async Task<List<Stream>> RetrieveStreams(string game, List<string> channels, int limit, bool embeddable, bool hls)
        //{
        //    Uri uri;
        //    string uriString = "https://api.twitch.tv/kraken/streams";

        //    if (game != "")
        //    {
        //        uriString = "https://api.twitch.tv/kraken/streams?game=" + game;

        //        if (channels.Count > 0)
        //        {
        //            uriString += "&channel=";
        //            for (int i = 0; i < channels.Count; i++)
        //            {
        //                if (i != channels.Count - 1)
        //                {
        //                    uriString += channels[i] + ",";
        //                }
        //                else
        //                {
        //                    uriString += channels[i];
        //                }
        //            }
        //        }

        //        if (limit <= 100)
        //        {
        //            uriString += "&limit=" + limit.ToString();
        //        }
        //        else
        //        {
        //            uriString += "&limit=100";
        //        }

        //        if (embeddable)
        //        {
        //            uriString += "&embeddable=true";
        //        }

        //        if (hls)
        //        {
        //            uriString += "&hls=true";
        //        }
        //    }
        //    else
        //    {
        //        string seperator = "?";

        //        if (channels.Count > 0)
        //        {
        //            uriString += "?channel=";
        //            seperator = "&";
        //            for (int i = 0; i < channels.Count; i++)
        //            {
        //                if (i != channels.Count - 1)
        //                {
        //                    uriString += channels[i] + ",";
        //                }
        //                else
        //                {
        //                    uriString += channels[i];
        //                }
        //            }
        //        }

        //        if (limit <= 100)
        //        {
        //            uriString += seperator + "limit=" + limit.ToString();
        //        }
        //        else
        //        {
        //            uriString += seperator + "limit=100";
        //        }
        //        seperator = "&";

        //        if (embeddable)
        //        {
        //            uriString += "&embeddable=true";
        //        }

        //        if (hls)
        //        {
        //            uriString += "&hls=true";
        //        }
        //    }

        //    uri = new Uri(uriString);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadStreams(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the featured live streams on Twitch
        /// </summary>
        /// <returns>Returns a list of featured streams.
        /// If the page of featured streams contains no streams this will return an empty list.
        /// If a error occurs this will return null.</returns>
        //public async Task<List<FeaturedStream>> RetrieveFeaturedStreams()
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/streams/featured");
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadFeaturedStreams(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the featured live streams on Twitch
        /// </summary>
        /// <param name="limit">How many featured streams to get at one time. Default is 25. Maximum is 100</param>
        /// <param name="hls">If set to true, only returns streams using HLS</param>
        /// <returns>Returns a list of featured streams.
        /// If the page of featured streams contains no streams this will return an empty list.
        /// If an error occurs this will return null.</returns>
        //public async Task<List<FeaturedStream>> RetrieveFeaturedStreams(int limit, bool hls)
        //{
        //    Uri uri;
        //    if (!hls)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/streams/featured?limit=" + limit.ToString());
        //    }
        //    else
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/streams/featured?limit=" + limit.ToString() + "&hls=true");
        //    }
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadFeaturedStreams(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a summary of live streams on Twitch
        /// </summary>
        /// <returns>A Dictionary with the first key of Viewers and the second key of Channels.
        /// If an error occurs this will return null.</returns>
        //public async Task<Dictionary<string, int>> RetrieveStreamsSummary()
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/streams/summary");
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        JObject summary = JObject.Parse(responseString);
        //        Dictionary<string, int> summaryDict = new Dictionary<string, int>();
        //        summaryDict.Add("Viewers", (int)summary["viewers"]);
        //        summaryDict.Add("Channels", (int)summary["channels"]);
        //        return summaryDict;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Creates a URL that can be used to authenticate a user
        /// </summary>
        /// <param name="scopes">The permissions you are requesting. Must contain at least one permission.</param>
        /// <returns>Returns a URL to be used for authenticating a user.
        /// If the scopes list contained no scopes this returns null.</returns>
        //public Uri Login(List<TwitchConstants.Scope> scopes)
        //{
        //    if (scopes == null)
        //    {
        //        CreateError("The list of scopes cannot be null.");
        //        return null;
        //    }

        //    if (scopes.Count > 0)
        //    {
        //        List<TwitchConstants.Scope> cleanScopes = new List<TwitchConstants.Scope>();
        //        for (int i = 0; i < scopes.Count; i++)
        //        {
        //            if (!cleanScopes.Contains(scopes[i]))
        //            {
        //                cleanScopes.Add(scopes[i]);
        //            }
        //            else
        //            {
        //                scopes.RemoveAt(i);
        //                i--;
        //            }
        //        }
        //        Uri uri;
        //        uri = new Uri("https://api.twitch.tv/kraken/oauth2/authorize" +
        //        "?response_type=token" +
        //        "&client_id=" + clientID +
        //        "&redirect_uri=" + redirectUrl +
        //        "&scope=");
        //        string originalString = uri.OriginalString;
        //        foreach (TwitchConstants.Scope scope in scopes)
        //        {
        //            originalString += TwitchConstants.ScopeToString(scope) + " ";
        //        }
        //        uri = new Uri(originalString);
        //        return uri;
        //    }
        //    else
        //    {
        //        CreateError("You must have at least 1 scope");
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a user by their name
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <returns>Returns a user.
        /// If an error occurs this returns null.</returns>
        //public async Task<User> RetrieveUser(string name)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/users/" + name);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadUser(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the chat URL's for the specified user
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <returns>Returns list of URLs.
        /// If an error occurs this returns null.</returns>
        //public async Task<List<Uri>> RetrieveChat(string user)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/chat/" + user);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<Uri> chatLinks = new List<Uri>();
        //        chatLinks.Add(new Uri((string)JObject.Parse(responseString)["_links"]["self"]));
        //        chatLinks.Add(new Uri((string)JObject.Parse(responseString)["_links"]["emoticons"]));
        //        chatLinks.Add(new Uri((string)JObject.Parse(responseString)["_links"]["badges"]));
        //        return chatLinks;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the list of emoticons on Twitch
        /// </summary>
        /// <returns>Returns a list of emoticons.
        /// If an error occurs this returns null.</returns>
        //public async Task<List<Emoticon>> RetrieveEmoticons()
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/chat/emoticons");
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadEmoticons(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a list of live streams based upon a search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>Returns a list of streams.
        /// If an errr occurs this will return null.</returns>
        //public async Task<List<Stream>> SearchStreams(string query)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/search/streams?q=" + query);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadStreams(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a list of live streams based upon a search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="limit">How many streams to get at one time. Default is 25. Maximum is 100</param>
        /// <returns>Returns list of streams.
        /// If an error occurs this will return null.</returns>
        //public async Task<List<Stream>> SearchStreams(string query, int limit)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/search/streams?q=" + query + "&limit=" + limit.ToString());
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadStreams(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a list of games based upon a search query
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="live">If true, only returns games that are live on at least one channel</param>
        /// <returns>Returns a list of searched games.
        /// If an error occurs this will return null.</returns>
        //public async Task<List<SearchedGame>> SearchGames(string query, bool live)
        //{
        //    Uri uri;
            
        //    if (live)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/search/games?q=" + query + "&type=suggest&live=true");
        //    }
        //    else
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/search/games?q=" + query + "&type=suggest&live=false");
        //    }

        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadSearchedGames(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the list of teams from Twitch
        /// </summary>
        /// <param name="getNext">If this method was called before then this will get the next page of teams</param>
        /// <returns>Returns a list of teams.
        /// If the page of teams contains no teams this will return an empty list.
        /// If an error occurs this will return null.</returns>
        //public async Task<List<Team>> RetrieveTeams(bool getNext)
        //{
        //    Uri uri;
        //    if (!getNext)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/teams");
        //    }
        //    else
        //    {
        //        if (nextTeams != null)
        //        {
        //            uri = nextTeams.url;
        //        }
        //        else
        //        {
        //            uri = new Uri("https://api.twitch.tv/kraken/teams");
        //        }
        //    }
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<Team> teams = new List<Team>();
        //        nextTeams = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //        foreach (JObject o in JObject.Parse(responseString)["teams"])
        //        {
        //            teams.Add(LoadTeam(o));
        //        }
        //        return teams;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the list of teams from Twitch
        /// </summary>
        /// <param name="limit">How many teams to get at one time. Default is 25. Maximum is 100</param>
        /// <returns>A list of teams</returns>
        //public async Task<List<Team>> RetrieveTeams(int limit)
        //{
        //    Uri uri;
        //    if (limit <= 100)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/teams?limit=" + limit.ToString());
        //    }
        //    else
        //    {
        //        CreateError("The max number of teams you can get at once is 100");
        //        return null;
        //    }
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<Team> teams = new List<Team>();
        //        nextTeams = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //        foreach (JObject o in JObject.Parse(responseString)["teams"])
        //        {
        //            teams.Add(LoadTeam(o));
        //        }
        //        return teams;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a team by name
        /// </summary>
        /// <param name="name">The name of the team</param>
        /// <returns>A team</returns>
        //public async Task<Team> RetrieveTeam(string name)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/teams/" + name);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadTeam(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a video by ID
        /// </summary>
        /// <param name="id">The video ID</param>
        /// <returns>A video</returns>
        //public async Task<Video> RetrieveVideo(string id)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/videos/" + id);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadVideo(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the top videos on Twitch
        /// </summary>
        /// <param name="getNext">If this method was called before then this will get the next page of videos</param>
        /// <returns>A list of videos</returns>
        //public async Task<List<Video>> RetrieveTopVideos(bool getNext)
        //{
        //    Uri uri;
        //    if (!getNext)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/videos/top");
        //    }
        //    else
        //    {
        //        if (nextVideos != null)
        //        {
        //            uri = nextVideos.url;
        //        }
        //        else
        //        {
        //            uri = new Uri("https://api.twitch.tv/kraken/videos/top");
        //        }
        //    }
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        nextVideos = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //        List<Video> videos = new List<Video>();
        //        foreach (JObject video in (JArray)JObject.Parse(responseString)["videos"])
        //        {
        //            videos.Add(LoadTopVideo(video));
        //        }
        //        return videos;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the top videos on Twitch
        /// </summary>
        /// <param name="limit">How many videos to get at one time. Default is 10. Maximum is 100</param>
        /// <param name="game">The name of the game to get videos for</param>
        /// <param name="period">The time period you want to look in</param>
        /// <returns>A list of videos</returns>
        //public async Task<List<Video>> RetrieveTopVideos(int limit, string game, TwitchConstants.Period period)
        //{
        //    Uri uri;
        //    string url = "https://api.twitch.tv/kraken/videos/top";
        //    if (limit <= 100)
        //    {
        //        url += "?limit=" + limit.ToString();
        //    }
        //    else
        //    {
        //        CreateError("You cannot load more than 100 videos at a time");
        //        return null;
        //    }

        //    if (game != "")
        //    {
        //        url += "&game=" + game;
        //    }

        //    if (period != TwitchConstants.Period.None)
        //    {
        //        url += "&period=" + TwitchConstants.PeriodToString(period);
        //    }

        //    uri = new Uri(url);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<Video> videos = new List<Video>();
        //        nextVideos = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //        foreach (JObject video in (JArray)JObject.Parse(responseString)["videos"])
        //        {
        //            videos.Add(LoadTopVideo(video));
        //        }
        //        return videos;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a list of videos for a specified channel
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <param name="getNext">If this method was called before then this will get the next page of videos</param>
        /// <returns></returns>
        //public async Task<List<Video>> RetrieveVideos(string channel, bool getNext)
        //{
        //    Uri uri;
        //    if (!getNext)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/channels/" + channel + "/videos");
        //    }
        //    else
        //    {
        //        if (nextVideos != null)
        //        {
        //            uri = nextVideos.url;
        //        }
        //        else
        //        {
        //            uri = new Uri("https://api.twitch.tv/kraken/channels/" + channel + "/videos");
        //        }
        //    }
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<Video> videos = new List<Video>();
        //        foreach (JObject video in (JArray)JObject.Parse(responseString)["videos"])
        //        {
        //            videos.Add(LoadVideo(video));
        //        }
        //        return videos;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a list of videos for a specified channel
        /// </summary>
        /// <param name="channel">The name of the channel</param>
        /// <param name="limit">How many videos to get at one time. Default is 10. Maximum is 100</param>
        /// <param name="broadcasts">Returns only broadcasts when true. Otherwise only highlights are returned. Default is false.</param>
        /// <returns>A list of videos</returns>
        //public async Task<List<Video>> RetrieveVideos(string channel, int limit, bool broadcasts)
        //{
        //    Uri uri;
        //    string url = "https://api.twitch.tv/kraken/channels/" + channel;
        //    if (limit <= 100)
        //    {
        //        url += "/videos?limit=" + limit.ToString();
        //    }
        //    else
        //    {
        //        CreateError("You cannot load more than 100 videos at a time");
        //        return null;
        //    }

        //    if (broadcasts)
        //    {
        //        url += "&broadcasts=true";
        //    }
        //    else
        //    {
        //        url += "&broadcasts=false";
        //    }

        //    uri = new Uri(url);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<Video> videos = new List<Video>();
        //        nextVideos = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //        foreach (JObject video in (JArray)JObject.Parse(responseString)["videos"])
        //        {
        //            videos.Add(LoadVideo(video));
        //        }
        //        return videos;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a list of users following a specified user
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <param name="getNext">If this method was called before then this will get the next page of users</param>
        /// <returns>A list of users</returns>
        //public async Task<List<User>> RetrieveFollowers(string user, bool getNext)
        //{
        //    Uri uri;
        //    if (!getNext)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/channels/" + user + "/follows");
        //    }
        //    else
        //    {
        //        if (nextFollows != null)
        //        {
        //            uri = nextFollows.url;
        //        }
        //        else
        //        {
        //            uri = uri = new Uri("https://api.twitch.tv/kraken/channels/" + user + "/follows");
        //        }
        //    }
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<User> following = new List<User>();
        //        nextFollows = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //        foreach (JObject o in (JArray)JObject.Parse(responseString)["follows"])
        //        {
        //            following.Add(LoadUser((JObject)o["user"]));
        //        }
        //        return following;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets a list of users following a specified user
        /// </summary>
        /// <param name="user">The name of the user</param>
        /// <param name="limit">How many users to get at one time. Default is 25. Maximum is 100</param>
        /// <returns>A list of users</returns>
        //public async Task<List<User>> RetrieveFollowers(string user, int limit)
        //{
        //    Uri uri;

        //    if (limit <= 100)
        //    {
        //        uri = new Uri("https://api.twitch.tv/kraken/channels/" + user + "/follows?limit=" + limit.ToString());
        //    }
        //    else
        //    {
        //        CreateError("You cannot retrieve more than 100 followers at a time");
        //        return null;
        //    }

        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<User> following = new List<User>();
        //        nextFollows = new Uri((string)JObject.Parse(responseString)["_links"]["next"]);
        //        foreach (JObject o in (JArray)JObject.Parse(responseString)["follows"])
        //        {
        //            following.Add(LoadUser((JObject)o["user"]));
        //        }
        //        return following;
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the channel of the specified user
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <returns>A channel</returns>
        //public async Task<Channel> RetrieveChannel(string name)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/channels/" + name);
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        return LoadChannel(JObject.Parse(responseString));
        //    }
        //    else
        //    {
        //        if (responseString == "404")
        //        {
        //            CreateError(name + " was not found");
        //        }
        //        else
        //        {
        //            CreateError(responseString);
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the list of RTMP ingest points
        /// </summary>
        /// <returns>A list of ingests</returns>
        //public async Task<List<Ingest>> RetrieveIngests()
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken/ingests");
        //    string responseString;
        //    responseString = await GetWebData(uri);
        //    if (GoodStatusCode(responseString))
        //    {
        //        List<Ingest> ingests = new List<Ingest>();
        //        foreach (JObject o in (JArray)JObject.Parse(responseString)["ingests"])
        //        {
        //            ingests.Add(LoadIngest(o));
        //        }

        //        return ingests;
        //    }
        //    else
        //    {
        //        if (responseString == "503")
        //        {
        //            CreateError("Error retrieving ingest status");
        //            return null;
        //        }
        //        else
        //        {
        //            CreateError(responseString);
        //            return null;
        //        }
        //    }
        //}

        //async Task<User> RetrieveAuthenticatedUser(string accessToken, List<TwitchConstants.Scope> authorizedScopes)
        //{
        //    if (authorizedScopes.Contains(TwitchConstants.Scope.UserRead))
        //    {
        //        Uri uri;
        //        uri = new Uri("https://api.twitch.tv/kraken/user");
        //        string responseString;
        //        responseString = await GetWebData(uri, accessToken);
        //        if (GoodStatusCode(responseString))
        //        {
        //            return LoadAuthUser(JObject.Parse(responseString), accessToken, authorizedScopes);
        //        }
        //        else
        //        {
        //            CreateError(responseString);
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        CreateError("This user has not given user_read permissions");
        //        return null;
        //    }
        //}

        /// <summary>
        /// Gets the status of an access token, if the token is valid this returns an
        /// authorized user object
        /// </summary>
        /// <param name="accessToken">The access token</param>
        /// <returns>An authorized user</returns>
        //public async Task<User> RetrieveUserWithAccessToken(string accessToken)
        //{
        //    Uri uri;
        //    uri = new Uri("https://api.twitch.tv/kraken");
        //    string responseString;
        //    responseString = await Twixel.GetWebData(uri, accessToken);
        //    if (GoodStatusCode(responseString))
        //    {
        //        JObject o = JObject.Parse(responseString);
        //        if ((bool)JObject.Parse(responseString)["token"]["valid"])
        //        {
        //            JArray userScopesA = (JArray)o["token"]["authorization"]["scopes"];
        //            List<TwitchConstants.Scope> userScopes = new List<TwitchConstants.Scope>();
        //            foreach (string scope in userScopesA)
        //            {
        //                userScopes.Add(TwitchConstants.StringToScope(scope));
        //            }
        //            return await RetrieveAuthenticatedUser(accessToken, userScopes);
        //        }
        //        else
        //        {
        //            CreateError(accessToken + " is not authorized");
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        CreateError(responseString);
        //        return null;
        //    }
        //}

        /// <summary>
        /// Checks to see if the status code returned was 200.
        /// </summary>
        /// <param name="response">The response</param>
        /// <returns>If the status code was ok or not</returns>
        public static bool GoodStatusCode(string response)
        {
            return response != "400" &&
                response != "401" &&
                response != "404" &&
                response != "422" &&
                response != "500" &&
                response != "503" &&
                response != "Unknown status code";
        }

        List<Game> LoadGames(JObject o)
        {
            List<Game> games = new List<Game>();
            nextGames = new Uri((string)o["_links"]["next"]);
            maxGames = (int?)o["_total"];

            foreach (JObject obj in (JArray)o["top"])
            {
                games.Add(new Game((string)obj["game"]["name"],
                    (JObject)obj["game"]["box"],
                    (JObject)obj["game"]["logo"],
                    (long?)obj["game"]["_id"],
                    (long?)obj["game"]["giantbomb_id"],
                    (int?)obj["viewers"],
                    (int?)obj["channels"]));
            }

            return games;
        }

        List<SearchedGame> LoadSearchedGames(JObject o)
        {
            List<SearchedGame> games = new List<SearchedGame>();
            foreach (JObject obj in (JArray)o["games"])
            {
                games.Add(new SearchedGame((string)obj["name"],
                    (JObject)obj["box"],
                    (JObject)obj["logo"],
                    (long?)obj["_id"],
                    (long?)obj["giantbomb_id"],
                    (int?)obj["viewers"],
                    (int?)obj["channels"],
                    (JObject)obj["images"],
                    (int?)obj["popularity"]));
            }
            return games;
        }

        List<FeaturedStream> LoadFeaturedStreams(JObject o)
        {
            List<FeaturedStream> streams = new List<FeaturedStream>();
            nextStreams = new Uri((string)o["_links"]["next"]);

            foreach (JObject obj in (JArray)o["featured"])
            {
                streams.Add(new FeaturedStream((string)obj["stream"]["channel"]["_links"]["self"], (string)obj["image"], (string)obj["text"], (JObject)obj["stream"], this));
            }

            return streams;
        }

        List<Emoticon> LoadEmoticons(JObject o)
        {
            List<Emoticon> emoticons = new List<Emoticon>();
            foreach (JObject obj in (JArray)o["emoticons"])
            {
                emoticons.Add(new Emoticon((string)obj["regex"],
                    (JArray)obj["images"]));
            }

            return emoticons;
        }

        User LoadAuthUser(JObject o, string accessToken, List<TwitchConstants.Scope> authorizedScopes)
        {
            User user = new User(this, accessToken, authorizedScopes,
                (string)o["name"],
                (string)o["logo"],
                (long)o["_id"],
                (string)o["display_name"],
                (string)o["email"],
                (bool?)o["staff"],
                (bool?)o["partnered"],
                (string)o["created_at"],
                (string)o["updated_at"],
                (string)o["bio"]);
            return user;
        }

        Video LoadVideo(JObject o)
        {
            Video video = new Video((string)o["recorded_at"],
                (string)o["title"],
                (string)o["url"],
                (string)o["_id"],
                (string)o["_links"]["channel"],
                (string)o["embed"],
                (int)o["views"],
                (string)o["description"],
                (int)o["length"],
                (string)o["game"],
                (string)o["preview"]);
            return video;
        }

        Video LoadTopVideo(JObject o)
        {
            Video video = new Video((string)o["recorded_at"],
                (string)o["title"],
                (string)o["url"],
                (string)o["_id"],
                (string)o["_links"]["channel"],
                (int)o["views"],
                (string)o["description"],
                (int)o["length"],
                (string)o["game"],
                (string)o["preview"],
                (string)o["channel"]["name"]);
            return video;
        }

        Ingest LoadIngest(JObject o)
        {
            Ingest ingest = new Ingest((string)o["name"],
                (bool)o["default"],
                (long)o["_id"],
                (string)o["url_template"],
                (double)o["availability"]);
            return ingest;
        }

        public static async Task<string> GetWebData(Uri uri, APIVersion version = APIVersion.None)
        {
            HttpClient client = new HttpClient();
            if (version == APIVersion.None)
            {
                version = APIVersion.v3;
            }

            if (version == APIVersion.v2)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
            else if (version == APIVersion.v3)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v3+json");

            client.DefaultRequestHeaders.Add("Client-ID", clientID);
            HttpResponseMessage response = await client.GetAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // 200 - OK
                return responseString;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 400 - Bad request
                return "400";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // 401 - Unauthoriezed
                return "401";
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new TwitchException(JObject.Parse(responseString));
            }
            else if ((int)response.StatusCode == 422)
            {
                throw new TwitchException(JObject.Parse(responseString));
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 500 - Internal server error
                return "500";
            }
            else if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                return "503";
            }
            else
            {
                Debug.WriteLine((int)response.StatusCode);
                return "Unknown status code";
            }
        }

        public static async Task<string> GetWebData(Uri uri, string accessToken, APIVersion version = APIVersion.v2)
        {
            HttpClient client = new HttpClient();

            if (version == APIVersion.v2)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
            else if (version == APIVersion.v3)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v3+json");

            client.DefaultRequestHeaders.Add("Client-ID", clientID);
            client.DefaultRequestHeaders.Add("Authorization", "OAuth " + accessToken);
            HttpResponseMessage response = await client.GetAsync(uri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // 200 - OK
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // 400 - Bad request
                return "400";
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // 401 - Unauthoriezed
                return "401";
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // 404 - Summoner not found
                return "404";
            }
            else if ((int)response.StatusCode == 422)
            {
                return "422";
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                // 500 - Internal server error
                return "500";
            }
            else
            {
                return "Unknown status code";
            }
        }

        public static async Task<string> PutWebData(Uri uri, string accessToken, string content, APIVersion version = APIVersion.v2)
        {
            HttpClient client = new HttpClient();

            if (version == APIVersion.v2)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
            else if (version == APIVersion.v3)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v3+json");

            client.DefaultRequestHeaders.Add("Client-ID", clientID);
            client.DefaultRequestHeaders.Add("Authorization", "OAuth " + accessToken);
            StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(uri, stringContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            else
            {
                return "Unknown status code";
            }
        }

        public static async Task<string> PostWebData(Uri uri, string accessToken, string content, APIVersion version = APIVersion.v2)
        {
            HttpClient client = new HttpClient();

            if (version == APIVersion.v2)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
            else if (version == APIVersion.v3)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v3+json");

            client.DefaultRequestHeaders.Add("Client-ID", clientID);
            client.DefaultRequestHeaders.Add("Authorization", "OAuth " + accessToken);
            StringContent stringContent = new StringContent(content, Encoding.UTF8);
            HttpResponseMessage response = await client.PostAsync(uri, stringContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            else if ((int)response.StatusCode == 422)
            {
                return "422";
            }
            else
            {
                return "Unknown status code";
            }
        }

        public static async Task<string> DeleteWebData(Uri uri, string accessToken, APIVersion version = APIVersion.v2)
        {
            HttpClient client = new HttpClient();

            if (version == APIVersion.v2)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
            else if (version == APIVersion.v3)
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v3+json");

            client.DefaultRequestHeaders.Add("Client-ID", clientID);
            client.DefaultRequestHeaders.Add("Authorization", "OAuth " + accessToken);
            HttpResponseMessage response = await client.DeleteAsync(uri);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
            else if (response.StatusCode == HttpStatusCode.NoContent)
            {
                string responseString = "";
                return responseString;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return "404";
            }
            else if ((int)response.StatusCode == 422)
            {
                return "422";
            }
            else
            {
                return "Unknown status code";
            }
        }
    }
}
