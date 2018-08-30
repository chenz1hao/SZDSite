using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZD.WebSite.Model
{
    public class News
    {
        [Key]
        public int ID { get; set; }

        public string Title { get; set; }

        public string Html { get; set; }

        public DateTime Date { get; set; }

        public string Desc { get; set; }

        public int Type { get; set; }

        public int UID { get; set; }
        [ForeignKey("UID")]
        public virtual User Users { get; set; }

        public byte[] Image { get; set; }
    }
}
