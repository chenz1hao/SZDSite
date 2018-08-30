using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SZD.WebSite.Model;

namespace SZDWebSite.ViewModels
{
    public class EditProductModel
    {
        public Product Product { get; set; }
        public List<string> ProductType { get; set; }
    }
}
