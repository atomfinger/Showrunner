using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Showrunner.Data.Models
{
    public class Network
    {
        public Network()
        {
            this.Shows = new HashSet<Show>();
        }

        public int Oid { get; set; }
        public int ApiId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Show> Shows { get; set; }
    }
}
