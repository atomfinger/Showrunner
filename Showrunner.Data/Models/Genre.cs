using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Models
{
    public class Genre
    {
        public Genre()
        {
            Shows = new HashSet<Show>();
        }

        public int Oid { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Show> Shows { get; set; }
    }
}
