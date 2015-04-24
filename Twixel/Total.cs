using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Total object
    /// </summary>
    /// <typeparam name="T">Wrapped object</typeparam>
    public class Total<T> : TwixelObjectBase
    {
        /// <summary>
        /// Total number of objects, not just in wrapped
        /// </summary>
        public long? total;

        /// <summary>
        /// Wrapped object
        /// </summary>
        public T wrapped;

        /// <summary>
        /// Total constructor
        /// </summary>
        /// <param name="total">Total number of objects</param>
        /// <param name="t">Wrapped object</param>
        /// <param name="version">Twitch API version</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public Total(long? total, T t,
            Twixel.APIVersion version, JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.total = total;
            this.wrapped = t;
        }
    }
}
