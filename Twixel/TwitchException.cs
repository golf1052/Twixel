using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    /// <summary>
    /// Twitch exception
    /// </summary>
    public class TwitchException : Exception
    {
        /// <summary>
        /// Twitch error
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// HTTP error code
        /// </summary>
        public int Status { get; private set; }

        /// <summary>
        /// TwitchException constructor
        /// </summary>
        /// <param name="errorO">Error JSON object</param>
        public TwitchException(JObject errorO) : base((string)errorO["message"])
        {
            Error = (string)errorO["error"];
            Status = (int)errorO["status"];
        }
        
        public TwitchException(string message, string error, int status) : base(message)
        {
            Error = error;
            Status = status;
        }
    }
}
