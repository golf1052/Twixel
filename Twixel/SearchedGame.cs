using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class SearchedGame : Game
    {
        public long? popularity;

        public SearchedGame(string name,
            long? popularity,
            long? id,
            long? giantBombId,
            JObject boxO,
            JObject logoO,
            Twixel.APIVersion version,
            JObject baseLinksO) : base(null, null, name, id, giantBombId, boxO, logoO, version, baseLinksO)
        {
            this.popularity = popularity;
        }
    }
}
