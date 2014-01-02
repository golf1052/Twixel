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
        public WebUrl background;
        public WebUrl banner;
        public string name;
        public long id;
        public string displayName;
        public WebUrl logo;

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
                this.background = new WebUrl(background);
            }
            if (banner != null)
            {
                this.banner = new WebUrl(banner);
            }
            this.name = name;
            this.id = id;
            this.displayName = displayName;
            if (logo != null)
            {
                this.logo = new WebUrl(logo);
            }
        }
    }
}
