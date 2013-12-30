using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwixelAPI
{
    public class WebUrl
    {
        public string urlString;
        public Uri url;

        public WebUrl(string urlString)
        {
            this.urlString = urlString;
            url = new Uri(urlString);
        }
    }
}
