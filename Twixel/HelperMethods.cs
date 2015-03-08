using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public static class HelperMethods
    {
        internal static Dictionary<string, Uri> LoadLinks(JObject o)
        {
            Dictionary<string, Uri> links = JsonConvert.DeserializeObject<Dictionary<string, Uri>>(o.ToString());
            return links;
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
                return null;
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

        //internal static User LoadUser(JObject o)
        //{
        //    User user = new User((string)o["name"],
        //        (string)o["logo"],
        //        (long)o["_id"],
        //        (string)o["display_name"],
        //        (bool?)o["staff"],
        //        (string)o["created_at"],
        //        (string)o["updated_at"],
        //        (string)o["bio"]);
        //    return user;
        //}

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
                return null;
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
