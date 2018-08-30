using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SZD.WebSite.Model;

namespace SZDWebSite.ViewModels
{
    public class SolutionViewModel
    {
        public List<string> SolutionType { get; set; }
        public List<Solution> Solution { get; set; }
    }
}
