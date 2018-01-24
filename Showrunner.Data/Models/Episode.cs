using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Models
{
    public class Episode
    {
        public int Oid { get; set; }
        public Show Show { get; set; }
        public int ShowId { get; set; }
        public int? ApiId { get; set; }
        public string Title { get; set; }
        public int? Season { get; set; }
        public DateTime? AirDate { get; set; }
    }
}
