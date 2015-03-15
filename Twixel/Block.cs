using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Block object
    /// </summary>
    public class Block : TwixelObjectBase
    {
        /// <summary>
        /// Last updated
        /// </summary>
        public DateTime updatedAt;

        /// <summary>
        /// Block ID
        /// </summary>
        public long id;

        /// <summary>
        /// User that is blocked
        /// </summary>
        public User user;

        /// <summary>
        /// Block constructor
        /// </summary>
        /// <param name="updatedAt">Last updated at as a string</param>
        /// <param name="id">Block ID</param>
        /// <param name="userO">User JSON object</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Block(string updatedAt,
            long id,
            JObject userO,
            Twixel.APIVersion version,
            JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.updatedAt = DateTime.Parse(updatedAt);
            this.id = id;
            this.user = HelperMethods.LoadUser(userO, version);
        }
    }
}
