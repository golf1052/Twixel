using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace TwixelAPI
{
    public class Team
    {
        public string info;
        public Uri background;
        public Uri banner;
        public string name;
        public long id;
        public string displayName;
        public Uri logo;

        public Team(string info,
            string background,
            string banner,
            string name,
            long id,
            string displayName,
            string logo)
        {
            this.info = info;
            if (background != null)
            {
                this.background = new Uri(background);
            }
            if (banner != null)
            {
                this.banner = new Uri(banner);
            }
            this.name = name;
            this.id = id;
            this.displayName = displayName;
            if (logo != null)
            {
                this.logo = new Uri(logo);
            }
        }
    }
}
