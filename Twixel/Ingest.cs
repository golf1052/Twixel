using System;

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
