using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Game
    {
        public string name;
        public Uri boxLarge;
        public Uri boxMedium;
        public Uri boxSmall;
        public Uri boxTemplate;
        public Uri logoLarge;
        public Uri logoMedium;
        public Uri logoSmall;
        public Uri logoTemplate;
        public long? id;
        public long? giantBombId;
        public int? viewers;
        public int? channels;

        public Game(string name, JObject boxO, JObject logoO, long? id, long? giantBombId, int? viewers, int? channels)
        {
            this.name = name;
            boxLarge = new Uri((string)boxO["large"]);
            boxMedium = new Uri((string)boxO["medium"]);
            boxSmall = new Uri((string)boxO["small"]);
            boxTemplate = new Uri((string)boxO["template"]);
            logoLarge = new Uri((string)logoO["large"]);
            logoMedium = new Uri((string)logoO["medium"]);
            logoSmall = new Uri((string)logoO["small"]);
            logoTemplate = new Uri((string)logoO["template"]);
            this.id = id;
            this.giantBombId = giantBombId;
            this.viewers = viewers;
            this.channels = channels;
        }
    }
}
