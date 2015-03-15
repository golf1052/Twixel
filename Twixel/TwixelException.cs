using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Twixel exception
    /// </summary>
    public class TwixelException : Exception
    {
        /// <summary>
        /// Base links
        /// </summary>
        public TwixelObjectBase BaseLinks { get; private set; }

        /// <summary>
        /// Wrapped Twitch exception
        /// </summary>
        public TwitchException Error { get; private set; }

        /// <summary>
        /// TwixelException constructor. Simple messages.
        /// </summary>
        /// <param name="message">Message</param>
        public TwixelException(string message) : base(message)
        {
        }

        /// <summary>
        /// TwixelException constructor. Message with base links.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="baseLinksO">Base links JSON object</param>
        public TwixelException(string message, JObject baseLinksO) : base(message)
        {
            this.BaseLinks = new TwixelObjectBase(baseLinksO);
        }

        /// <summary>
        /// TwixelException constructor. Message with wrapped Twitch exception.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="error">Twitch exception</param>
        public TwixelException(string message, TwitchException error) : base(message, error)
        {
            this.Error = error;
        }
    }
}
