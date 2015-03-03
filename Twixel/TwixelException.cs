using System;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class TwixelException : Exception
    {
        public TwixelObjectBase BaseLinks { get; private set; }
        public TwitchException Error { get; private set; }

        public TwixelException(string message) : base(message)
        {
        }

        public TwixelException(string message, JObject baseLinksO) : base(message)
        {
            this.BaseLinks = new TwixelObjectBase(baseLinksO);
        }

        public TwixelException(string message, TwitchException error) : base(message, error)
        {
            this.Error = error;
        }
    }
}
