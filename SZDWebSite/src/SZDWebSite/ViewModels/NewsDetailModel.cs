using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SZD.WebSite.Model;

namespace SZDWebSite.ViewModels
{
    public class NewsDetailModel
    {
        public News ThisNews { get; set; }
        public News NextNews { get; set; }
        public News PreNews { get; set; }
        public string Author { get; set; }
    }
}
