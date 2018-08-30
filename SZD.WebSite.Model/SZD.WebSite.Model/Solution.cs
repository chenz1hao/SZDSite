using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZD.WebSite.Model
{
    public class Solution
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public int TypeID { get; set; }
        [ForeignKey("TypeID")]
        public virtual SolutionType SolutionTypes { get; set; }

        public string Html { get; set; }

        public int Order { get; set; }
    }
}
