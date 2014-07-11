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
    public class SearchedGame : Game
    {
        public WebUrl imagesThumb;
        public WebUrl imagesTiny;
        public WebUrl imagesSmall;
        public WebUrl imagesSuper;
        public WebUrl imagesMedium;
        public WebUrl imagesIcon;
        public WebUrl imagesScreen;
        public int? popularity;
        Game baseGame;

        public SearchedGame(string name,
            JObject boxO,
            JObject logoO,
            long? id,
            long? giantBombId,
            int? viewers,
            int? channels,
            JObject imagesO,
            int? popularity) : base(name, boxO, logoO, id, giantBombId, viewers, channels)
        {
            this.popularity = popularity;
            LoadImages(imagesO);
            baseGame = new Game(name, boxO, logoO, id, giantBombId, 0, 0);
        }

        // Also a little note Twitch NEVER uses this...
        void LoadImages(JObject o)
        {
            if (o != null)
            {
                imagesThumb = new WebUrl((string)o["thumb"]);
                imagesTiny = new WebUrl((string)o["tiny"]);
                imagesSmall = new WebUrl((string)o["small"]);
                imagesSuper = new WebUrl((string)o["super"]);
                imagesMedium = new WebUrl((string)o["medium"]);
                imagesIcon = new WebUrl((string)o["icon"]);
                imagesScreen = new WebUrl((string)o["screen"]);
            }
        }

        public Game ToGame()
        {
            return baseGame;
        }
    }
}
