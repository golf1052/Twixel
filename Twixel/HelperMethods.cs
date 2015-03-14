using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public static class HelperMethods
    {
        private const string versionCannotBeNoneString = "Version cannot be none.";

        internal static Dictionary<string, Uri> LoadLinks(JObject o)
        {
            if (o != null)
            {
                return JsonConvert.DeserializeObject<Dictionary<string, Uri>>(o.ToString());
            }
            else
            {
                return new Dictionary<string, Uri>();
            }
        }

        internal static Channel LoadChannel(JObject o, Twixel.APIVersion version)
        {
            if (version == Twixel.APIVersion.v2)
            {
                return new Channel((bool?)o["mature"],
                    (string)o["status"],
                    (string)o["display_name"],
                    (string)o["game"],
                    (long)o["_id"],
                    (string)o["name"],
                    (string)o["created_at"],
                    (string)o["updated_at"],
                    (string)o["logo"],
                    (string)o["banner"],
                    (string)o["video_banner"],
                    (string)o["background"],
                    (string)o["url"],
                    (JObject)o["_links"],
                    (JArray)o["teams"]);
            }
            else if (version == Twixel.APIVersion.v3)
            {
                return new Channel((bool?)o["mature"],
                    (string)o["status"],
                    (string)o["broadcaster_language"],
                    (string)o["display_name"],
                    (string)o["game"],
                    (int?)o["delay"],
                    (long)o["_id"],
                    (string)o["name"],
                    (string)o["created_at"],
                    (string)o["updated_at"],
                    (string)o["logo"],
                    (string)o["banner"],
                    (string)o["video_banner"],
                    (string)o["background"],
                    (string)o["profile_banner"],
                    (string)o["profile_banner_background_color"],
                    (bool?)o["partner"],
                    (string)o["url"],
                    (long?)o["views"],
                    (long?)o["followers"],
                    (JObject)o["_links"]);
            }
            else
            {
                throw new TwixelException("Channel: " + versionCannotBeNoneString);
            }
        }

        internal static Team LoadTeam(JObject o, Twixel.APIVersion version)
        {
            Team team = new Team((long)o["_id"],
                (string)o["name"],
                (string)o["info"],
                (string)o["display_name"],
                (string)o["created_at"],
                (string)o["updated_at"],
                (string)o["logo"],
                (string)o["banner"],
                (string)o["background"],
                version,
                (JObject)o["_links"]);
            return team;
        }

        internal static User LoadUser(JObject o,
            Twixel.APIVersion version)
        {
            if (version == Twixel.APIVersion.v2)
            {
                return new User((string)o["display_name"],
                    (long)o["_id"],
                    (string)o["name"],
                    (bool)o["staff"],
                    (string)o["created_at"],
                    (string)o["updated_at"],
                    (string)o["logo"],
                    (JObject)o["_links"]);
            }
            else if (version == Twixel.APIVersion.v3)
            {
                return new User((string)o["display_name"],
                    (long)o["_id"],
                    (string)o["name"],
                    (string)o["type"],
                    (string)o["bio"],
                    (string)o["created_at"],
                    (string)o["updated_at"],
                    (string)o["logo"],
                    (JObject)o["_links"]);
            }
            else
            {
                throw new TwixelException("User: " + versionCannotBeNoneString);
            }
        }

        internal static List<Stream> LoadStreams(JObject o, Twixel.APIVersion version)
        {
            List<Stream> streams = new List<Stream>();

            foreach (JObject obj in (JArray)o["streams"])
            {
                streams.Add(LoadStream(obj, (JObject)o["_links"], version));
            }

            return streams;
        }

        internal static Stream LoadStream(JObject o, JObject baseLinksO, Twixel.APIVersion version)
        {
            JObject channelO = (JObject)o["channel"];
            if (version == Twixel.APIVersion.v2)
            {
                return new Stream((long?)o["_id"],
                    (string)o["game"],
                    (long?)o["viewers"],
                    (string)o["created_at"],
                    (int?)o["video_height"],
                    (double?)o["average_fps"],
                    (JObject)o["_links"],
                    (string)o["name"],
                    (string)o["broadcaster"],
                    (string)o["preview"],
                    channelO,
                    baseLinksO);
            }
            else if (version == Twixel.APIVersion.v3)
            {
                return new Stream((long?)o["_id"],
                    (string)o["game"],
                    (long?)o["viewers"],
                    (string)o["created_at"],
                    (int)o["video_height"],
                    (double)o["average_fps"],
                    (JObject)o["_links"],
                    (string)o["name"],
                    (string)o["broadcaster"],
                    (JObject)o["preview"],
                    channelO,
                    baseLinksO);
            }
            else
            {
                throw new TwixelException("Stream: " + versionCannotBeNoneString);
            }
        }

        internal static List<FeaturedStream> LoadFeaturedStreams(JObject o, Twixel.APIVersion version)
        {
            List<FeaturedStream> streams = new List<FeaturedStream>();

            foreach (JObject obj in (JArray)o["featured"])
            {
                if (version == Twixel.APIVersion.v2)
                {
                    streams.Add(new FeaturedStream((string)obj["text"],
                        (string)obj["image"],
                        (JObject)obj["stream"],
                        (JObject)o["_links"]));
                }
                else if (version == Twixel.APIVersion.v3)
                {
                    streams.Add(new FeaturedStream((string)obj["text"],
                        (string)obj["image"],
                        (string)obj["title"],
                        (bool)obj["sponsored"],
                        (int)obj["priority"],
                        (bool)obj["scheduled"],
                        (JObject)obj["stream"],
                        (JObject)o["_links"]));
                }
            }

            return streams;
        }

        internal static Video LoadVideo(JObject o, Twixel.APIVersion version)
        {
            if (version == Twixel.APIVersion.v2)
            {
                return new Video((string)o["title"],
                    (string)o["description"],
                    (long)o["broadcast_id"],
                    (string)o["status"],
                    (string)o["_id"],
                    (string)o["recorded_at"],
                    (string)o["game"],
                    (long)o["length"],
                    (string)o["preview"],
                    (string)o["url"],
                    (string)o["embed"],
                    (long)o["views"],
                    (string)o["broadcast_type"],
                    (JObject)o["channel"],
                    (JObject)o["_links"]);
            }
            else if (version == Twixel.APIVersion.v3)
            {
                return new Video((string)o["title"],
                    (string)o["description"],
                    (long)o["broadcast_id"],
                    (string)o["status"],
                    (string)o["tag_list"],
                    (string)o["_id"],
                    (string)o["recorded_at"],
                    (string)o["game"],
                    (long)o["length"],
                    (string)o["preview"],
                    (string)o["url"],
                    (long)o["views"],
                    o["fps"].ToString(),
                    o["resolutions"].ToString(),
                    (string)o["broadcast_type"],
                    (JObject)o["channel"],
                    (JObject)o["_links"]);
            }
            else
            {
                throw new TwixelException("Video: " + versionCannotBeNoneString);
            }
        }

        internal static List<Game> LoadGames(JObject o, Twixel.APIVersion version)
        {
            List<Game> games = new List<Game>();
            foreach (JObject obj in (JArray)o["top"])
            {
                JObject game = (JObject)obj["game"];
                games.Add(new Game((long?)obj["viewers"],
                    (long?)obj["channels"],
                    (string)game["name"],
                    (long?)game["_id"],
                    (long?)game["giantbomb_id"],
                    (JObject)game["box"],
                    (JObject)game["logo"],
                    version,
                    null));
            }
            return games;
        }

        internal static List<SearchedGame> LoadSearchedGames(JObject o, Twixel.APIVersion version)
        {
            List<SearchedGame> games = new List<SearchedGame>();
            foreach (JObject obj in (JArray)o["games"])
            {
                games.Add(new SearchedGame((string)obj["name"],
                    (long?)obj["popularity"],
                    (long?)obj["_id"],
                    (long?)obj["giantbomb_id"],
                    (JObject)obj["box"],
                    (JObject)obj["logo"],
                    version,
                    null));
            }
            return games;
        }

        internal static Ingest LoadIngest(JObject o, JObject baseLinksO, Twixel.APIVersion version)
        {
            Ingest ingest = new Ingest((string)o["name"],
                (bool)o["default"],
                (long)o["_id"],
                (string)o["url_template"],
                (double)o["availability"],
                version,
                baseLinksO);
            return ingest;
        }

        internal static List<Emoticon> LoadEmoticons(JObject o,
            Twixel.APIVersion version)
        {
            List<Emoticon> emoticons = new List<Emoticon>();
            foreach (JObject obj in (JArray)o["emoticons"])
            {
                emoticons.Add(new Emoticon((string)obj["regex"],
                    (JArray)obj["images"],
                    version,
                    (JObject)o["_links"]));
            }
            return emoticons;
        }

        internal static List<Badge> LoadBadges(JObject o,
            Twixel.APIVersion version)
        {
            List<Badge> badges = new List<Badge>();
            List<string> names = new List<string>(
                new string[] {"global_mod", "admin", "broadcaster",
                "mod", "staff", "turbo", "subscriber"});
            JObject links = (JObject)o["_links"];
            foreach (string str in names)
            {
                JObject obj = null;
                if (o[str].Type != JTokenType.Null)
                {
                    obj = (JObject)o[str];
                }
                badges.Add(new Badge(str, obj, version, links));
            }
            return badges;
        }

        internal static List<Follow> LoadFollows(JObject o, Twixel.APIVersion version)
        {
            List<Follow> follows = new List<Follow>();
            foreach (JObject obj in (JArray)o["follows"])
            {
                follows.Add(LoadFollow(obj, version));
            }
            return follows;
        }

        internal static Follow LoadFollow(JObject o, Twixel.APIVersion version)
        {
            return new Follow((string)o["created_at"],
                (bool)o["notifications"],
                (JObject)o["user"],
                version,
                (JObject)o["_links"]);
        }

        internal static Total<T> LoadTotal<T>(JObject o, T t,
            Twixel.APIVersion version)
        {
            return new Total<T>((long)o["_total"],
                t,
                version,
                (JObject)o["_links"]);
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        /// <param name="source">A string</param>
        /// <remarks>Taken from http://www.dotnetperls.com/remove-html-tags </remarks>
        /// <returns>A string</returns>
        public static string RemoveHtmlTags(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        /// <summary>
        /// <![CDATA[Replaces all instances of &amp; with &]]>
        /// </summary>
        /// <param name="source">A string</param>
        /// <returns>A string</returns>
        public static string ConvertAmp(string source)
        {
            return source.Replace("&amp;", "&");
        }
    }
}
