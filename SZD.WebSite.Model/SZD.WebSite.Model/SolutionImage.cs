using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZD.WebSite.Model
{
    public class SolutionImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SolutionID { get; set; }

        public byte[] Image { get; set; }

    }
}
