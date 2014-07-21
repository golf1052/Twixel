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
    public delegate void TwixelErrorHandler(object source, TwixelErrorEventArgs e);
    public class TwixelErrorEventArgs : EventArgs
    {
        public string ErrorString { get; set; }
    }

    public class Twixel
    {
        public static string clientID = "";
        public static string clientSecret = "";

        public event TwixelErrorHandler TwixelErrorEvent;

        string errorString = "";
        public string ErrorString
        {
            get
            {
                return errorString;
            }
        }

        /// <summary>
        /// The next games url
        /// </summary>
        public WebUrl nextGames;
        public int? maxGames;

        /// <summary>
        /// The next streams url
        /// </summary>
        /// <remarks>Used by RetrieveStreams()</remarks>
        public WebUrl nextStreams;

        public WebUrl nextTeams;

        public WebUrl nextVideos;

        public WebUrl nextFollows;

        public Twixel(string id, string secret)
        {
            clientID = id;
            clientSecret = secret;
        }

        /// <summary>
        /// Gets games by number of viewers
        /// </summary>
        /// <param name="getNext">If this method was called before then this will get the next page of games</param>
        /// <returns>A list of games (default length 25)</returns>
        public async Task<List<Game>> RetrieveTopGames(bool getNext)
        {
            Uri uri;

            if (!getNext)
            {
                uri = new Uri("https://api.twitch.tv/kraken/games/top");
            }
            else
            {
                if (nextGames != null)
                {
                    uri = nextGames.url;
                }
                else
                {
                    uri = new Uri("https://api.twitch.tv/kraken/games/top");
                }
            }

            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadGames(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        /// <summary>
        /// Gets games by number of viewers, can specify how many games to get
        /// </summary>
        /// <param name="limit">How many games to get, can get up to 100 games</param>
        /// <param name="hls">Get only HLS streams?</param>
        /// <returns>A list of games</returns>
        public async Task<List<Game>> RetrieveTopGames(int limit, bool hls)
        {
            Uri uri;

            if (limit <= 100)
            {
                if (!hls)
                {
                    uri = new Uri("https://api.twitch.tv/kraken/games/top?limit=" + limit.ToString());
                }
                else
                {
                    uri = new Uri("https://api.twitch.tv/kraken/games/top?limit=" + limit.ToString() + "&hls=true");
                }
            }
            else
            {
                CreateError("The max number of top games you can get at one time is 100");
                return null;
            }

            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadGames(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<Stream> RetrieveStream(string channelName)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/streams/" + channelName);

            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                JObject stream = JObject.Parse(responseString);
                if (stream["stream"].ToString() != "")
                {
                    return LoadStream((JObject)stream["stream"], (string)stream["_links"]["channel"]);
                }
                else
                {
                    CreateError(channelName + " is offline");
                    return null;
                }
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Stream>> RetrieveStreams(bool getNext)
        {
            Uri uri;
            if (!getNext)
            {
                uri = new Uri("https://api.twitch.tv/kraken/streams");
            }
            else
            {
                if (nextStreams != null)
                {
                    uri = nextStreams.url;
                }
                else
                {
                    uri = new Uri("https://api.twitch.tv/kraken/streams");
                }
            }

            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadStreams(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
                //return new List<Stream>();
            }
        }

        public async Task<List<Stream>> RetrieveStreams(string game)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/streams?game=" + game);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadStreams(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
                //return new List<Stream>();
            }
        }

        public async Task<List<Stream>> RetrieveStreams(string game, List<string> channels, int limit, bool embeddable, bool hls)
        {
            Uri uri;
            string uriString = "https://api.twitch.tv/kraken/streams";

            if (game != "")
            {
                uriString = "https://api.twitch.tv/kraken/streams?game=" + game;

                if (channels.Count > 0)
                {
                    uriString += "&channel=";
                    for (int i = 0; i < channels.Count; i++)
                    {
                        if (i != channels.Count - 1)
                        {
                            uriString += channels[i] + ",";
                        }
                        else
                        {
                            uriString += channels[i];
                        }
                    }
                }

                if (limit <= 100)
                {
                    uriString += "&limit=" + limit.ToString();
                }
                else
                {
                    uriString += "&limit=100";
                }

                if (embeddable)
                {
                    uriString += "&embeddable=true";
                }

                if (hls)
                {
                    uriString += "&hls=true";
                }
            }
            else
            {
                string seperator = "?";

                if (channels.Count > 0)
                {
                    uriString += "?channel=";
                    seperator = "&";
                    for (int i = 0; i < channels.Count; i++)
                    {
                        if (i != channels.Count - 1)
                        {
                            uriString += channels[i] + ",";
                        }
                        else
                        {
                            uriString += channels[i];
                        }
                    }
                }

                if (limit <= 100)
                {
                    uriString += seperator + "limit=" + limit.ToString();
                }
                else
                {
                    uriString += seperator + "limit=100";
                }
                seperator = "&";

                if (embeddable)
                {
                    uriString += "&embeddable=true";
                }

                if (hls)
                {
                    uriString += "&hls=true";
                }
            }

            uri = new Uri(uriString);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadStreams(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
                //return new List<Stream>();
            }
        }

        public async Task<List<FeaturedStream>> RetrieveFeaturedStreams()
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/streams/featured");
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadFeaturedStreams(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<FeaturedStream>> RetrieveFeaturedStreams(int limit, bool hls)
        {
            Uri uri;
            if (!hls)
            {
                uri = new Uri("https://api.twitch.tv/kraken/streams/featured?limit=" + limit.ToString());
            }
            else
            {
                uri = new Uri("https://api.twitch.tv/kraken/streams/featured?limit=" + limit.ToString() + "&hls=true");
            }
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadFeaturedStreams(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<Dictionary<string, int>> RetrieveStreamsSummary()
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/streams/summary");
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                JObject summary = JObject.Parse(responseString);
                Dictionary<string, int> summaryDict = new Dictionary<string, int>();
                summaryDict.Add("Viewers", (int)summary["viewers"]);
                summaryDict.Add("Channels", (int)summary["channels"]);
                return summaryDict;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public Uri Login(List<TwitchConstants.Scope> scopes)
        {
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
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/oauth2/authorize" +
                "?response_type=token" +
                "&client_id=" + clientID +
                "&redirect_uri=http://golf1052.com" +
                "&scope=");
                string originalString = uri.OriginalString;
                foreach (TwitchConstants.Scope scope in scopes)
                {
                    originalString += TwitchConstants.ScopeToString(scope) + " ";
                }
                uri = new Uri(originalString);
                return uri;
            }
            else
            {
                CreateError("You must have at least 1 scope");
                return null;
            }
        }

        public async Task<User> CreateUser(string name)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/users/" + name);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadUser(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<User> CreateUser(string accessToken, List<TwitchConstants.Scope> authorizedScopes)
        {
            if (authorizedScopes.Contains(TwitchConstants.Scope.UserRead))
            {
                Uri uri;
                uri = new Uri("https://api.twitch.tv/kraken/user");
                string responseString;
                responseString = await GetWebData(uri, accessToken);
                if (GoodStatusCode(responseString))
                {
                    return LoadAuthUser(JObject.Parse(responseString), accessToken, authorizedScopes);
                }
                else
                {
                    CreateError(responseString);
                    return null;
                }
            }
            else
            {
                CreateError("This user has not given user_read permissions");
                return null;
            }
        }

        public async Task<List<WebUrl>> RetrieveChat(string user)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/chat/" + user);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<WebUrl> chatLinks = new List<WebUrl>();
                chatLinks.Add(new WebUrl((string)JObject.Parse(responseString)["_links"]["self"]));
                chatLinks.Add(new WebUrl((string)JObject.Parse(responseString)["_links"]["emoticons"]));
                chatLinks.Add(new WebUrl((string)JObject.Parse(responseString)["_links"]["badges"]));
                return chatLinks;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Emoticon>> RetrieveEmoticons()
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/chat/emoticons");
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadEmoticons(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Stream>> SearchStreams(string query)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/search/streams?q=" + query);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadStreams(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Stream>> SearchStreams(string query, int limit)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/search/streams?q=" + query + "&limit=" + limit.ToString());
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadStreams(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<SearchedGame>> SearchGames(string query, bool live)
        {
            Uri uri;
            
            if (live)
            {
                uri = new Uri("https://api.twitch.tv/kraken/search/games?q=" + query + "&type=suggest&live=true");
            }
            else
            {
                uri = new Uri("https://api.twitch.tv/kraken/search/games?q=" + query + "&type=suggest&live=false");
            }

            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadSearchedGames(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Team>> RetrieveTeams(bool getNext)
        {
            Uri uri;
            if (!getNext)
            {
                uri = new Uri("https://api.twitch.tv/kraken/teams");
            }
            else
            {
                if (nextTeams != null)
                {
                    uri = nextTeams.url;
                }
                else
                {
                    uri = new Uri("https://api.twitch.tv/kraken/teams");
                }
            }
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<Team> teams = new List<Team>();
                nextTeams = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                foreach (JObject o in JObject.Parse(responseString)["teams"])
                {
                    teams.Add(LoadTeam(o));
                }
                return teams;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Team>> RetrieveTeams(int limit)
        {
            Uri uri;
            if (limit <= 100)
            {
                uri = new Uri("https://api.twitch.tv/kraken/teams?limit=" + limit.ToString());
            }
            else
            {
                uri = new Uri("https://api.twitch.tv/kraken/teams?limit=100");
                errorString = "The max number of teams you can get at once is 100";
                TwixelErrorEventArgs error = new TwixelErrorEventArgs();
                error.ErrorString = errorString;
                TwixelErrorEvent(this, error);
            }
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<Team> teams = new List<Team>();
                nextTeams = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                foreach (JObject o in JObject.Parse(responseString)["teams"])
                {
                    teams.Add(LoadTeam(o));
                }
                return teams;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<Team> RetrieveTeam(string name)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/teams/" + name);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadTeam(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<Video> RetrieveVideo(string id)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/videos/" + id);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadVideo(JObject.Parse(responseString));
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Video>> RetrieveTopVideos(bool getNext)
        {
            Uri uri;
            if (!getNext)
            {
                uri = new Uri("https://api.twitch.tv/kraken/videos/top");
            }
            else
            {
                if (nextVideos != null)
                {
                    uri = nextVideos.url;
                }
                else
                {
                    uri = new Uri("https://api.twitch.tv/kraken/videos/top");
                }
            }
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                nextVideos = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                List<Video> videos = new List<Video>();
                foreach (JObject video in (JArray)JObject.Parse(responseString)["videos"])
                {
                    videos.Add(LoadTopVideo(video));
                }
                return videos;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Video>> RetrieveTopVideos(int limit, string game, TwitchConstants.Period period)
        {
            Uri uri;
            string url = "https://api.twitch.tv/kraken/videos/top";
            if (limit <= 100)
            {
                url += "?limit=" + limit.ToString();
            }
            else
            {
                url += "?limit=100";
                errorString = "You cannot load more than 100 videos at a time";
                TwixelErrorEventArgs error = new TwixelErrorEventArgs();
                error.ErrorString = errorString;
                TwixelErrorEvent(this, error);
            }

            if (game != "")
            {
                url += "&game=" + game;
            }

            if (period != TwitchConstants.Period.None)
            {
                url += "&period=" + TwitchConstants.PeriodToString(period);
            }

            uri = new Uri(url);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<Video> videos = new List<Video>();
                nextVideos = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                foreach (JObject video in (JArray)JObject.Parse(responseString)["videos"])
                {
                    videos.Add(LoadTopVideo(video));
                }
                return videos;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Video>> RetrieveVideos(string channel, bool getNext)
        {
            Uri uri;
            if (!getNext)
            {
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + channel + "/videos");
            }
            else
            {
                if (nextVideos != null)
                {
                    uri = nextVideos.url;
                }
                else
                {
                    uri = new Uri("https://api.twitch.tv/kraken/channels/" + channel + "/videos");
                }
            }
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<Video> videos = new List<Video>();
                foreach (JObject video in (JArray)JObject.Parse(responseString)["videos"])
                {
                    videos.Add(LoadVideo(video));
                }
                return videos;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<Video>> RetrieveVideos(string channel, int limit, bool broadcasts)
        {
            Uri uri;
            string url = "https://api.twitch.tv/kraken/channels/" + channel;
            if (limit <= 100)
            {
                url += "/videos?limit=" + limit.ToString();
            }
            else
            {
                url += "/videos?limit=100";
                errorString = "You cannot load more than 100 videos at a time";
                TwixelErrorEventArgs error = new TwixelErrorEventArgs();
                error.ErrorString = errorString;
                TwixelErrorEvent(this, error);
            }

            if (broadcasts)
            {
                url += "&broadcasts=true";
            }
            else
            {
                url += "&broadcasts=false";
            }

            uri = new Uri(url);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<Video> videos = new List<Video>();
                nextVideos = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                foreach (JObject video in (JArray)JObject.Parse(responseString)["videos"])
                {
                    videos.Add(LoadVideo(video));
                }
                return videos;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<User>> RetrieveFollowers(string user, bool getNext)
        {
            Uri uri;
            if (!getNext)
            {
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + user + "/follows");
            }
            else
            {
                if (nextFollows != null)
                {
                    uri = nextFollows.url;
                }
                else
                {
                    uri = uri = new Uri("https://api.twitch.tv/kraken/channels/" + user + "/follows");
                }
            }
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<User> following = new List<User>();
                nextFollows = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                foreach (JObject o in (JArray)JObject.Parse(responseString)["follows"])
                {
                    following.Add(LoadUser((JObject)o["user"]));
                }
                return following;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<List<User>> RetrieveFollowers(string user, int limit)
        {
            Uri uri;

            if (limit <= 100)
            {
                uri = new Uri("https://api.twitch.tv/kraken/channels/" + user + "/follows?limit=" + limit.ToString());
            }
            else
            {
                CreateError("You cannot retrieve more than 100 followers at a time");
                return null;
            }

            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<User> following = new List<User>();
                nextFollows = new WebUrl((string)JObject.Parse(responseString)["_links"]["next"]);
                foreach (JObject o in (JArray)JObject.Parse(responseString)["follows"])
                {
                    following.Add(LoadUser((JObject)o["user"]));
                }
                return following;
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

        public async Task<Channel> RetrieveChannel(string name)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/channels/" + name);
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                return LoadChannel(JObject.Parse(responseString));
            }
            else
            {
                if (responseString == "404")
                {
                    CreateError(name + " was not found");
                }
                else
                {
                    CreateError(responseString);
                }
                return null;
            }
        }

        public async Task<List<Ingest>> RetrieveIngests()
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken/ingests");
            string responseString;
            responseString = await GetWebData(uri);
            if (GoodStatusCode(responseString))
            {
                List<Ingest> ingests = new List<Ingest>();
                foreach (JObject o in (JArray)JObject.Parse(responseString)["ingests"])
                {
                    ingests.Add(LoadIngest(o));
                }

                return ingests;
            }
            else
            {
                if (responseString == "503")
                {
                    CreateError("Error retrieving ingest status");
                    return null;
                }
                else
                {
                    CreateError(responseString);
                    return null;
                }
            }
        }

        public async Task<User> CreateUserWithAccessToken(string accessToken)
        {
            Uri uri;
            uri = new Uri("https://api.twitch.tv/kraken");
            string responseString;
            responseString = await Twixel.GetWebData(uri, accessToken);
            if (GoodStatusCode(responseString))
            {
                JObject o = JObject.Parse(responseString);
                if ((bool)JObject.Parse(responseString)["token"]["valid"])
                {
                    JArray userScopesA = (JArray)o["token"]["authorization"]["scopes"];
                    List<TwitchConstants.Scope> userScopes = new List<TwitchConstants.Scope>();
                    foreach (string scope in userScopesA)
                    {
                        userScopes.Add(TwitchConstants.StringToScope(scope));
                    }
                    return await CreateUser(accessToken, userScopes);
                }
                else
                {
                    CreateError(accessToken + " is not authorized");
                    return null;
                }
            }
            else
            {
                CreateError(responseString);
                return null;
            }
        }

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
            nextGames = new WebUrl((string)o["_links"]["next"]);
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

        internal List<Stream> LoadStreams(JObject o)
        {
            List<Stream> streams = new List<Stream>();
            nextStreams = new WebUrl((string)o["_links"]["next"]);

            foreach (JObject obj in (JArray)o["streams"])
            {
                streams.Add(LoadStream(obj, (string)o["_links"]["channel"]));
            }

            return streams;
        }

        internal Stream LoadStream(JObject o, string channel)
        {
            return new Stream(channel,
                (string)o["broadcaster"],
                    (long?)o["_id"],
                    (string)o["preview"],
                    (string)o["game"],
                    (JObject)o["channel"],
                    (string)o["name"],
                    (int?)o["viewers"],
                    this);
        }

        List<FeaturedStream> LoadFeaturedStreams(JObject o)
        {
            List<FeaturedStream> streams = new List<FeaturedStream>();
            nextStreams = new WebUrl((string)o["_links"]["next"]);

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

        internal User LoadUser(JObject o)
        {
            User user = new User((string)o["name"],
                (string)o["logo"],
                (long)o["_id"],
                (string)o["display_name"],
                (bool?)o["staff"],
                (string)o["created_at"],
                (string)o["updated_at"],
                (string)o["bio"]);
            return user;
        }

        User LoadAuthUser(JObject o, string accessToken, List<TwitchConstants.Scope> authorizedScopes)
        {
            User user = new User(accessToken, authorizedScopes,
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

        internal Team LoadTeam(JObject o)
        {
            Team team = new Team((string)o["info"],
                (string)o["background"],
                (string)o["banner"],
                (string)o["name"],
                (long)o["_id"],
                (string)o["display_name"],
                (string)o["logo"]);
            return team;
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

        internal Channel LoadChannel(JObject o)
        {
            Channel channel = new Channel((string)o["mature"],
                (string)o["background"],
                (string)o["updated_at"],
                (long)o["_id"],
                (JArray)o["teams"],
                (string)o["status"],
                (string)o["logo"],
                (string)o["url"],
                (string)o["display_name"],
                (string)o["game"],
                (string)o["banner"],
                (string)o["name"],
                (string)o["video_banner"],
                (string)o["_links"]["chat"],
                (string)o["_links"]["subscriptions"],
                (string)o["_links"]["features"],
                (string)o["_links"]["commercial"],
                (string)o["_links"]["stream_key"],
                (string)o["_links"]["editors"],
                (string)o["_links"]["videos"],
                (string)o["_links"]["self"],
                (string)o["_links"]["follows"],
                (string)o["created_at"],
                (string)o["profile_banner"],
                (string)o["primary_team_name"],
                (string)o["primary_team_display_name"],
                (long?)o["views"],
                (long?)o["followers"],
                this);
            return channel;
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

        void CreateError(string errorStr)
        {
            errorString = errorStr;
            TwixelErrorEventArgs error = new TwixelErrorEventArgs();
            error.ErrorString = errorString;
            TwixelErrorEvent(this, error);
        }

        public static async Task<string> GetWebData(Uri uri)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
            client.DefaultRequestHeaders.Add("Client-ID", clientID);
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

        public static async Task<string> GetWebData(Uri uri, string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
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

        public static async Task<string> PutWebData(Uri uri, string accessToken, string content)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
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

        public static async Task<string> PostWebData(Uri uri, string accessToken, string content)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
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

        public static async Task<string> DeleteWebData(Uri uri, string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.twitchtv.v2+json");
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
