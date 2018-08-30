using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZD.WebSite.Model
{
    public class Company
    {
        [Key]
        public int ID { get; set; }

        public string Introduction { get; set; }

        public string Culture { get; set; }

        public string Recruit { get; set; }
    }
}
