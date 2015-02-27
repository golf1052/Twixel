using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Threading.Tasks;

namespace TwixelAPI
{
    public class Ingest
    {
        public string name;
        public bool defaultIngest;
        public long id;
        public Uri urlTemplate;
        public double avalibility;

        public Ingest(string name,
            bool defaultIngest,
            long id,
            string urlTemplate,
            double avalibility)
        {
            this.name = name;
            this.defaultIngest = defaultIngest;
            this.id = id;
            this.urlTemplate = new Uri(urlTemplate);
            this.avalibility = avalibility;
        }
    }
}
