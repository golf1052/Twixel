using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TwixelAPI
{
    public class Total<T> : TwixelObjectBase
    {
        public long total;
        public T wrapped;

        public Total(long total, T t,
            Twixel.APIVersion version, JObject baseLinksO) : base(baseLinksO)
        {
            this.version = version;
            this.total = total;
            this.wrapped = t;
        }
    }
}
