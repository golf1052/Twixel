using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class TwitchException : Exception
    {
        public readonly string Error { get; private set; }
        public readonly int Status { get; private set; }

        public TwitchException(JObject errorO) : base((string)errorO["message"])
        {
            Error = (string)errorO["error"];
            Status = (int)errorO["status"];
        }
    }
}
