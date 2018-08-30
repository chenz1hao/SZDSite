using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SZDWebSite.ViewModels
{
    public class ManageIndexModel
    {
        public List<ManageIndexView> ManageIndex { get; set; }
    }
    public class ManageIndexView
    {
        public int Nid { get; set; }
        public string Ntitle { get; set; }
        public DateTime Ndate { get; set; }
        public int Ntype { get; set; }
        public string Nauthor { get; set; }
        public string Ndesc { get; set; }
    }
}
