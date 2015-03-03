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
            Dictionary<string, string> stringLinks = JsonConvert.DeserializeObject<Dictionary<string, string>>(o.ToString());
            Dictionary<string, Uri> links = new Dictionary<string, Uri>();
            foreach (var link in stringLinks)
            {
                links.Add(link.Key, new Uri(link.Value));
            }
            return links;
        }

        internal static Channel LoadChannel(JObject o)
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
                (long?)o["followers"]);
            return channel;
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

        internal static User LoadUser(JObject o)
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

        internal static List<Stream> LoadStreams(JObject o)
        {
            List<Stream> streams = new List<Stream>();
            nextStreams = new Uri((string)o["_links"]["next"]);

            foreach (JObject obj in (JArray)o["streams"])
            {
                streams.Add(LoadStream(obj, (string)o["_links"]["channel"]));
            }

            return streams;
        }

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
                    o);
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
                    o);
            }
            else
            {
                return null;
            }
        }
    }
}
