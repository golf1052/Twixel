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
        public Uri imagesThumb;
        public Uri imagesTiny;
        public Uri imagesSmall;
        public Uri imagesSuper;
        public Uri imagesMedium;
        public Uri imagesIcon;
        public Uri imagesScreen;
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
                imagesThumb = new Uri((string)o["thumb"]);
                imagesTiny = new Uri((string)o["tiny"]);
                imagesSmall = new Uri((string)o["small"]);
                imagesSuper = new Uri((string)o["super"]);
                imagesMedium = new Uri((string)o["medium"]);
                imagesIcon = new Uri((string)o["icon"]);
                imagesScreen = new Uri((string)o["screen"]);
            }
        }

        public Game ToGame()
        {
            return baseGame;
        }
    }
}
