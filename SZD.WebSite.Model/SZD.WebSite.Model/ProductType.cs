using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZD.WebSite.Model
{
    public class ProductType
    {
        [Key]
        public int ID { get; set; }

        public string Type { get; set; }

        public string Remark { get; set; }
    }
}
