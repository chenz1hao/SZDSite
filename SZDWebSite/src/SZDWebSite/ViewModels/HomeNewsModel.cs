using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZDWebSite.ViewModels
{
    public class HomeNewsModel
    {
        public List<GsywViews> IndexGsywViewsList { get; set; }
    }

    public class GsywViews
    {
        public int Nid { get; set; }
        public string Ntitle { get; set; }
        public string Ndesc { get; set; }
        public string Ndate { get; set; }
    }
}
