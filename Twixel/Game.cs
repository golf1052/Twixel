using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace TwixelAPI
{
    public class Game
    {
        public string name;
        public WebUrl boxLarge;
        public WebUrl boxMedium;
        public WebUrl boxSmall;
        public WebUrl boxTemplate;
        public WebUrl logoLarge;
        public WebUrl logoMedium;
        public WebUrl logoSmall;
        public WebUrl logoTemplate;
        public long? id;
        public long? giantBombId;
        public int? viewers;
        public int? channels;

        public Game(string name, JObject boxO, JObject logoO, long? id, long? giantBombId, int? viewers, int? channels)
        {
            this.name = name;
            boxLarge = new WebUrl((string)boxO["large"]);
            boxMedium = new WebUrl((string)boxO["medium"]);
            boxSmall = new WebUrl((string)boxO["small"]);
            boxTemplate = new WebUrl((string)boxO["template"]);
            logoLarge = new WebUrl((string)logoO["large"]);
            logoMedium = new WebUrl((string)logoO["medium"]);
            logoSmall = new WebUrl((string)logoO["small"]);
            logoTemplate = new WebUrl((string)logoO["template"]);
            this.id = id;
            this.giantBombId = giantBombId;
            this.viewers = viewers;
            this.channels = channels;
        }
    }
}
