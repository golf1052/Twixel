using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                return new Channel((bool)o["mature"],
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
                return new Channel((bool)o["mature"],
                    (string)o["status"],
                    (string)o["broadcaster_language"],
                    (string)o["display_name"],
                    (string)o["game"],
                    (int)o["delay"],
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
                    (bool)o["partner"],
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

        internal static Team LoadTeam(JObject o)
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

        //internal static List<Stream> LoadStreams(JObject o)
        //{
        //    List<Stream> streams = new List<Stream>();
        //    nextStreams = new Uri((string)o["_links"]["next"]);

        //    foreach (JObject obj in (JArray)o["streams"])
        //    {
        //        streams.Add(LoadStream(obj, (string)o["_links"]["channel"]));
        //    }

        //    return streams;
        //}

        internal static Stream LoadStream(JObject o, Twixel.APIVersion version)
        {
            JObject streamO = (JObject)o["stream"];
            JObject channelO = (JObject)streamO["channel"];
            if (version == Twixel.APIVersion.v2)
            {
                return new Stream((long?)streamO["_id"],
                    (string)streamO["game"],
                    (long?)streamO["viewers"],
                    (string)streamO["created_at"],
                    (int)streamO["video_height"],
                    (double)streamO["average_fps"],
                    (JObject)streamO["_links"],
                    (string)streamO["name"],
                    (string)streamO["broadcaster"],
                    (string)streamO["preview"],
                    channelO,
                    (JObject)o["_links"]);
            }
            else if (version == Twixel.APIVersion.v3)
            {
                return new Stream((long?)streamO["_id"],
                    (string)streamO["game"],
                    (long?)streamO["viewers"],
                    (string)streamO["created_at"],
                    (int)streamO["video_height"],
                    (double)streamO["average_fps"],
                    (JObject)streamO["_links"],
                    (string)streamO["name"],
                    (string)streamO["broadcaster"],
                    (JObject)streamO["preview"],
                    channelO,
                    (JObject)o["_links"]);
            }
            else
            {
                return null;
            }
        }
    }
}
