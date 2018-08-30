using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SZD.WebSite.Model;

namespace SZDWebSite.ViewModels
{
    public class ProductViewModel
    {
        public List<string> ProductType { get; set; }
        public List<Product> Product { get; set; }
    }
}
