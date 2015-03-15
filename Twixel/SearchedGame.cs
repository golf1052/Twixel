using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// SearchedGame object
    /// </summary>
    public class SearchedGame : Game
    {
        /// <summary>
        /// Popularity
        /// </summary>
        public long? popularity;

        /// <summary>
        /// SearchedGame constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="popularity">Popularity</param>
        /// <param name="id">ID</param>
        /// <param name="giantBombId">GiantBomb ID</param>
        /// <param name="boxO">Box JSON object</param>
        /// <param name="logoO">Logo JSON object</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
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
